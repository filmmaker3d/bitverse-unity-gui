using Bitverse.Unity.Gui;
using UnityEngine;


public class BitPopup : BitWindow
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.box; }
	}

	#endregion


	#region Behaviour

	private bool _visibility;
	private bool _invisibility;

	public override bool Visible
	{
		set
		{
			base.Visible = value;
			if (value)
			{
				_visibility = true;
				_invisibility = false;
			}
			else
			{
				_visibility = false;
				_invisibility = true;
			}
		}
	}

	[SerializeField]
	private bool _alwaysInsideScreen;

	public bool AlwaysInsideScreen
	{
		get { return _alwaysInsideScreen; }
		set { _alwaysInsideScreen = value; }
	}

	public void Show(Point position, GUISkin skin)
	{
		Skin = skin;
		Show(position);
	}

	public void Show(Point position)
	{
		Rect p = new Rect(position.X, position.Y, Position.width, Position.height);
		if (_alwaysInsideScreen)
		{
			p.x = p.xMax > Screen.width ? Screen.width - p.width : position.X;
			p.y = p.yMax > Screen.height ? Screen.height - p.height : position.Y;
		}
		Location = new Point(p.x, p.y);
		Visible = true;
	}

	public void Hide()
	{
		Visible = false;
	}

	#endregion


	#region Data

	private BitList _options;

	public BitList Options
	{
		get { return _options ?? FindControl<BitList>("options"); }
	}

	private object _selectedItem;


	public object SelectedItem
	{
		get { return _selectedItem; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		base.DoDraw();
		if (_visibility)
		{
			_visibility = false;
			_options.Size = ViewSize;
			GUI.BringWindowToFront(WindowID);
			BitForm.BeforeOnGUI += FocusLost;
		}

		if (_invisibility)
		{
			_invisibility = false;
			//_options.MouseClick -= OptionsMouseClick;
			//_options.SelectionChanged -= OptionsSelectionChanged;
			BitForm.BeforeOnGUI -= FocusLost;
		}
	}

	#endregion


	#region Events

	public event SelectionChangedEventHandler<object> SelectionChanged;

	private void RaiseSelectionChanged(object selectedObject)
	{
		if (SelectionChanged != null)
		{
			SelectionChanged(this, new SelectionChangedEventArgs<object>(selectedObject));
		}
	}

	private void OptionsSelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		if (e.Selection.Length > 0)
		{
			_selectedItem = e.Selection[0];
			RaiseSelectionChanged(_selectedItem);
		}
	}

	private void FocusLost()
	{
		if (Input.GetMouseButtonDown(MouseButtons.Left) || Input.GetMouseButtonDown(MouseButtons.Right) || Input.GetMouseButtonDown(MouseButtons.Middle))
		{
			Vector2 mp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			if (!AbsolutePosition.Contains(mp))
			{
				Visible = false;
			}
		}
	}

	private void OptionsMouseClick(object sender, MouseClickEventArgs e)
	{
		Visible = false;
	}

	#endregion


	#region MonoBehavior

	public override void Awake()
	{
		base.Awake();
		//WindowMode = WindowModes.None;
		if (_options == null)
		{
			_options = InternalFindControl<BitList>("options") ?? (BitList)InternalAddControl(typeof(BitList), "options");
			_options.Location = new Point(0, 0);

			BitControl r = _options.ListRenderer ?? _options.AddControl(typeof(BitLabel), "renderer");
			r.StyleName = "popup_renderer";
			r.Location = new Point(0, 0);
		}
		_options.MouseClick += OptionsMouseClick;
		_options.SelectionChanged += OptionsSelectionChanged;

		Visible = false;
		Draggable = false;
	}

	#endregion
}