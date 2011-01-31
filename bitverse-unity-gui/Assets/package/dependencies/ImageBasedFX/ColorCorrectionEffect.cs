
using UnityEngine;

//[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color Correction")]
public class ColorCorrectionEffect : ImageEffectBase
{
    private Texture _textureRamp;

	public float rampOffsetR;
	public float rampOffsetG;
	public float rampOffsetB;

	private const string shaderName = "ColorCorrectionEffect";

    public Texture TextureRamp
    {
        get { return _textureRamp; }
        set
        {
            if (value != null)
            {
                value.wrapMode = TextureWrapMode.Clamp;
            }
            _textureRamp = value;
        }
    }

    private bool _shouldDisable;
    void Start()
	{

        #region HARDCODED DISABLE
        //_shouldDisable = true;
        //enabled = false;
        //Destroy(this);
        //return;
        #endregion


        Initialize(shaderName);

        //Disable if Quality Settings not enough
        if (QualitySettings.currentLevel.Equals(QualityLevel.Fastest) || QualitySettings.currentLevel.Equals(QualityLevel.Fast))
        {
            Debug.LogError("DISABLING COLOR CORRECTION!", this);
            enabled = false;
        }

        if (QualitySettings.currentLevel.Equals(QualityLevel.Simple))
        {
            Debug.LogWarning("CCE - Simple");
        }
        if(QualitySettings.currentLevel.Equals(QualityLevel.Good))
        {
            Debug.LogWarning("CCE - GOOD");
        }
        if (QualitySettings.currentLevel.Equals(QualityLevel.Beautiful))
        {
            Debug.LogWarning("CCE - Beautiful");
        }
        if (QualitySettings.currentLevel.Equals(QualityLevel.Fantastic))
        {
            Debug.LogWarning("CCE - Fantastic");
        }

	}

	// Called by camera to apply image effect
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!Initialized || TextureRamp == null || _shouldDisable)
		{
            Graphics.Blit(source, destination);
            return;
		}
        
		material.SetTexture("_RampTex", TextureRamp);
		//material.SetVector("_RampOffset", new Vector4(rampOffsetR, rampOffsetG, rampOffsetB, 0));
		Graphics.Blit(source, destination, material);
		//ImageEffectsArt.BlitWithMaterial(material, source, destination);
	}
}