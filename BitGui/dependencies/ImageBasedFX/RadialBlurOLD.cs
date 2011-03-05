using UnityEngine;

[ExecuteInEditMode]
public class RadialBlurOLD : MonoBehaviour
{
	public Shader rbShader;

	public float blurStrength = 2.2f;
	public float blurWidth = 1.0f;

	private Material rbMaterial = null;
	private bool isOpenGL;

	private Material GetMaterial()
	{
		if (rbMaterial == null)
		{
			rbMaterial = new Material(rbShader);
			rbMaterial.hideFlags = HideFlags.HideAndDontSave;
		}
		return rbMaterial;
	}

	void Start()
	{
		rbShader = (Shader)Resources.Load("RadialBlur");

		if (rbShader == null || !rbShader.isSupported)
		{
			Debug.LogError("RadialBlur shader missing or not supported!", this);
			enabled = false;
			return;
		}
		isOpenGL = SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL");

		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects )
		{
			enabled = false;
			return;
		}
	}


	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		//If we run in OpenGL mode, our UV coords are
		//not in 0-1 range, because of the texRECT sampler
		float ImageWidth = 1;
		float ImageHeight = 1;
		if (isOpenGL)
		{
			ImageWidth = source.width;
			ImageHeight = source.height;
		}

		GetMaterial().SetFloat("_BlurStrength", blurStrength);
		GetMaterial().SetFloat("_BlurWidth", blurWidth);
		GetMaterial().SetFloat("_iHeight", ImageWidth);
		GetMaterial().SetFloat("_iWidth", ImageHeight);
		//ImageEffects.BlitWithMaterial(GetMaterial(), source, dest);
		Graphics.Blit(source, dest, GetMaterial());
	}
}