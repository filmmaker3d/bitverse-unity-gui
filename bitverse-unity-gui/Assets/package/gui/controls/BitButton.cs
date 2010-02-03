using Bitverse.Unity.Gui;
using UnityEngine;


public class BitButton : BitControl
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
		_value = GUI.Button(Position, Content, Style ?? DefaultStyle);
		//switch (Event.current.type)
		//{
		//    case EventType.mouseDown:
		//        _value = true;
		//        break;
		//    case EventType.MouseUp:
		//        _value = false;
		//        break;
		//    case EventType.repaint:
		//        (Style ?? DefaultStyle).Draw(Position, Content, Position.Contains(Event.current.mousePosition), _value, false, Focus);
		//        break;
		//}

	}

	#endregion


	#region Events

	public event MouseClickEventHandler MouseClick;

	private void RaiseMouseClick(int mouseButton)
	{
		if (MouseClick != null)
		{
			MouseClick(this, new MouseClickEventArgs(mouseButton));
		}
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