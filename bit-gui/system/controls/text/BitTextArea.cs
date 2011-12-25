using UnityEngine;


public class BitTextArea : AbstractBitTextField
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.textArea; }
	}

	protected override bool IsMultiline()
	{
		return true;
	}

	#endregion
}