using UnityEngine;


public interface IPPShaderInterface
{
	int QueueOrder
	{
		get;
		set;
	}

	string Name
	{
		get;
		set;
	}

	bool Enabled
	{
		get;
		set;
	}

	//Obs: Should use "if(!enabled) { return null; }" at the beginning to avoid activating a shader if it's not enabled
	//RenderTexture ApplyEffect(RenderTexture source, RenderTexture destination);
}