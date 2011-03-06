using System;
using UnityEngine;

[RequireComponent (typeof(Camera))]
[AddComponentMenu("")]
public class ImageEffectBase : MonoBehaviour
{
	/// Provides a shader property that is set in the inspector
	/// and a material instantiated from the shader
	public Shader   shader;
	private Material m_Material;
	public bool Initialized = false;

    //private InvokeUtils.VoidCall startCall;
    //protected void Start() { if (startCall == null) startCall = SafeStart; InvokeUtils.SafeCall(this, startCall); }
    //void SafeStart()
	//{
		
	//    // Disable if we don't support image effects
	//    if (!SystemInfo.supportsImageEffects)
	//    {
	//        enabled = false;
	//        return;
	//    }

	//    // Disable the image effect if the shader can't
	//    // run on the users graphics card
	//    if (!shader || !shader.isSupported)
	//        enabled = false;
	//}

	public void Initialize(String shaderName)
	{
		shader = (Shader)Resources.Load(shaderName);

		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects)
		{
		    Debug.LogWarning(shaderName + " not supported!");
			enabled = false;
			return;
		}

		// Disable the image effect if the shader can't
		// run on the users graphics card
		if (!shader || !shader.isSupported)
        {
            Debug.LogWarning(shaderName + " not supported!");
            enabled = false;
		    
		}
			

		Initialized = true;
	}

    protected Material material {
		get {
			if (m_Material == null) {
				m_Material = new Material (shader);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_Material;
		} 
	}

    private InvokeUtils.VoidCall ondisableCall;
    protected void OnDisable() { if (ondisableCall == null) ondisableCall = SafeOnDisable; InvokeUtils.SafeCall(this, ondisableCall); }
    void SafeOnDisable()
    {
		if( m_Material ) {
			DestroyImmediate( m_Material );
		}
	}
}
