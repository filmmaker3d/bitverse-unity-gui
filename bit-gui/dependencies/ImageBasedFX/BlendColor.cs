using UnityEngine;
using System.Collections;

public class BlendColor : MonoBehaviour
{

    public Color blendColor = Color.white;
    static Material blendMaterial;

    static void CreateBlendMaterial()
    {
        if (!blendMaterial)
        {
            blendMaterial = new Material("Shader \"Quad/Colored Blended\" {" +
            "SubShader { Pass { " +
            "  " +
            " ZWrite On Cull Off Fog { Mode Off } " +
            " ColorMask A" +
            " ZTest Greater " +
            " BindChannels {" +
            " Bind \"vertex\", vertex Bind \"color\", color }" +
            "} } }");
            blendMaterial.hideFlags = HideFlags.HideAndDontSave;
            blendMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    private InvokeUtils.VoidCall onpostrenderCall;
    void OnPostRender() { if (onpostrenderCall == null) onpostrenderCall = SafeOnPostRender; InvokeUtils.SafeCall(this, onpostrenderCall); }
    void SafeOnPostRender()
    {
        GL.LoadOrtho();
        CreateBlendMaterial();
        // set the current material
        blendMaterial.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.Color(blendColor);
        GL.Vertex3(0, 0, -100);
        GL.Vertex3(1, 0, -100);
        GL.Vertex3(1, 1, -100);
        GL.Vertex3(0, 1, -100);
        GL.End();
    }
}
