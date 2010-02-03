using Bitverse.Unity.Gui;
using UnityEngine;


public class BitRepeatButton : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.button; }
	}

	#endregion


	#region Data

	private bool _value;

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		_value = GUI.RepeatButton(Position, Content, Style ?? DefaultStyle);
	}

	#endregion


	#region Events

	public event MouseClickEventHandler MouseClick;

	private void RaiseMouseClick(int mouseButton)
	{
		if (MouseClick == null)
		{
			return;
		}
		MouseClick(this, new MouseClickEventArgs(mouseButton));
	}

	protected override bool UserEventsAfterDraw()
	{
		if (!_value)
		{
			return false;
		}
		RaiseMouseClick(Event.current.button);
		return true;
	}

	#endregion
}