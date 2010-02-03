using UnityEngine;


public class BitGroup : BitContainer
{
	#region Data

	public string Text
	{
		get { return Content.text; }
		set { Content.text = value; }
	}

	public Texture Image
	{
		get { return Content.image; }
		set { Content.image = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		GUI.BeginGroup(Position, Content, Style ?? DefaultStyle);

		DrawChildren();

		GUI.EndGroup();
	}

	#endregion
}