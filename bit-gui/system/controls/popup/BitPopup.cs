using Bitverse.Unity.Gui;


public class BitPopup : AbstractBitPopup
{
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

	private void OptionsMouseClick(object sender, MouseEventArgs e)
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
			_options = InternalFindControl<BitList>("options");
			if (_options == null)
			{
				_options = InternalAddControl<BitList>("options");
				_options.Location = new Point(0, 0);

				BitControl r = _options.Renderer;
				if (r == null)
				{
					r = _options.AddControl<BitLabel>("renderer");
					r.StyleName = "popup-renderer";
					r.Location = new Point(0, 0);
				}
			}
		}
		_options.MouseClick += OptionsMouseClick;
		_options.SelectionChanged += OptionsSelectionChanged;

		Visible = false;
		Draggable = false;
	}

	#endregion
}