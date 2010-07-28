using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


internal delegate void ExecuteNextFrame();


public partial class BitControlEditor : Editor
{
    private readonly Dictionary<Type, ModeHandler> _modeHandlers = new Dictionary<Type, ModeHandler>();

    private Type _mode;

    internal bool IsMouseUp;
    internal bool IsMouseDown;
    internal bool IsDrag;

    private bool _testDrag;
    internal Object[] ComponentList;

    internal static ExecuteNextFrame ExecuteNextFrame;

    private Vector2 _startDragPosition;

    public Vector2 StartDragPosition
    {
        get { return _startDragPosition; }
    }

    public void Awake()
    {
        if (_modeHandlers.Count == 0)
        {
            _modeHandlers.Add(typeof(SelectHandler), new SelectHandler(this));
            _modeHandlers.Add(typeof(MoveHandler), new MoveHandler(this));
            _modeHandlers.Add(typeof(ResizeHandler), new ResizeHandler(this));
        }
        Mode = typeof(SelectHandler);
    }

    internal Type Mode
    {
        get { return _mode; }
        set
        {
            if (_mode != null)
                _modeHandlers[_mode].OnDisable();
            _mode = value;
            _modeHandlers[_mode].OnEnable();
        }
    }

    public T Handler<T>() where T : ModeHandler
    {
        return (T)_modeHandlers[typeof(T)];
    }

    public void OnSceneGUI()
    {
        UpdateComponentList();
        UpdateMouseState();
        ExecuteDelayedOperations();
        ExecuteHandler();
        ProcessShortcuts();
        SetCurrentTarget();
    }

    private void SetCurrentTarget()
    {
        if (CurrentTarget == null)
        {
            CurrentTarget = target;
        }
        else if (CurrentTarget != target)
        {
            CurrentTarget = target;
            if (_addingControl)
            {
                _addingControl = false;
                InternalOnAddControl();
            }
        }
    }

    private static Object CurrentTarget;

    private void ProcessShortcuts()
    {
        if ((_mode == null) || (!_modeHandlers.ContainsKey(_mode)))
        {
            return;
        }
        foreach (Shortcut shortcut in _modeHandlers[_mode].Shortcuts)
        {
            if (shortcut._type == Event.current.type)
                if (shortcut._code == Event.current.keyCode)
                    shortcut._callback(shortcut);
        }
    }

    private void ExecuteHandler()
    {
        if ((_mode != null) && (_modeHandlers.ContainsKey(_mode)))
        {
            _modeHandlers[_mode].Execute();
        }
    }

    private static void ExecuteDelayedOperations()
    {
        if (ExecuteNextFrame == null)
        {
            return;
        }
        ExecuteNextFrame();
        ExecuteNextFrame = null;
    }

    private void UpdateComponentList()
    {
        BitControl[] objects = (BitControl[])FindObjectsOfType(typeof(BitControl));
        List<Object> r = new List<Object>();
        foreach (BitControl o in objects)
        {
            if (!o.Unselectable)
            {
                r.Add(o);
            }
        }
        ComponentList = r.ToArray();
    }

    private void UpdateMouseState()
    {
        GuiEditorUtils.UpdateMousePosition();
        if (!Event.current.isMouse)
        {
            return;
        }
        IsMouseDown = Event.current.type == EventType.MouseDown;
        IsMouseUp = Event.current.type == EventType.MouseUp;
        IsDrag = Event.current.type == EventType.MouseDrag;
        if ((!_testDrag) && (IsDrag))
        {
            _startDragPosition = GuiEditorUtils.MousePosition;
            _testDrag = true;
        }
        if (IsMouseUp)
            _testDrag = false;
    }

    private static bool _addingControl;
    private static BitControl _controlAdded;

    internal static void AddControl(BitControl control)
    {
        if (control == null)
        {
            return;
        }
        _addingControl = true;
        control.Size = new Size(80, 20);
        _controlAdded = control;
    }

    private void InternalOnAddControl()
    {
        if (_controlAdded == null)
        {
            return;
        }
        _controlAdded.Awake();
        OnAddControl(_controlAdded);
        _controlAdded = null;
    }

    protected virtual void OnAddControl(BitControl control)
    {
    }
}
