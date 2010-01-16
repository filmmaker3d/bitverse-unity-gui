using System;
using UnityEngine;
using System.Collections.Generic;
using Bitverse.Unity.Gui;

public class BitWindow : BitContainer
{
	#region Accessibility

    private int _windowId = ++_windowsCount;

    public int WindowId
    {
        get { return _windowId; }
    }
    
	#endregion

	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "window"; }
	}

	#endregion

	#region Behaviour

    [SerializeField]
    private bool _draggable = true;

    [SerializeField]
    private FormModes _formMode = FormModes.Modeless;

    [SerializeField]
    private WindowModes _windowMode = WindowModes.Window;

    private Rect _viewPosition;

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
                Layout();

            return new Size(_viewPosition.width, _viewPosition.height);
        }

    }

    #endregion

    #region Constructors

    public BitWindow()
    {
        _windowId = ++_windowsCount;
        //_controDictionary = new Dictionary<string, BitControl>();
        //AddDictionaryControl(this);
    }

    #endregion

	#region Control

    private string _lastTooltip = "";
	private static int _windowsCount;


    private void DoWindow(int w)
    {

        GUIClip.Push(_viewPosition);

        DrawChildren();

        GUIClip.Pop();

        if (Disabled)
        {
            GUI.BringWindowToBack(WindowId);
        }
        else
        {
            if (_draggable)
            {
                GUI.DragWindow();
            }

            //    if (Event.current.type == EventType.repaint && GUI.tooltip != _lastTooltip)
            //    {
            //        if (_lastTooltip != "" && _controDictionary.ContainsKey(_lastTooltip))
            //        {
            //            _controDictionary[_lastTooltip].RaiseEventMouseOut();
            //        }

            //        if (GUI.tooltip != "" && _controDictionary.ContainsKey(GUI.tooltip))
            //        {
            //            _controDictionary[GUI.tooltip].RaiseEventMouseOver();
            //        }

            //        _lastTooltip = GUI.tooltip;
            //    }
        }
    }

    private void DoWindowLess()
    {

        if (Style != null)
        {
            GUI.BeginGroup(Position, Content, Style);
        }
        else
        {
            GUI.BeginGroup(Position, Content);
        }

        DrawChildren();

        GUI.EndGroup();

        if (!Disabled)
        {
            if (Event.current.type == EventType.repaint && GUI.tooltip != _lastTooltip)
            {
                if (_lastTooltip != "" && _controDictionary.ContainsKey(_lastTooltip))
                {
                    _controDictionary[_lastTooltip].RaiseEventMouseOut();
                }

                if (GUI.tooltip != "" && _controDictionary.ContainsKey(GUI.tooltip))
                {
                    _controDictionary[GUI.tooltip].RaiseEventMouseOver();
                }

                _lastTooltip = GUI.tooltip;
            }
        }
    }

	#endregion

	#region Data

    private readonly Dictionary<string, BitControl> _controDictionary;

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

    public override void DoDraw()
    {
		GUI.color = Color;
        if (WindowMode == WindowModes.Window)
        {
            if (Style != null)
            {
				Position = GUI.Window(_windowId, Position, DoWindow, Content, Style);
            }
            else
            {
				Position = GUI.Window(_windowId, Position, DoWindow, Content);
            }
            if (!Disabled)
            {
                if (FormMode == FormModes.Modal || Focus)
                {
                    GUI.BringWindowToFront(WindowId);
                    GUI.FocusWindow(WindowId);
                }
            }
        }
        else
        {
            DoWindowLess();
        }
    }

	#endregion


	#region Layout
	
    protected override void DoLayout()
    {
        if (!MinSize.IsEmpty && Size < MinSize)
        {
            float height = Size.Height;
            float width = Size.Width;

            if (Size.Height < MinSize.Height)
            {
                height = MinSize.Height;
            }

            if (Size.Width < MinSize.Width)
            {
                width = MinSize.Width;
            }

            Size = new Size(width, height);
        }

        GUISkin s = Skin;

        if (s == null)
            _viewPosition = new Rect(5, 5, Position.width - 10, Position.height - 10);
        else
            _viewPosition = new Rect
                (
                s.window.padding.left,
                s.window.padding.top,
                Size.Width - s.window.padding.horizontal,
                Size.Height - s.window.padding.vertical
                );
    }

    #endregion


    #region TODO

    //private WindowStates _windowState = WindowStates.Normal;
    //private StartPositions _startPosition = StartPositions.Manual;

    [SerializeField]
    private Vector2 _maxSize;
    [SerializeField]
    private Vector2 _minSize;


    public Size MaxSize
    {
        get { return new Size(_maxSize.x, _maxSize.y); }
        set { _maxSize.x = value.Width; _maxSize.y = value.Height; }
    }

    public Size MinSize
    {
        get { return new Size(_minSize.x, _minSize.y); }
        set { _minSize.x = value.Width; _minSize.y = value.Height; }
    }



    #endregion
    
    #region Private Methods

    //internal void AddDictionaryControl(Control source)
    //{
    //    _controDictionary.Add(source.ID.ToString(), source);
    //}

    #endregion

}
