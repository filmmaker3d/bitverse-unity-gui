using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEngine;


/// <summary>
/// Base class to all list controls.
/// Internal structure is shown bellow:
///  _______________________________________________
/// | <see cref="BitControl.Position"/>             |
/// |   _________________________________________   |
/// |  | <see cref="_scrollRect"/>               |  |
/// |  |   ___________________________________   |  |
/// |  |  | <see cref="_scrollView"/>         |  |  |
/// |__|__|___________________________________|__|__|
/// </summary>
public abstract class AbstractBitList : BitContainer, ISelectableControl<object>
{
	#region Behaviour

	[SerializeField]
	private bool _alwaysShowScroll;

	private bool _notInEditMode;

	protected Vector2 _scrollPosition;

	protected Rect _scrollRect;
	protected Rect _scrollView;

	protected bool _showScroll;

	/// <summary>
	/// Gets and sets whether the scroll is always visible (only vertical).
	/// </summary>
	public bool ScrollAlwaysVisible
	{
		get { return _alwaysShowScroll; }
		set { _alwaysShowScroll = value; }
	}

	/// <summary>
	/// [Read-only] Whether the scroll is current visible or not (only vertical).
	/// </summary>
	public bool ScrollIsVisible
	{
		get { return _alwaysShowScroll || _showScroll; }
	}

	#endregion


	#region Data

	private BitControl _renderer;

	/// <summary>
	/// The renderer that is used as a template to draw all list items.
	/// </summary>
	public BitControl ListRenderer
	{
		get
		{
			if (_renderer == null)
			{
				_renderer = FindControl<BitControl>();
			}
			return _renderer;
		}
	}


	private IBitListModel _model;

	/// <summary>
	/// List data.
	/// </summary>
	public IBitListModel Model
	{
		get { return _model; }
		set
		{
			_model = value;
			_scrollPosition.y = 0;
		}
	}

	private IBitListPopulator _populator;

	/// <summary>
	/// Populator that populates the <see cref="ListRenderer"/> using <see cref="Model"/> data.
	/// </summary>
	public IBitListPopulator Populator
	{
		get { return _populator; }
		set
		{
			_populator = value;
			_scrollPosition.y = 0;
		}
	}

	#endregion


	#region Draw

	private float _scrollHorizontalPadding;

	private void BeforeDraw(GUIStyle listStyle, bool showScroll)
	{
		if (Event.current.type == EventType.mouseUp)
		{
			if (Position.Contains(Event.current.mousePosition))
			{
				Event e = Event.current;
				Vector2 mp = e.mousePosition;
				_mouseClickInfo = new MouseClickInfo(e.button, new Vector2(mp.x - Position.x, mp.y - Position.y + _scrollPosition.y), e.control, e.shift);
			}
		}

		GUIStyle scrollStyle = Skin.scrollView ?? new GUIStyle();

		if (showScroll)
		{
			_scrollHorizontalPadding =
				scrollStyle.padding.right +
				Skin.verticalScrollbar.margin.horizontal +
				Skin.verticalScrollbarThumb.fixedWidth;

			_scrollRect = new Rect(
				listStyle.padding.left + scrollStyle.margin.left,
				listStyle.padding.top + scrollStyle.margin.top,
				Position.width - listStyle.padding.horizontal - scrollStyle.margin.horizontal,
				Position.height - listStyle.padding.vertical - scrollStyle.margin.vertical);
		}
		else
		{
			_scrollHorizontalPadding = 0;

			_scrollRect = new Rect(
				listStyle.padding.left + scrollStyle.margin.left,
				listStyle.padding.top + scrollStyle.margin.top,
				Position.width - listStyle.padding.horizontal,
				Position.height - listStyle.padding.vertical);
		}


		_scrollView.width = _scrollRect.width - _scrollHorizontalPadding;
	}

	protected override void DoDraw()
	{
		GUIStyle listStyle = Style ?? DefaultStyle;
		bool showScroll = ScrollIsVisible;

		BeforeDraw(listStyle, showScroll);
		GUI.BeginGroup(Position, Content, listStyle);

		_scrollPosition = GUI.BeginScrollView(_scrollRect, _scrollPosition, _scrollView, false, showScroll);

		DrawChildren();

		GUI.EndScrollView();
		GUI.EndGroup();
		_mouseClickInfo = null;
	}

	protected override void DrawChildren()
	{
		if (!_notInEditMode)
		{
			DrawInEditMode();
			return;
		}

		BitControl listRenderer = ListRenderer;

		if (listRenderer == null)
		{
			return;
		}

		IBitListModel model = Model;
		if (model == null)
		{
			return;
		}

		IBitListPopulator populator = Populator;
		if (populator == null)
		{
			return;
		}

		PopulateAndDraw(listRenderer, model, populator);
	}

	/// <summary>
	/// Populates and draws list items.
	/// </summary>
	/// <param name="listRenderer">A not-null list renderer.</param>
	/// <param name="model">A not-null list model.</param>
	/// <param name="populator">A not-null populator.</param>
	protected abstract void PopulateAndDraw(BitControl listRenderer, IBitListModel model, IBitListPopulator populator);

	#endregion


	#region Editor

	/// <summary>
	/// Method called to draw gizmos in editor mode.
	/// </summary>
	protected abstract void DrawInEditMode();

	#endregion


	#region Events

	public event SelectionChangedEventHandler<object> SelectionChanged;

	protected void RaiseSelectionChanged()
	{
		if (SelectionChanged != null)
		{
			SelectionChanged(this, new SelectionChangedEventArgs<object>(_selectedItems.ToArray()));
		}
	}

	public event MouseClickEventHandler MouseClick;

	protected void RaiseMouseClick(int button)
	{
		if (MouseClick != null)
		{
			MouseClick(this, new MouseClickEventArgs(button));
		}
	}

	#endregion


	#region Hierarchy

	public override BitControl AddControl(Type controlControlType, string controlName)
	{
		if (ListRenderer != null && ListRenderer.gameObject != null)
		{
			if (!_notInEditMode)
			{
				DestroyImmediate(ListRenderer.gameObject);
			}
			else
			{
				Destroy(ListRenderer.gameObject);
			}
		}
		BitControl control = base.AddControl(controlControlType, controlName);
		control.Size = new Size(0, 20);
		return control;
	}

	#endregion


	#region Layout

	internal override void LayoutChildren()
	{
	}

	#endregion


	#region MonoBehaviour

	//TODO Eliminate this please!
	public void Start()
	{
		_notInEditMode = true;
	}

	//TODO Eliminate this please!
	public void Stop()
	{
		_notInEditMode = false;
	}

	#endregion


	#region Selection

	private readonly List<object> _selectedItems = new List<object>();

	public object[] SelectedItems
	{
		get { return _selectedItems.ToArray(); }
	}


	[SerializeField]
	private bool _multiSelection;

	public bool MultiSelection
	{
		get { return _multiSelection; }
		set
		{
			_multiSelection = value;
			if (!value && _selectedItems.Count > 0)
			{
				ClearSelection();
				AddSelectionItem(_lastSelectedItem, KeyboardModifiers.None);
			}
		}
	}

	//TODO Implement keep selection
	private bool _keepSelection;

	public bool KeepSelection
	{
		get { return _keepSelection; }
		set { _keepSelection = value; }
	}

	public int IndexOf(object item)
	{
		if (item != null)
		{
			return _selectedItems.IndexOf(item);
		}
		return -1;
	}

	public void AddSelectionItem(object item, short modifiers)
	{
		if (item == null)
		{
			return;
		}

		if (!MultiSelection)
		{
			if (IsSelected(item))
			{
				return;
			}
			ClearSelection();
			SelectItem(item);
			return;
		}

		if (modifiers == KeyboardModifiers.None)
		{
			ClearSelection();
			SelectItem(item);
			return;
		}


		bool shift = (modifiers & KeyboardModifiers.Shift) == KeyboardModifiers.Shift;
		bool control = (modifiers & KeyboardModifiers.Control) == KeyboardModifiers.Control;
		if (shift)
		{
			if (!control)
			{
				ClearSelection();
			}
			SelectRange(Model.IndexOf(_lastSelectedItem), Model.IndexOf(item));
			return;
		}

		if (control)
		{
			if (IsSelected(item))
			{
				RemoveSelectionItem(item);
			}
			else
			{
				SelectItem(item);
			}
		}
	}

	private object _lastSelectedItem;

	private void SelectItem(object item)
	{
		_lastSelectedItem = item;
		_selectedItems.Add(item);
		RaiseSelectionChanged();
	}

	public void SelectRange(int first, int last)
	{
		if (first < 0)
		{
			if (last < 0)
			{
				return;
			}
			SelectItem(_model[last]);
			return;
		}

		if (last < 0)
		{
			if (first < 0)
			{
				return;
			}
			SelectItem(_model[first]);
			return;
		}

		if (first == last)
		{
			SelectItem(_model[first]);
			return;
		}

		if (first > last)
		{
			int aux = first;
			first = last;
			last = aux;
		}
		for (int i = first; i < last; i++)
		{
			_selectedItems.Add(_model[i]);
		}
		SelectItem(_model[last]);
	}

	public bool RemoveSelectionItem(object item)
	{
		return _selectedItems.Remove(item);
	}

	public void ClearSelection()
	{
		_selectedItems.Clear();
	}

	public bool IsSelected(object item)
	{
		return _selectedItems.Contains(item);
	}

	private MouseClickInfo _mouseClickInfo;

	protected bool CheckSelection(object item, Rect itemPosition)
	{
		if (_mouseClickInfo != null)
		{
			if (itemPosition.Contains(_mouseClickInfo.MousePosition))
			{
				RaiseMouseClick(_mouseClickInfo.Button);
				AddSelectionItem(item, _mouseClickInfo.Modifiers);
				_mouseClickInfo = null;
			}
		}

		return IsSelected(item);
	}

	#endregion
}