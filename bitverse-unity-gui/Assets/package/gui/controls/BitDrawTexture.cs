using UnityEngine;


public class BitDrawTexture : BitControl
{
	#region Appearance

	[SerializeField]
	private bool _alphaBlend = true;

	[SerializeField]
	private float _imageAspect;

	[SerializeField]
	private ScaleMode _scaleMode = ScaleMode.StretchToFill;

	public Texture Image
	{
		get { return Content.image; }
		set { Content.image = value; }
	}

	public ScaleMode ScaleMode
	{
		get { return _scaleMode; }
		set { _scaleMode = value; }
	}

	public bool AlphaBlend
	{
		get { return _alphaBlend; }
		set { _alphaBlend = value; }
	}

	public float ImageAspect
	{
		get { return _imageAspect; }
		set { _imageAspect = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		if (Image != null)
			GUI.DrawTexture(Position, Image, ScaleMode, AlphaBlend, ImageAspect);
	}

	#endregion
}