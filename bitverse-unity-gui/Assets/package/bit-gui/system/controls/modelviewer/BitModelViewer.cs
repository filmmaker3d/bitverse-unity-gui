using Bitverse.Unity.Gui;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class BitModelViewer : BitBox
{
    [SerializeField]
    private GameObject _target;

    public GameObject target
    {
        get { return _target; }
        set { _target = value; setTarget(_target); }
    }

    private RenderTexture _renderTarget;
    private float _width, _height;
    private float _targetSize;
    private Vector3 _targetCenter;

    private void setRTandCamera()
    {
        _width = Position.width;
        _height = Position.height;
        _renderTarget = new RenderTexture((int) _width, (int) _height, 24);
        camera.targetTexture = _renderTarget;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.cullingMask = 1 << LayerHelper.GetLayers(LayerHelper.LayerContext.ModelViewer);
        //Debug.LogWarning("BitModelViewer old aspect: " + camera.aspect, this);
        camera.aspect = _width / _height;
        //Debug.LogWarning("BitModelViewer new aspect: " + camera.aspect, this);
        Content = new GUIContent(camera.targetTexture);
        //TODO: replace this with a shader that create a discrete alpha mask with the z buffer channel
        /*if (GetComponent<BlendColor>() == null)
        {
            BlendColor bc = gameObject.AddComponent<BlendColor>();
            bc.blendColor = new Color(0,0,0,1);
        }*/

        if(!camera.gameObject.GetComponent<BloomEffect>())
        {
            camera.gameObject.AddComponent<BloomEffect>();
        }
        if (!camera.gameObject.GetComponent<GlowThresholdEffect>())
        {
            camera.gameObject.AddComponent<GlowThresholdEffect>();
        }
        
    }
    public void CorrectAspect()
    {
        _width = Position.width;
        _height = Position.height;
        //Debug.LogWarning("BitModelViewer old aspect: " + camera.aspect, this);
        camera.aspect = _width / _height;
        //Debug.LogWarning("BitModelViewer new aspect: " + camera.aspect, this);
    }

    private void setTarget(GameObject theTarget)
    {
        if (theTarget == null)
            return;
        _targetSize = 0;
        _targetCenter = new Vector3();
        //Vector3 objectPosition = theTarget.transform.root.position;

        Bounds newBounds = new Bounds();

        Renderer[] renderers = theTarget.GetComponentsInChildren<Renderer>(true);
        //Debug.Log("Number of renderers: " + renderers.Length);
        foreach (Renderer meshRenderer in renderers)
        {
            newBounds.Encapsulate(meshRenderer.bounds);
        }

        //MeshFilter[] meshFilters = theTarget.GetComponentsInChildren<MeshFilter>(true);
        ////Debug.Log("Number of renderers: " + renderers.Length);
        //foreach (MeshFilter meshFilter in meshFilters)
        //{
        //    newBounds.Encapsulate(meshFilter.mesh.bounds);
        //}

        _targetCenter = newBounds.center;
        _targetSize = newBounds.extents.magnitude;

        gameObject.camera.farClipPlane = Mathf.Max(0.5f, _targetSize * 3.0f);
        gameObject.camera.transform.position = theTarget.transform.position + _targetCenter + (camera.transform.forward * -_targetSize) * 1.5f;
        //Debug.LogWarning("Positioned at " + gameObject.camera.transform.position);

        Transform[] childs = theTarget.GetComponentsInChildren<Transform>(true);

        int monitorlayer = LayerHelper.GetLayers(LayerHelper.LayerContext.ModelViewer);
        if(monitorlayer != 0)
            theTarget.layer = monitorlayer;

        foreach (Transform child in childs)
        {
            child.gameObject.layer = monitorlayer;
        }

    }

    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            base.Visible = value;
            if(ModelViewerLight != null) //AgentAddon changes this in runtime, so we gotta use here also :( ..
            {
                ModelViewerLight.cullingMask = 1 << LayerHelper.GetLayers(LayerHelper.LayerContext.ModelViewer);
            }
        }
    }

    public GameObject LightGameObject;
    public Light ModelViewerLight;
    public override void Start()
    {
        if (target != null && ShouldMove)
        {
            setTarget(target);
        }

        //Create Light
        if (LightGameObject == null)
        {
            LightGameObject = new GameObject("Model Viewer Light");
            LightGameObject.transform.parent = transform.parent; //To get the addon transform
            LightGameObject.transform.Rotate(90, 0, 0);//Quaternion.identity;
            // Add the light component
            ModelViewerLight = LightGameObject.AddComponent<Light>();
            ModelViewerLight.type = LightType.Directional;
            ModelViewerLight.intensity = 0.3f;
            ModelViewerLight.cullingMask = 1 << LayerHelper.GetLayers(LayerHelper.LayerContext.ModelViewer);
        }
        
        //ATTN: this should be set from the outside and not here, its only a test
        CameraHandler += onPositionCamera;

        setRTandCamera();
     }

    public bool ShouldMove = true;
    public event PositionCameraHandler CameraHandler;

    //private float _logTimeInterval = 5;
    public void Update()
    {
        if (target == null || !TopWindow.Visible)
        {
            if (camera && camera.enabled)
                camera.enabled = false;
            return;
        }
        
        if (_width != Position.width || _height != Position.height)
        {
            //Debug.LogWarning("BitModelViewer Width: " + _width + "BitModelViewer Height: " + _height, this);
            setRTandCamera();
        }

        float targetAspect = _width / _height;
        if (camera && targetAspect != camera.aspect)
        {
            //Debug.LogWarning("Camera Aspect is not the same as target aspect, changing it..", this);
            CorrectAspect();
        }

        if (CameraHandler != null && ShouldMove)
            CameraHandler(this, gameObject.camera);
        
        //if(CameraHandler == null && Time.time > _logTimeInterval + _lastLog)
        //{
        //    _lastLog = Time.time;
        //    Logger.Warn("Camera Handler is null!", this);
        //}
        if (camera && !camera.enabled)
            camera.enabled = true;
    }

    //ATTN: This function is only for testing purposes. This should be replaced by an outside callback
    public void onPositionCamera(object sender, Camera theCamera)
    {
        target.transform.RotateAround(_targetCenter, target.transform.up, 45.0f * Time.deltaTime);
    }

    public void OnDisable()
    {
        if(_renderTarget != null)
            _renderTarget.Release();
    }
}