using UnityEngine;

// This class implements simple ghosting type Motion Blur.
// If Extra Blur is selected, the scene will allways be a little blurred,
// as it is scaled to a smaller resolution.
// The effect works by accumulating the previous frames in an accumulation
// texture.
[AddComponentMenu("Image Effects/Motion Blur")]
public class MotionBlur : ImageEffectBase
{
	public float blurAmount = 0.8f;
	public bool extraBlur = false;

	public void Start()
	{
		Debug.LogWarning("LOG Constructor!");
		shader = (Shader)Resources.Load("MotionBlur");//Shader.Find("MotionBlur");
		//shader = Shader.Find("MotionBlur");
		if (!shader)
		{
			Debug.LogError("Shader is null!");
		}
		Debug.Log("Shader name: " + shader.name);
		InvokeUtils.SafeCall(this, SafeStart);
	}

	void SafeStart()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}

		// Disable the image effect if the shader can't
		// run on the users graphics card
		if (!shader || !shader.isSupported)
			enabled = false;
	}

	private RenderTexture accumTexture;

	protected new void OnDisable()
	{
		base.OnDisable();
		DestroyImmediate(accumTexture);
	}

	// Called by camera to apply image effect
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		// Create the accumulation texture
		if (accumTexture == null || accumTexture.width != source.width || accumTexture.height != source.height)
		{
			DestroyImmediate(accumTexture);
			Debug.LogWarning("Shader NAME: " + material.shader.name);
			accumTexture = new RenderTexture(source.width, source.height, 0);
			accumTexture.hideFlags = HideFlags.HideAndDontSave;
			ImageEffectsArt.Blit(source, accumTexture);
		}

		// If Extra Blur is selected, downscale the texture to 4x4 smaller resolution.
		if (extraBlur)
		{
			RenderTexture blurbuffer = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
            ImageEffectsArt.Blit(accumTexture, blurbuffer);
            ImageEffectsArt.Blit(blurbuffer, accumTexture);
			RenderTexture.ReleaseTemporary(blurbuffer);
		}

		// Clamp the motion blur variable, so it can never leave permanent trails in the image
		blurAmount = Mathf.Clamp(blurAmount, 0.0f, 0.92f);

		// Setup the texture and floating point values in the shader
		material.SetTexture("_MainTex", accumTexture);
		material.SetFloat("_AccumOrig", 1.0F - blurAmount);

		// Render the image using the motion blur shader
		Graphics.Blit(source, accumTexture, material);
		Graphics.Blit(accumTexture, destination);
	}
}