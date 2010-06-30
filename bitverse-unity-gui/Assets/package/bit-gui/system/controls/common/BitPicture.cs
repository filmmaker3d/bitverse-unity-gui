using UnityEngine;


public class BitPicture : BitControl
{
	#region Appearance

	[SerializeField]
	protected bool _alphaBlend = true;

	[SerializeField]
    protected float _imageAspect;

	[SerializeField]
    protected ScaleMode _scaleMode = ScaleMode.StretchToFill;

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
        {
            Color color = GUI.color;
            if (!GUI.enabled)
            {
                Color temp = color;
                temp.a = 0.3f;
                GUI.color = temp;
            }
            GUI.DrawTexture(Position, Image, ScaleMode, AlphaBlend, ImageAspect);
            GUI.color = color;
        }
	}

	#endregion
}