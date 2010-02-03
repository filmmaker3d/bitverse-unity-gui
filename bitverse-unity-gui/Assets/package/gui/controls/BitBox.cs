using UnityEngine;


public class BitBox : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.box; }
	}

	#endregion


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
		GUI.Box(Position, Content, Style ?? DefaultStyle);
	}

	#endregion
}