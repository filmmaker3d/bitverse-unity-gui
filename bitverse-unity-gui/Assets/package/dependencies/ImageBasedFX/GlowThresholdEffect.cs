
using UnityEngine;

// Glow uses the alpha channel as a source of "extra brightness".
// All builtin Unity shaders output baseTexture.alpha * color.alpha, plus
// specularHighlight * specColor.alpha into that.
// Usually you'd want either to make base textures to have zero alpha; or
// set the color to have zero alpha (by default alpha is 0.5).

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Glow Threshold")]
public class GlowThresholdEffect : MonoBehaviour, IPPShaderInterface
{
	#region ShaderInterface Implementation
	private int queueOrder = 100;
	public int QueueOrder
	{
		get { return queueOrder; }
		set { queueOrder = value; }
	}
	private string name = "Glow";
	public string Name
	{
		get { return name; }
		set { name = value; }
	}

	public bool Enabled
	{
		get { return enabled; }
		set { enabled = value; }
	}
    #endregion



	//HARDCODED
	public int downsampleAmount = 4;
    /// The color bightness threshold for applying blur.
	public float glowThreshold = 0;

	/// The brightness of the glow. Values larger than one give extra "boost".
    public float glowIntensity = 1.1f;

	/// Blur iterations - larger number means more blur.
	public int blurIterations = 2;

	/// Blur spread for each iteration. Lower values
	/// give better looking blur, but require more iterations to
	/// get large blurs. Value is usually between 0.5 and 1.0.
    public float blurSpread = 0.58f;

	/// Tint glow with this color. Alpha adds additional glow everywhere.
	public Color glowTint = new Color(1, 1, 1, 0);


	// --------------------------------------------------------
	// The final composition shader:
	//   adds (glow color * glow alpha * amount) to the original image.
	// In the combiner glow amount can be only in 0..1 range; we apply extra
	// amount during the blurring phase.

	private static string compositeMatString =
@"Shader ""GlowCompose"" {
	Properties {
		_Color (""Glow Amount"", Color) = (1,1,1,1)
		_MainTex ("""", RECT) = ""white"" {}
	}
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			Blend One One
			SetTexture [_MainTex] {constantColor [_Color] combine constant * texture DOUBLE}
		}
	}
	Fallback off
}";

	static Material m_CompositeMaterial = null;
	protected static Material compositeMaterial
	{
		get
		{
			if (m_CompositeMaterial == null)
			{
				m_CompositeMaterial = new Material(compositeMatString);
				m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
				m_CompositeMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_CompositeMaterial;
		}
	}


	// --------------------------------------------------------
	// The blur iteration shader.
	// Basically it just takes 4 texture samples and averages them.
	// By applying it repeatedly and spreading out sample locations
	// we get a Gaussian blur approximation.
	// The alpha value in _Color would normally be 0.25 (to average 4 samples),
	// however if we have glow amount larger than 1 then we increase this.

	private static string blurMatString =
@"Shader ""GlowConeTap"" {
	Properties {
		_Color (""Blur Boost"", Color) = (0,0,0,0.25)
		_MainTex ("""", RECT) = ""white"" {}
	}
	SubShader {
		Pass {
			ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant alpha}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
			SetTexture [_MainTex] {constantColor [_Color] combine texture * constant + previous}
		}
	}
	Fallback off
}";

	static Material m_BlurMaterial = null;
	protected static Material blurMaterial
	{
		get
		{
			if (m_BlurMaterial == null)
			{
				m_BlurMaterial = new Material(blurMatString);
				m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
				m_BlurMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_BlurMaterial;
		}
	}


	// --------------------------------------------------------
	// The image downsample shaders for each brightness mode.
	// It is in external assets as it's quite complex and uses Cg.

	public Shader downsampleShader;
	Material m_DownsampleMaterial = null;
	protected Material downsampleMaterial
	{
		get
		{
			if (m_DownsampleMaterial == null)
			{
				m_DownsampleMaterial = new Material(downsampleShader);
				m_DownsampleMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_DownsampleMaterial;
		}
	}


	// --------------------------------------------------------
	//  finally, the actual code

	protected void OnDisable()
	{
		if (m_CompositeMaterial)
		{
			DestroyImmediate(m_CompositeMaterial.shader);
			DestroyImmediate(m_CompositeMaterial);
		}
		if (m_BlurMaterial)
		{
			DestroyImmediate(m_BlurMaterial.shader);
			DestroyImmediate(m_BlurMaterial);
		}
		if (m_DownsampleMaterial)
			DestroyImmediate(m_DownsampleMaterial);
	}

	protected void Start()
	{

        #region Hardcoded disable!
        //enabled = false;
        //return;
        #endregion

		downsampleShader = (Shader)Resources.Load("GlowEffectThresholdDownsample");
		GlowReplaceShader = (Shader)Resources.Load("GlowReplace");

		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}

		// Disable the effect if no Downsample shader is setup
		if (downsampleShader == null)
		{
			Debug.Log("No Downsample shader assigned! Disabling glow.");
			enabled = false;
		}

		// Disable the effect if no Glow Replace shader is setup
		if (downsampleShader == null)
		{
			Debug.Log("No Glow Replace shader assigned! Disabling glow.");
			enabled = false;
		}

		// Disable if any of the shaders can't run on the users graphics card
		else
		{
			if (!blurMaterial.shader.isSupported)
				enabled = false;
			if (!compositeMaterial.shader.isSupported)
				enabled = false;
			if (!downsampleMaterial.shader.isSupported)
				enabled = false;
		}

        //Disable if Quality Settings not enough
        if (QualitySettings.currentLevel == QualityLevel.Fastest || QualitySettings.currentLevel == QualityLevel.Fast || QualitySettings.currentLevel == QualityLevel.Simple)
        {
            Debug.LogError("DISABLING GLOW!", this);
            enabled = false;
        }

	}

	public Shader GlowReplaceShader;
	private RenderTexture glowTexture;
	// The camera that renders the replacement shaders (Don’t access this directly, use GetPPCamera())
	private GameObject shaderCamera;
	/**
	* Camera that renders the replacement shaders
	* ppCamera getter
	* @return
	*/

	private Camera GetPPCamera()
	{
		// Create the shader camera if it doesn’t exist yet
		if (!shaderCamera)
		{
			shaderCamera = new GameObject("ShaderCamera", typeof(Camera));
			shaderCamera.camera.enabled = false;
			shaderCamera.hideFlags = HideFlags.HideAndDontSave;
		}

		return shaderCamera.camera;
	}

	/**
	* Cleanup all resources used for Post Processing
	*/
	private void CleanResources()
	{
		if (shaderCamera)
		{
			DestroyImmediate(shaderCamera);
		}
		CleanRenderTextures();
	}

	private void CleanRenderTextures()
	{
		if (glowTexture != null)
		{
			RenderTexture.ReleaseTemporary(glowTexture);
			glowTexture = null;
		}
	}

	private void OnPreRender()
	{
		if (!enabled || !gameObject.active)
			return;

		// Start from nothing
		CleanRenderTextures();

		// Reference to ppCamera’s camera
		Camera cam = GetPPCamera();

		cam.CopyFrom(camera);
		cam.cullingMask = camera.cullingMask;
		cam.backgroundColor = new Color(0, 0, 0, 0);
		cam.clearFlags = CameraClearFlags.SolidColor;


		//Render Blur Texture
		glowTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        glowTexture.wrapMode = TextureWrapMode.Clamp; //Testing
		cam.targetTexture = glowTexture;
		cam.RenderWithShader(GlowReplaceShader, "RenderEffect");
	}


    // Performs one blur iteration.
    public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    {
        float off = iteration * blurSpread;
        Graphics.BlitMultiTap(source, dest, blurMaterial,
            new Vector2(off, off),
            new Vector2(-off, off),
            new Vector2(off, -off),
            new Vector2(-off, -off)
        );
    }
    #region OLD
    // Performs one blur iteration.
    //public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
    //{
    //    RenderTexture.active = dest;
    //    blurMaterial.SetTexture("_MainTex", source);

    //    float offsetX = (.5F + iteration * blurSpread) / (float)source.width;
    //    float offsetY = (.5F + iteration * blurSpread) / (float)source.height;

    //    //offsetX = 2f / (float)source.width;
    //    //offsetY = 2f / (float)source.height;

    //    GL.PushMatrix();
    //    GL.LoadOrtho();

    //    for (int i = 0; i < blurMaterial.passCount; i++)
    //    {
    //        blurMaterial.SetPass(i);
    //        Render4TapQuad(dest, offsetX, offsetY);
    //    }
    //    GL.PopMatrix();
    //}
    #endregion

	// Downsamples the texture to a set resolution.
	private void DownSample(RenderTexture source, RenderTexture dest)
	{
		downsampleMaterial.SetFloat("_DownsampleAmount", downsampleAmount);

		// Remove colors below the threshold.
		downsampleMaterial.SetFloat("_GlowThreshold", glowThreshold);

		downsampleMaterial.color = new Color(glowTint.r, glowTint.g, glowTint.b, glowTint.a / downsampleAmount);
		Graphics.Blit(source, dest, downsampleMaterial);
		//ImageEffects.BlitWithMaterial( downsampleMaterial, source, dest );
	}

	public RenderTexture ApplyEffect(RenderTexture source, RenderTexture destination)
	{
		if (!gameObject.active)
		{
			return null;
		}

		#region Clamp parameters to sane values
		glowThreshold = Mathf.Clamp(glowThreshold, 0.0f, 1.0f);
		glowIntensity = Mathf.Clamp(glowIntensity, 0.0f, 10.0f);
		blurIterations = Mathf.Clamp(blurIterations, 0, 30);
		blurSpread = Mathf.Clamp(blurSpread, 0.0f, 1.0f);
		#endregion

		RenderTexture buffer = RenderTexture.GetTemporary(source.width / downsampleAmount, source.height / downsampleAmount, 0);
		RenderTexture buffer2 = RenderTexture.GetTemporary(source.width / downsampleAmount, source.height / downsampleAmount, 0);

		// Copy glow texture to the 4x4 smaller texture.
		DownSample(glowTexture, buffer);

		// Blur the small texture
		float extraBlurBoost = Mathf.Clamp01((glowIntensity - 1.0f) / downsampleAmount);
		blurMaterial.color = new Color(1F, 1F, 1F, 0.25f + extraBlurBoost);

		bool oddEven = true;
		for (int i = 0; i < blurIterations; i++)
		{
			if (oddEven)
				FourTapCone(buffer, buffer2, i);
			else
				FourTapCone(buffer2, buffer, i);
			oddEven = !oddEven;
		}
		Graphics.Blit(source, destination);
		//ImageEffects.Blit(source,destination);

		if (oddEven)
			BlitGlow(buffer, destination);
		else
			BlitGlow(buffer2, destination);
        
		RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);

		return destination;
	}
	#region OnRenderImage - OLD
	
	// Called by the camera to apply the image effect
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!gameObject.active)
		{
			return;
		}

		#region Clamp parameters to sane values
		glowThreshold = Mathf.Clamp(glowThreshold, 0.0f, 1.0f);
		glowIntensity = Mathf.Clamp(glowIntensity, 0.0f, 10.0f);
		blurIterations = Mathf.Clamp(blurIterations, 0, 30);
		blurSpread = Mathf.Clamp(blurSpread, 0.0f, 1.0f);
		#endregion

		RenderTexture buffer = RenderTexture.GetTemporary(source.width / downsampleAmount, source.height / downsampleAmount, 0);
		RenderTexture buffer2 = RenderTexture.GetTemporary(source.width / downsampleAmount, source.height / downsampleAmount, 0);

		// Copy glow texture to the 4x4 smaller texture.
		DownSample(glowTexture, buffer);

		// Blur the small texture
		float extraBlurBoost = Mathf.Clamp01((glowIntensity - 1.0f) / downsampleAmount);
		blurMaterial.color = new Color(1F, 1F, 1F, 0.25f + extraBlurBoost);

		bool oddEven = true;
		for (int i = 0; i < blurIterations; i++)
		{
			if (oddEven)
				FourTapCone(buffer, buffer2, i);
			else
				FourTapCone(buffer2, buffer, i);
			oddEven = !oddEven;
		}
		Graphics.Blit(source, destination);
		//ImageEffects.Blit(source,destination);

		if (oddEven)
			BlitGlow(buffer, destination);
		else
			BlitGlow(buffer2, destination);
        
		RenderTexture.ReleaseTemporary(buffer);
		RenderTexture.ReleaseTemporary(buffer2);
	}
	#endregion

	public void BlitGlow(RenderTexture source, RenderTexture dest)
	{
		compositeMaterial.color = new Color(1F, 1F, 1F, Mathf.Clamp01(glowIntensity));
		Graphics.Blit(source, dest, compositeMaterial);
		//ImageEffects.BlitWithMaterial( compositeMaterial, source, dest );
    }

    #region OLD
    //private static void Render4TapQuad(RenderTexture dest, float offsetX, float offsetY)
    //{
    //    GL.Begin(GL.QUADS);

    //    // Direct3D needs interesting texel offsets!		
    //    Vector2 off = Vector2.zero;
    //    if (dest != null)
    //        off = dest.GetTexelOffset() * 0.75f;

    //    Set4TexCoords(off.x, off.y, offsetX, offsetY);
    //    GL.Vertex3(0, 0, .1f);

    //    Set4TexCoords(1.0f + off.x, off.y, offsetX, offsetY);
    //    GL.Vertex3(1, 0, .1f);

    //    Set4TexCoords(1.0f + off.x, 1.0f + off.y, offsetX, offsetY);
    //    GL.Vertex3(1, 1, .1f);

    //    Set4TexCoords(off.x, 1.0f + off.y, offsetX, offsetY);
    //    GL.Vertex3(0, 1, .1f);

    //    GL.End();
    //}

    //private static void Set4TexCoords(float x, float y, float offsetX, float offsetY)
    //{
    //    GL.MultiTexCoord2(0, x - offsetX, y - offsetY);
    //    GL.MultiTexCoord2(1, x + offsetX, y - offsetY);
    //    GL.MultiTexCoord2(2, x + offsetX, y + offsetY);
    //    GL.MultiTexCoord2(3, x - offsetX, y + offsetY);
    //}
    #endregion


}
