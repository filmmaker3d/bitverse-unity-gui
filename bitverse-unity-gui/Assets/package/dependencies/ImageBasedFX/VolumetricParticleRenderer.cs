
using UnityEngine;
using System.Collections;

[@RequireComponent(typeof(Camera))]
public class VolumetricParticleRenderer : MonoBehaviour
{

    private Camera childCamera;
    public Texture maskTexture;

    public bool doPostProcessing = false;

    public Shader volumetricParticleShader;

    public int downSamplerValue = 3;

    private Material dustAddMat = new Material(
        @"Shader ""DustAdd"" {
	        Properties {
		        _Color (""Glow Amount"", Color) = (1,1,1,1)
		        _MainTex ("""", Rect) = ""white"" {}
                _MaskTex ("""", 2D) = ""white"" {}
	        }
	        SubShader {
		        Pass {
                    ColorMask A
			        ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			        SetTexture [_MaskTex] {constantColor [_Color] combine constant * texture}
		        }
		        Pass {
			        ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			        Blend DstAlpha One
			        SetTexture [_MainTex] {constantColor [_Color] combine constant * texture}
		        }
	        }
	        Fallback off
        }");

    private Material VolumetricDustAddMat = new Material(
        @"Shader ""VolumetricAdd"" {
	        Properties {
		        _Color (""Glow Amount"", Color) = (1,1,1,1)
		        _MainTex ("""", Rect) = ""white"" {}
                _MaskTex ("""", 2D) = ""white"" {}
	        }
	        SubShader {
		        Pass {
                    ColorMask A
			        ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			        SetTexture [_MaskTex] {constantColor [_Color] combine constant * texture DOUBLE}
		        }
		        Pass {
			        ZTest Always Cull Off ZWrite Off Fog { Mode Off }
			        Blend DstAlpha SrcAlpha
			        SetTexture [_MainTex] {constantColor [_Color] combine constant * texture DOUBLE}
		        }
	        }
	        Fallback off
        }");

    private RenderTexture targetRenderTexture;

    void Awake()
    {
        //camera.depthTextureMode = DepthTextureMode.Depth;
    }

    // Use this for initialization
    void Start(){

        int volumetricLayer = LayerMask.NameToLayer("Volumetric Particles");
        GameObject childCameraObject = new GameObject();
        childCamera = childCameraObject.AddComponent<Camera>();
        childCamera.depthTextureMode = DepthTextureMode.None;
        childCamera.CopyFrom(camera);
        childCamera.cullingMask = 1 << volumetricLayer;
        childCamera.clearFlags = CameraClearFlags.Color;
        childCamera.backgroundColor = Color.black;
        childCameraObject.transform.parent = transform;
        childCamera.enabled = false;

        camera.cullingMask &= ~(1 << volumetricLayer);

        // HACK TODO Remove this ( visual-assets\Gfx\Dust\Resources\DustMask.dds )
        maskTexture = Resources.Load("DustMask") as Texture;

        dustAddMat.SetTexture("_MaskTex", maskTexture);
        VolumetricDustAddMat.SetTexture("_MaskTex", maskTexture);
    }

    // Update is called once per frame
    void OnPostRender()
    {
        if (doPostProcessing)
            return;
        if (downSamplerValue == 0)
            return;
        RenderTexture targetRenderTexture = RenderTexture.GetTemporary(Screen.width / downSamplerValue, Screen.height / downSamplerValue);
        childCamera.targetTexture = targetRenderTexture;
        childCamera.Render();
        childCamera.targetTexture = null;
        dustAddMat.SetTexture("_MainTex", targetRenderTexture);
        GL.LoadOrtho();
        for (int i = 0; i < dustAddMat.passCount; i++)
        {
            dustAddMat.SetPass(i);
            //GL.LoadPixelMatrix(0,0,1,1);
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 0, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 1, 0);
            GL.End();
        }
        dustAddMat.SetTexture("_MainTex", null);
        RenderTexture.ReleaseTemporary(targetRenderTexture);
        //Graphics.DrawTexture(new Rect(0, 0, 1, 1), targetRenderTexture, dustAddMat);
    }
    /*
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        camera.depthTextureMode = DepthTextureMode.Depth;
        if (!doPostProcessing)
            return;
        if (downSamplerValue <= 0)
            downSamplerValue = 1;
        RenderTexture targetRenderTexture = RenderTexture.GetTemporary(Screen.width/downSamplerValue, Screen.height/downSamplerValue, 0);
        Shader.SetGlobalVector("_CameraDepthTexture_Size", new Vector4(camera.pixelWidth, camera.pixelHeight, 0, 0));
        childCamera.targetTexture = targetRenderTexture;
        childCamera.depthTextureMode = DepthTextureMode.None;
        childCamera.RenderWithShader(volumetricParticleShader, "");
        //childCamera.Render();
        childCamera.targetTexture = null;
        Graphics.Blit(targetRenderTexture, destination, VolumetricDustAddMat);
        RenderTexture.ReleaseTemporary(targetRenderTexture);
    }*/
}
