using UnityEngine;


public class BitPicture : BitControl
{
    #region Appearance

    [SerializeField]
    protected bool _alphaBlend = true;

    [SerializeField]
    protected float _imageAspect;

    [SerializeField]
    protected ScaleMode _scaleMode = ScaleMode.StretchToFill;

    public bool IsLoading1
    {
        get { return _isLoading; }
    }

    public ScaleMode ScaleMode
    {
        get { return _scaleMode; }
        set { _scaleMode = value; }
    }

    public bool AlphaBlend
    {
        get { return _alphaBlend; }
        set { _alphaBlend = value; }
    }

    public float ImageAspect
    {
        get { return _imageAspect; }
        set { _imageAspect = value; }
    }

    #endregion


    private bool _shouldReload;
    private bool _isLoading;

    private object _loadKey;

    public object LoadKey
    {
        get { return _loadKey; }
        set
        {
            if (_loadKey == null || Image == null || (_loadKey != null && !_loadKey.Equals(value)))
                _shouldReload = true;
            _loadKey = value;
        }
    }

    private BitStage.PoolCallback<Texture2D> _imageCallback;

    private bool _lastState=true;

    public void Update()
    {
        base.Update();
        if ((_lastState == IsCurrentlyBeingDrawed) && (!_shouldReload))
            return;

        _lastState = IsCurrentlyBeingDrawed;
        if (_shouldReload)
        {
            UnloadImage(); // unload previous image
            _shouldReload = false;
        }
        if (IsCurrentlyBeingDrawed)
        {
            LoadImage();
        }
        else
        {
            UnloadImage();
        }
    }

    private void UnloadImage()
    {
        if (_loadKey == null)
            return;

        //BitStage.CustomAssetLoader.LogWarning("IMAGE " + name + "NOT VISIBLE");

        if (Image != null)
        {
            Texture2D asset = (Texture2D)Image;
            Image = null;
            BitStage.CustomAssetLoader.RemoveToAssetPool(asset);
        }
    }

    private void LoadImage()
    {
        if (_loadKey == null)
            return;

        //BitStage.CustomAssetLoader.LogWarning("IMAGE " + name + "VISIBLE");

        if (_imageCallback == null)
            _imageCallback = ImageCallback;
        _isLoading = true;
        BitStage.CustomAssetLoader.GetFromAssetPool(_loadKey, _imageCallback);
    }


    #region Draw

    protected override void DoDraw()
    {
        if (Image != null)
        {
            Color color = GUI.color;
            if (!GUI.enabled)
            {
                Color temp = color;
                temp.a = 0.3f;
                GUI.color = temp;
            }
            GUI.DrawTexture(Position, Image, ScaleMode, AlphaBlend, ImageAspect);
            GUI.color = color;
        }
    }

    private void ImageCallback(Texture2D texture)
    {
        Image = texture;
        _isLoading = false;
    }

    #endregion

    public override void OnDrawGizmos()
    {
        OnDrawGizmos(SelectedInEditor ? Color.yellow : Color.blue);
    }
}