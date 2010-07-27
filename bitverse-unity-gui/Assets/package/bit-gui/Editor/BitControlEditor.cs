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


#region Common

[CustomEditor(typeof(BitLabel))]
public class BitLabelEditor : BitControlEditor
{
}


[CustomEditor(typeof(BitButton))]
public class BitButtonEditor : BitControlEditor
{
}


[CustomEditor(typeof(BitToggle))]
public class BitToggleEditor : BitControlEditor
{
}


[CustomEditor(typeof(BitRepeatButton))]
public class BitRepeatButtonEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(110, 20);
    }
}


[CustomEditor(typeof(BitBox))]
public class BitBoxEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}


[CustomEditor(typeof(BitDropDown))]
public class BitDropDownEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(150, 29);
    }
}


[CustomEditor(typeof(BitPicture))]
public class BitPictureEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 100);
    }
}

[CustomEditor(typeof(BitWebImage))]
public class BitWebImageEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 100);
    }
}

[CustomEditor(typeof(BitSprite))]
public class BitSpriteEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 100);
    }
}


[CustomEditor(typeof(BitWindow))]
public class BitWindowEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(500, 300);
    }
}

#endregion


#region Group

[CustomEditor(typeof(BitGroup))]
public class BitGroupEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}

#endregion


#region List

[CustomEditor(typeof(BitList))]
public class BitListEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(150, 200);
    }
}


[CustomEditor(typeof(BitGridList))]
public class BitGridListEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(260, 160);
    }
}

#endregion


#region Popup

[CustomEditor(typeof(BitPopup))]
public class BitPopupEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(120, 160);
    }
}


[CustomEditor(typeof(BitContextMenu))]
public class BitContextMenuEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(120, 160);
    }
}

[CustomEditor(typeof(BitContextMenuItem))]
public class BitContextMenuItemEditor : BitControlEditor
{
}

[CustomEditor(typeof(BitContextMenuOptions))]
public class BitContextMenuOptionsEditor : BitControlEditor
{
}

#endregion


#region Progress

[CustomEditor(typeof(BitHorizontalProgressBar))]
public class BitHorizontalProgressBarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(160, 20);
    }
}


[CustomEditor(typeof(BitVerticalProgressBar))]
public class BitVerticalProgressBarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(20, 160);
    }
}

#endregion


#region Scroll

[CustomEditor(typeof(BitScrollView))]
public class BitScrollViewEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}


[CustomEditor(typeof(BitHorizontalScrollbar))]
public class BitHorizontalScrollbarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        BitControl p = control.Parent;
        if (p != null)
        {
            Rect parentPosition = p.Position;
            control.Location = new Point(0, parentPosition.height - 20);
            control.Size = new Size(parentPosition.width, 20);
            control.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
        }
        else
        {
            control.Size = new Size(200, 20);
        }
    }
}


[CustomEditor(typeof(BitVerticalScrollbar))]
public class BitVerticalScrollbarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        BitControl p = control.Parent;
        if (p != null)
        {
            Rect parentPosition = p.Position;
            control.Location = new Point(parentPosition.width - 20, 0);
            control.Size = new Size(20, parentPosition.height);
            control.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        }
        else
        {
            control.Size = new Size(20, 100);
        }
    }
}

#endregion


#region Slider

[CustomEditor(typeof(BitHorizontalSlider))]
public class BitHorizontalSliderEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 10);
    }
}


[CustomEditor(typeof(BitVerticalSlider))]
public class BitVerticalSliderEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(10, 100);
    }
}

#endregion


#region Stage

[CustomEditor(typeof(BitEditorStage))]
public class BitEditorStageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BitEditorStage stage = ((BitEditorStage)target);
        stage.Background = (Texture)EditorGUILayout.ObjectField("Background", stage.Background, typeof(Texture2D));
        stage.BackgroundScaleMode = (ScaleMode)EditorGUILayout.EnumPopup(stage.BackgroundScaleMode);
    }
}

#endregion


#region Tab

[CustomEditor(typeof(BitTabbedPane))]
public class BitTabbedPaneEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(230, 140);
    }
}


[CustomEditor(typeof(BitTab))]
public class BitTabEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(230, 140);
    }
}

#endregion


#region Text

[CustomEditor(typeof(BitTextField))]
public class BitTextFieldEditor : BitControlEditor
{
}


[CustomEditor(typeof(BitTextArea))]
public class BitTextAreaEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}


[CustomEditor(typeof(BitPasswordField))]
public class BitPasswordFieldEditor : BitControlEditor
{
}


[CustomEditor(typeof(BitFilteredTextField))]
public class BitFilteredTextFieldEditor : BitControlEditor
{
}

[CustomEditor(typeof(BitRichText))]
public class BitRichTextEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}

#endregion


#region ModelViewer

[CustomEditor(typeof(BitModelViewer))]
public class BitModelViewerEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}

#endregion