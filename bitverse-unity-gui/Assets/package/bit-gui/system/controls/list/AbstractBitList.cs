using System;
using System.Collections.Generic;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;


/// <summary>
/// Base class to all list controls.
/// Internal structure is shown bellow:
///  _______________________________________________
/// | <see cref="BitControl.Position"/>             |
/// |   _________________________________________   |
/// |  | <see cref="ScrollRect"/>                |  |
/// |  |   ___________________________________   |  |
/// |  |  | <see cref="ScrollView"/>          |  |  |
/// |__|__|___________________________________|__|__|
/// </summary>
public abstract class AbstractBitList<TModel, TPopulator> : BitContainer, ISelectableControl<object>
    where TModel : IListModel
    where TPopulator : IPopulator
{

    #region MonoBehaviour

    public override void Start()
    {
        base.Start();

        BitControl childRenderer = Renderer;

        if (childRenderer != null)
        {
            childRenderer.MouseUp += OnChildMouseUp;
            //childRenderer.MouseClick += OnChildMouseClick;
        }
    }
    #endregion

    #region Behaviour

    [SerializeField]
    private bool _alwaysShowScroll;

    private Vector2 _scrollPosition;

    public Vector2 ScrollPosition
    {
        get { return _scrollPosition; }
        set { _scrollPosition = value; }
    }

    protected Rect ScrollRect;
    protected Rect ScrollView;

    protected bool ShowScroll;

    protected bool _allowUnselect;
    public bool AllowUnselect
    {
        get { return _allowUnselect; }
        set { _allowUnselect = value; }
    }


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
        get { return _alwaysShowScroll || ShowScroll; }
    }

    #endregion


    #region Data

    protected BitControl _renderer;

    /// <summary>
    /// The renderer that is used as a template to draw all list items.
    /// </summary>
    public BitControl Renderer
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


    protected TModel _model;

    /// <summary>
    /// List data.
    /// </summary>
    public TModel Model
    {
        get { return _model; }
        set
        {
            _model = value;
            _scrollPosition.y = 0;
        }
    }

    protected TPopulator _populator;

    /// <summary>
    /// Populator that populates the <see cref="Renderer"/> using <see cref="Model"/> data.
    /// </summary>
    public TPopulator Populator
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

    private void BeforeDraw(GUIStyle listStyle, GUIStyle scrollStyle, bool showScroll)
    {
        if (showScroll)
        {
            _scrollHorizontalPadding =
                scrollStyle.padding.right +
                Skin.verticalScrollbar.margin.horizontal +
                Skin.verticalScrollbarThumb.fixedWidth;

            ScrollRect = new Rect(
                listStyle.padding.left + scrollStyle.margin.left,
                listStyle.padding.top + scrollStyle.margin.top,
                Position.width - listStyle.padding.horizontal - scrollStyle.margin.horizontal,
                Position.height - listStyle.padding.vertical - scrollStyle.margin.vertical);
        }
        else
        {
            _scrollHorizontalPadding = 0;

            ScrollRect = new Rect(
                listStyle.padding.left + scrollStyle.margin.left,
                listStyle.padding.top + scrollStyle.margin.top,
                Position.width - listStyle.padding.horizontal,
                Position.height - listStyle.padding.vertical);
        }

        ScrollView.width = ScrollRect.width - _scrollHorizontalPadding;
    }

    protected override void DoDraw()
    {
        GUIStyle listStyle = Style ?? DefaultStyle;
        GUIStyle scrollStyle = Skin.scrollView ?? GUIStyle.none;
        bool showScroll = ScrollIsVisible;

        BeforeDraw(listStyle, scrollStyle, showScroll);

        GUI.BeginGroup(Position, Content, listStyle);

        if (showScroll && ScrollView.height < ScrollRect.height)
        {
            ScrollView.height = ScrollRect.height;
        }
        _scrollPosition = GUI.BeginScrollView(ScrollRect, _scrollPosition, ScrollView, false, showScroll);

        BeforeDrawChildren(listStyle, scrollStyle);
        DrawChildren();
        RollScroll();

        GUI.EndScrollView();
        GUI.EndGroup();
        //_mouseClickInfo = null;
        IsActive = false;
        IsHover = false;
        IsOn = false;
    }

    protected virtual void BeforeDrawChildren(GUIStyle listStyle, GUIStyle scrollStyle)
    {
    }

    // returns false if at least one of them is null
    protected bool GetRendererModelAndPopulator(out BitControl listRenderer, out TModel model, out TPopulator populator)
    {
        listRenderer = Renderer;
        model = Model;
        populator = Populator;

        if (listRenderer == null || model == null || populator == null)
        {
            return false;
        }
        return true;
    }

    protected override void DrawChildren()
    {
        if (EditMode)
        {
            DrawInEditMode();
            return;
        }

        BitControl listRenderer;
        TModel model;
        TPopulator populator;

        if (!GetRendererModelAndPopulator(out listRenderer, out model, out populator))
        {
            return;
        }

        string key = name;
        if (MemoryMethodSampler.Enabled)
        {
            key = string.Format("Populator: {0}", key);
            MemoryMethodSampler.Begin(key);
        }
        
        PopulateAndDraw(listRenderer, model, populator);

        if (MemoryMethodSampler.Enabled)
            MemoryMethodSampler.End(key);
    }

    /// <summary>
    /// Populates and draws list items.
    /// </summary>
    /// <param name="listRenderer">A not-null list renderer.</param>
    /// <param name="model">A not-null list model.</param>
    /// <param name="populator">A not-null populator.</param>
    protected abstract void PopulateAndDraw(BitControl listRenderer, TModel model, TPopulator populator);

    private bool _rollDownScroll = false;
    private bool _rollUpScroll = false;
    public void RollDownScroll()
    {
        _rollDownScroll = true;
    }

    public void RollUpScroll()
    {
        _rollUpScroll = true;
    }

    private void RollScroll()
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (_rollDownScroll)
            {
                _rollDownScroll = false;
                _scrollPosition.y = float.MaxValue;
            }
            if (_rollUpScroll)
            {
                _rollUpScroll = false;
                _scrollPosition.y = 0.0f;
            }
        }

    }

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


    public event UnselectEventHandler<object> ItemUnselected;
    protected void RaiseUnselectItem()
    {
        if (ItemUnselected != null)
        {
            ItemUnselected(this);
        }
    }

    /*
    protected override void InternalOnMouseClick(int mouseButton, Vector2 mousePosition)
        {
            base.InternalOnMouseClick(mouseButton,mousePosition);
            if (Position.Contains(mousePosition))
            {
                _mouseClickInfo = new MouseClickInfo(mouseButton, new Vector2(mousePosition.x - Position.x, mousePosition.y - Position.y + _scrollPosition.y), Event.current.control, Event.current.shift);
            }
        }*/


    private void OnChildMouseUp(object sender, MouseEventArgs e)
    {
        if (e.MouseButton == MouseButtons.Left)
        {
            short Modifiers = KeyboardModifiers.None;
            if (Event.current.shift)
            {
                Modifiers |= KeyboardModifiers.Shift;
            }
            if (Event.current.control)
            {
                Modifiers |= KeyboardModifiers.Control;
            }
            AddSelectionItem(BitGuiContext.Current.Data, Modifiers);
        }
    }




    #endregion


    #region Hierarchy

    protected override T InternalAddControl<T>(string controlName)
    {
        if (Renderer != null && Renderer.gameObject != null)
        {
            if (EditMode)
            {
                DestroyImmediate(Renderer.gameObject);
            }
            else
            {
                BitStage.DestroyAsset(Renderer.gameObject);
            }
            _renderer.transform.parent = null;
            _renderer = null;
        }
        BitControl control = base.InternalAddControl<T>(controlName);
        control.Size = new Size(0, 20);
        return (T)control;
    }

    protected override BitControl InternalAddControl(Type controlType, string controlName)
    {
        if (Renderer != null && Renderer.gameObject != null)
        {
            if (EditMode)
            {
                DestroyImmediate(Renderer.gameObject);
            }
            else
            {
                BitStage.DestroyAsset(Renderer.gameObject);
            }
        }
        BitControl control = base.InternalAddControl(controlType, controlName);
        control.Size = new Size(0, 20);
        return control;
    }

    #endregion


    #region Layout

    internal override void LayoutChildren()
    {
    }

    #endregion


    //TODO Selection Manager please!


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
                if (!AllowUnselect)
                {
                    return;
                }

                RemoveSelectionItem(item);
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

    public void SelectItem(object item)
    {
        _lastSelectedItem = item;
        _selectedItems.Add(item);
        RaiseSelectionChanged();
    }

    public void SelectItemWithoutRaise(object item)
    {
        _lastSelectedItem = item;
        _selectedItems.Add(item);
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
        RaiseUnselectItem();
        return _selectedItems.Remove(item);
    }

    public void ClearSelection()
    {
        _selectedItems.Clear();
    }

    public void ClearSelectionWithRaise()
    {
        _selectedItems.Clear();
        RaiseSelectionChanged();
        RaiseUnselectItem();
    }

    public bool IsSelected(object item)
    {
        return _selectedItems.Contains(item);
    }
    /*
	
            private MouseClickInfo _mouseClickInfo;
	
            protected bool CheckSelection(object item, Rect itemPosition)
            {
                if (_mouseClickInfo != null)
                {
                    if (itemPosition.Contains(_mouseClickInfo.MousePosition))
                    {
                        RaiseMouseClick(_mouseClickInfo.Button, _mouseClickInfo.MousePosition);
                        AddSelectionItem(item, _mouseClickInfo.Modifiers);
                        _mouseClickInfo = null;
                    }
                }
	
                return IsSelected(item);
            }*/


    #endregion

    [Obsolete("Use BitGuiContext.Current.Data instead")]
    public abstract object GetObjectDataAt(Vector2 position);

    protected override bool ConsumeEvent(EventType type)
    {
        return false;
    }
}