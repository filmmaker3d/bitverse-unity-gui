using UnityEngine;

//[ExecuteInEditMode]
public class RadialBlur : ImageEffectBase, IPPShaderInterface
{
	#region ShaderInterface Implementation
	private int queueOrder = 150;
	public int QueueOrder
	{
		get { return queueOrder; }
		set { queueOrder = value; }
	}

	private string _name = "RadialBlur";
	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}

	public bool Enabled
	{
		get { return enabled; }
		set { enabled = value; }
	}

	#endregion

	public float blurStrength = 2.2f;
	public float blurWidth = 1.0f;
	private bool isOpenGL = false;
    //private const string shaderNameOGL = "RadialBlurOGL";
    //private const string shaderNameDX = "RadialBlurDX";
    private const string shaderName = "RadialBlur";

	void Start()
	{
        isOpenGL = SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL");

        Initialize(shaderName);
	}

	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		if (!Initialized)
		{
			Debug.LogWarning("Not initialized!", this);
		    Graphics.Blit(source, dest);
			return;
		}

        //If we run in OpenGL mode, our UV coords are
        //not in 0-1 range, because of the texRECT sampler
        float ImageWidth = 1;
        float ImageHeight = 1;
        if (isOpenGL)
        {
            ImageWidth = source.width;
            ImageHeight = source.height;
        }

        material.SetFloat("_BlurStrength", blurStrength);
        material.SetFloat("_BlurWidth", blurWidth);
        material.SetFloat("_iHeight", ImageWidth);
        material.SetFloat("_iWidth", ImageHeight);
        //ImageEffects.BlitWithMaterial(GetMaterial(), source, dest);
        Graphics.Blit(source, dest, material);
	}
}