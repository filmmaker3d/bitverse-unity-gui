using Bitverse.Unity.Gui;
using UnityEngine;


public class BitWindow : BitContainer
{
	#region Accessibility

	private int _windowID = ++WindowsCount;

	public int WindowID
	{
		get { return _windowID; }
	}

	#endregion


	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.window; }
	}

	#endregion


	#region Behaviour

	private static int WindowsCount;

	[SerializeField]
	private bool _draggable = true;

	[SerializeField]
	private FormModes _formMode = FormModes.Modeless;

	//private string _lastTooltip = "";

	private Rect _viewPosition;

	[SerializeField]
	private WindowModes _windowMode = WindowModes.Window;

	public FormModes FormMode
	{
		get { return _formMode; }
		set { _formMode = value; }
	}

	public WindowModes WindowMode
	{
		get { return _windowMode; }
		set { _windowMode = value; }
	}

	public bool Draggable
	{
		get { return _draggable; }
		set { _draggable = value; }
	}

	public Size ViewSize
	{
		get
		{
			if (IsInvalidated)
				LayoutItself();

			return new Size(_viewPosition.width, _viewPosition.height);
		}
	}

	#endregion


	#region Data

	//private readonly Dictionary<string, BitControl> _controDictionary = new Dictionary<string, BitControl>();

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
		GUI.color = Color;
		if (WindowMode == WindowModes.Window)
		{
			Position = GUI.Window(_windowID, Position, DoWindow, Content, Style ?? DefaultStyle);

			if (!Disabled)
			{
				if (FormMode == FormModes.Modal || Focus)
				{
					GUI.BringWindowToFront(WindowID);
					GUI.FocusWindow(WindowID);
				}
			}
		}
		else
		{
			DoWindowLess();
		}
	}


	private void DoWindow(int w)
	{
		//DrawTiles(Style ?? Skin.window, true);

		GUIClip.Push(_viewPosition);

		DrawChildren();

		GUIClip.Pop();

		if (Disabled)
		{
			GUI.BringWindowToBack(WindowID);
		}
		else
		{
			if (_draggable)
			{
				GUI.DragWindow();
			}

			//    if (Event.current._type == EventType.repaint && GUI.tooltip != _lastTooltip)
			//    {
			//        if (_lastTooltip != "" && _controDictionary.ContainsKey(_lastTooltip))
			//        {
			//            _controDictionary[_lastTooltip].RaiseMouseExitEvent();
			//        }

			//        if (GUI.tooltip != "" && _controDictionary.ContainsKey(GUI.tooltip))
			//        {
			//            _controDictionary[GUI.tooltip].RaiseMouseOverEvent();
			//        }

			//        _lastTooltip = GUI.tooltip;
			//    }
		}
	}

	private void DoWindowLess()
	{
		GUIStyle s = Style ?? DefaultStyle;

		if (s != null)
		{
			GUI.BeginGroup(Position, Content, s);
		}
		else
		{
			GUI.BeginGroup(Position, Content);
		}

		DrawChildren();

		GUI.EndGroup();

		//if (!Disabled)
		//{
		//    if (Event.current.type == EventType.repaint && GUI.tooltip != _lastTooltip)
		//    {
		//        if (_lastTooltip != "" && _controDictionary.ContainsKey(_lastTooltip))
		//        {
		//            _controDictionary[_lastTooltip].RaiseMouseExit();
		//        }

		//        if (GUI.tooltip != "" && _controDictionary.ContainsKey(GUI.tooltip))
		//        {
		//            _controDictionary[GUI.tooltip].RaiseMouseOver();
		//        }

		//        _lastTooltip = GUI.tooltip;
		//    }
		//}
	}

	#endregion


	#region Layout

	protected override void LayoutItself()
	{
		base.LayoutItself();
		CalculateViewRect();
	}

	internal override void LayoutChildren()
	{
		base.LayoutChildren();
		CalculateViewRect();
	}

	private void CalculateViewRect()
	{
		GUIStyle s = Style ?? DefaultStyle;

		_viewPosition = new Rect
			(
			s.padding.left,
			s.padding.top,
			Size.Width - s.padding.horizontal,
			Size.Height - s.padding.vertical
			);
	}

	#endregion


	#region MonoBehaviour

	public override void Awake()
	{
		base.Awake();
		_windowID = ++WindowsCount;
	}

	public override void OnDrawGizmos()
	{
		Rect abs = AbsolutePosition;
		DrawRect(Color.white, abs);
		GUIStyle s = Style ?? DefaultStyle;
		DrawRect(Color.gray, new Rect(abs.x + s.padding.left, abs.y + s.padding.top, abs.width - s.padding.left - s.padding.right - 2,
									  abs.height - s.padding.top - s.padding.bottom - 2));
	}

	#endregion
}