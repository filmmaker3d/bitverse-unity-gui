using System;
using System.Collections.Generic;
using UnityEngine;
using Object=UnityEngine.Object;


public class PostProcessingFXManager : MonoBehaviour
{
	public List<IPPShaderInterface> EffectsList; // = new List<IPPShaderInterface>();

	//void Awake()
	//{
	//    Debug.Log("Awake!", this);
	//    EffectsList = new List<IPPShaderInterface>();
	//}

	//private IPPShaderInterface foo;


	#region Register Post Processing Shader

	public void RegisterPPShader(IPPShaderInterface shader)
	{
		//foo = shader;
		Debug.Log("Registering PP Shader", this);
		if (shader != null)
		{
			EffectsList.Add(shader);
		}
		else
		{
			Debug.LogError("PPShader is null!!!", this);
		}


		if (EffectsList.Count > 1)
		{
			Debug.Log("Sorting Shaders", this);
			SortShadersByQueue();
		}
	}

	private void SortShadersByQueue()
	{
		Comparison<IPPShaderInterface> comparison = delegate(IPPShaderInterface obj1, IPPShaderInterface obj2)
		                                            	{
		                                            		int dist1 = obj1.QueueOrder;
		                                            		int dist2 = obj2.QueueOrder;
		                                            		return (dist1.CompareTo(dist2));
		                                            	};
		EffectsList.Sort(comparison);
	}

	#endregion


	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//if (foo != null)
		//    foo.ApplyEffect(source, destination);
		//New Source
		RenderTexture buffer = RenderTexture.GetTemporary(source.width, source.height, 0);
		Graphics.Blit(source, buffer);
		//Graphics.Blit(buffer, destination); //-> Works!

		//Iterates Through Sorted PP Shader List
		foreach (IPPShaderInterface shader in EffectsList)
		{
			//The Result Render Texture of a Shader is the source of the next one
			if (shader.Enabled)
			{
				//RenderTexture temp = shader.ApplyEffect(buffer, destination);
				//Graphics.Blit(temp, buffer);
				//To make sure that GUI will be writter on destination
				RenderTexture.active = destination;
				//Graphics.Blit(shader.ApplyEffect(buffer, destination), buffer);
			}
		}
		RenderTexture.ReleaseTemporary(buffer);
	}
}