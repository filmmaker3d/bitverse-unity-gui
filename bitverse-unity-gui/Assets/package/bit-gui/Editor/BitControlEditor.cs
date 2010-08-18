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
        CheckDuplicateGuids();
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

    // Objects in Scene
    private Dictionary<Guid, BitControl> _controlsInSceneEditor = new Dictionary<Guid, BitControl>();
    private float _checkFrequency = 1;
    private double _lastCheckedTime = Time.frameCount;

    private void CheckDuplicateGuids()
    {
        // Small Hack to Check Duplicate Guids
        if (Time.frameCount - _lastCheckedTime > _checkFrequency)
        {
            BitControl[] allControls = (BitControl[])FindObjectsOfType(typeof(BitControl));
            foreach (BitControl curControl in allControls)
            {
                BitControl storedControl;
                if (_controlsInSceneEditor.TryGetValue(curControl.ID, out storedControl))
                {
                    // If different control then change your ID
                    if (storedControl.gameObject.GetInstanceID() != curControl.gameObject.GetInstanceID())
                    {
                        curControl.ID = Guid.NewGuid();
                    }
                }
                else
                {
                    _controlsInSceneEditor.Add(curControl.ID, curControl);
                }
            }
            _lastCheckedTime = Time.frameCount;
        }
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

    private void TextKindCheck(bool defaultIsStatic)
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);

        foreach (Object o in selection)
        {
            if (o.GetType().IsAssignableFrom(typeof(GameObject)))
            {
                GameObject go = (GameObject)o;
                Component[] comps = go.GetComponentsInChildren(typeof(BitControl));
                for (int t = 0; t < comps.Length; t++)
                {
                    BitControl control = (BitControl)comps[t];

                    CheckThisContent(control, defaultIsStatic);
                }
            }
        }
    }

    private void CheckThisContent(BitControl control, bool defaultIsStatic)
    {
        if (control.Text.Equals(string.Empty))
            control.textKind = TextKind.NONE;
        else
            if (control.textKind == TextKind.NONE && defaultIsStatic)
                control.textKind = TextKind.STATIC_TEXT;
            else
                if (control.textKind == TextKind.NONE && !defaultIsStatic)
                    control.textKind = TextKind.DINAMIC_TEXT;

        if (control.textKind != TextKind.STATIC_TEXT && control.textKind != TextKind.NONE)
        {
            if (!control.Content.text.Contains("<"))
            {
                //control.Content.text = "<" + control.Content.text + ">";
            }
        }
        else
        {
            if (control.Content.text.Contains("<"))
            {
                //control.Content.text = control.Content.text.Substring(1, control.Content.text.Length - 2);
            }
        }
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
    private bool drawDefaultInspector;

    private List<TargetInfo> targetList = new List<TargetInfo>();
    private List<GUIStyle> styles = new List<GUIStyle>();
    private List<GUIStyle> copyStyles = new List<GUIStyle>();
    private string report = "";

    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(500, 300);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //DrawDefault();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        DrawButtons();
    }

    private void DrawDefault()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        /*drawDefaultInspector = EditorGUILayout.Foldout(drawDefaultInspector, "Default", EditorStyles.foldout);
        

        if (drawDefaultInspector)*/
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.EndVertical();
    }

    private GUISkin superSkin;

    private void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy all Styles"))
        {
            BitControl window = target as BitControl;

            if (window == null)
                return;

            report = "";

            styles.Clear();
            copyStyles.Clear();
            superSkin = window.Skin;

            foreach (GUIStyle s in window.Skin)
            {
                if (!Contains(window.Skin, s))
                {
                    Debug.Log("name: " + s.name);
                    GUIStyle clone = new GUIStyle(s);
                    report += "\nDefault added: " + s.name;
                    styles.Add(s);
                    copyStyles.Add(clone);
                }
            }

            if (window.Style != null)
            {
                Add(window.name, window.Skin.name, window.Style);
            }
            else
            {
                Debug.Log("window is null");
            }

            GetChildStyles(window, window.Skin);

            Debug.Log("Number of Styles: " + copyStyles.Count + "\n" + report + "\n");

            GUISkinInspector.Font = window.Skin.font;
            GUISkinInspector.PrefabCopy = true;
            GUISkinInspector.CopiedStyleList.Clear();
            GUISkinInspector.CopiedStyleList = copyStyles;
        }
        EditorGUILayout.EndHorizontal();
    }

    private bool Contains(GUISkin skin, GUIStyle s)
    {
        foreach (GUIStyle t in skin.customStyles)
        {
            if (s.Equals(t))
                return true;
        }
        return false;
    }

    private void GetChildStyles(BitControl parent, GUISkin parentSkin)
    {
        for (int i = 0; i < parent.transform.GetChildCount(); i++)
        {
            Component childd = parent.transform.GetChild(i).GetComponent(typeof(BitControl));

            BitControl child = (BitControl)childd;

            GUISkin childSkin = child.Skin;

            if (child.Skin.name.Equals("InspectorGUISkin"))
            {
                childSkin = parentSkin;
            }
            else
            {
                childSkin = child.Skin;
            }

            if (!child.StyleName.Equals(""))
            {
                GUIStyle style = childSkin.GetStyle(child.StyleName);

                if (!style.name.Equals(""))
                {
                    string newName = Add(child.name, childSkin.name, style);
                    if (!newName.Equals(""))
                        child.StyleName = newName;
                }

                //Debug.Log("else -> container: " + child.name + " , Skin Name: " + childSkin.name + " , style: " + style.name);

            }
            child.Skin = null;
            GetChildStyles(child, childSkin);
        }
    }

    private class StyleInfo
    {
        public GUISkin Skin;
        public GUIStyle Style;

        public string key()
        {
            return Skin.name + "-" + Style.name;
        }
    }

    private class TargetInfo
    {
        public GUIStyle CloneStyle;
        public StyleInfo SourceStyle;
    }

    private string Add(string control, string skinName, GUIStyle style)
    {
        string actualName = "";
        int sameName = 0;
        GUIStyle clone = new GUIStyle(style);
        foreach (GUIStyle guiStyle in styles)
        {
            if (guiStyle.Equals(style))
            {
                return "";
            }
            if (guiStyle.name.Equals(style.name + "_" + sameName))
                sameName++;
        }
        if (sameName > 0)
            actualName = clone.name + "_" + sameName;
        else
            actualName = clone.name;

        string reportLine = "\nControl Name: " + control + ", Skin Name: " + skinName + ", Style Name: " + actualName;

        Debug.Log(reportLine);

        report += reportLine;

        clone.name = actualName;

        styles.Add(style);
        copyStyles.Add(clone);

        return actualName;
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