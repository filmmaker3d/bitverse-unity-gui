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
    //internal bool IsDrag;

    //private bool _testDrag;
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
        //Handles.BeginGUI();
        UpdateComponentList();
        UpdateMouseState();
        ExecuteDelayedOperations();
        ExecuteHandler();
        
        // Move and resize body
        var selectionObjects = Selection.objects;
        if (selectionObjects!=null)
            foreach(Object o in selectionObjects)
            {
                var g = o as GameObject;
                if(g==null) continue;
                //Debug.Log(g);
                var control = g.GetComponent<BitControl>();
                var abs = control.AbsolutePosition;
                capColor = Color.cyan;
                Vector3 pos,npos;
                capSize = HandleUtility.GetHandleSize(new Vector3(abs.x,0,abs.y)) / 15f;

                
                // Resize from bottom-right
                pos = new Vector3(abs.xMax, 0, abs.yMax);
                npos = Handles.FreeMoveHandle(pos, Quaternion.identity, 1, new Vector3(1, 1, 1), DrawCornerCap);
                if (npos != pos)
                {
                    abs.xMax = npos.x;
                    abs.yMax = npos.z;
                    control.AbsolutePosition = abs;
                    break;
                }

                // Resize from right
                pos = new Vector3(abs.xMax, 0, abs.yMin + abs.height/2);
                npos = Handles.FreeMoveHandle(pos, Quaternion.identity, 1, new Vector3(1, 1, 1), DrawCornerCap);
                if (npos.x != pos.x)
                {
                    abs.xMax = npos.x;
                    control.AbsolutePosition = abs;
                    break;
                }


                // Resize from bottom
                pos = new Vector3(abs.xMin + abs.width / 2, 0, abs.yMax);
                npos = Handles.FreeMoveHandle(pos, Quaternion.identity, 1, new Vector3(1, 1, 1), DrawCornerCap);
                if (npos.z != pos.z)
                {
                    abs.yMax = npos.z;
                    control.AbsolutePosition = abs;
                    break;
                }


                // Move from top-left
                capColor = Color.blue;
                pos = new Vector3(abs.xMin, 0, abs.yMin);
                npos = Handles.FreeMoveHandle(pos, Quaternion.identity, 1, new Vector3(1, 1, 1), DrawCornerCap);
                if (npos != pos)
                {
                    abs.x = npos.x;
                    abs.y = npos.z;
                    control.AbsolutePosition = abs;
                    break;
                }
                // Move
                pos = new Vector3(abs.xMin + abs.width / 2, 0, abs.yMin + abs.height / 2);
                capSize = Math.Max(1,Math.Min(abs.width / 2 - 8, abs.height / 2 - 8));
                npos = Handles.FreeMoveHandle(pos, Quaternion.identity, capSize, new Vector3(1, 1, 1), DrawCornerCap);
                if (npos != pos)
                {
                    abs.x = npos.x - abs.width / 2;
                    abs.y = npos.z - abs.height / 2;
                    control.AbsolutePosition = abs;
                    break;
                }
            }
            

        ProcessShortcuts();
        SetCurrentTarget();
        //Handles.EndGUI();
    }


    private static Color capColor;
    private static float capSize;
    private static Vector3[] points = new Vector3[5];

    public static void DrawCornerCap(int controlID, Vector3 position, Quaternion rotation, float size)
    {
        if (Event.current.type == EventType.Repaint)
        {
            var vector = new Vector3(capSize, 0, 0);
            var vector2 = new Vector3(0, 0, capSize);
            points[0] = position + vector + vector2;
            points[1] = position + vector - vector2;
            points[2] = position - vector - vector2;
            points[3] = position - vector + vector2;
            points[4] = position + vector + vector2;

            Color color = Handles.color;
            Handles.color = capColor;
            Handles.DrawPolyLine(points);
            Handles.color = color;
        }
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
        //IsDrag = Event.current.type == EventType.MouseDrag;
        //if ((!_testDrag) && (IsDrag))
        //{
        //    Debug.Log("Start drag");
        //    _startDragPosition = GuiEditorUtils.MousePosition;
        //    _testDrag = true;
        //}
        //if (IsMouseUp)
        //{
        //    if (_testDrag)
        //    {
        //        _testDrag = false;
        //    }
        //}
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
