using UnityEngine;


public class BitBox : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.box; }
	}

	#endregion

	#region Draw

	protected override void DoDraw()
    {
        (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn, false);
		//GUI.Box(Position, Content, Style ?? DefaultStyle);
	}

	#endregion


}