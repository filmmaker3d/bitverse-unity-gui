using UnityEngine;


public class BitLabel : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.label; }
	}

	#endregion

	#region Draw

	protected override void DoDraw()
	{
		GUI.Label(Position, Content, Style ?? DefaultStyle);
	}

	#endregion
}