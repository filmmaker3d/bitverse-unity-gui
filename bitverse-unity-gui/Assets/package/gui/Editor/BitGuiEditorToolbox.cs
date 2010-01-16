using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BitGuiEditorToolbox : EditorWindow
{
    [MenuItem("Tools/XGui Toolbox")]
    public static void X()
    {

        EditorWindow window = EditorWindow.GetWindow(typeof(BitGuiEditorToolbox));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        AddComponent(typeof(BitForm), "Form");
        AddComponent(typeof(BitLabel), "Label");
        AddComponent(typeof(BitButton), "Button");
        AddComponent(typeof(BitWindow), "Window");
        AddComponent(typeof(BitBox), "Box");
        AddComponent(typeof(BitToggle), "Toggle");
        AddComponent(typeof(BitTextArea), "TextArea");
        AddComponent(typeof(BitTextField), "TextField");
        AddComponent(typeof(BitVerticalScrollbar), "VerticalScrollbar");
        AddComponent(typeof(BitRepeatButton), "RepeatButton");
        AddComponent(typeof(BitPasswordField), "PasswordField");
        AddComponent(typeof(BitHorizontalScrollbar), "HorizontalScrollbar");
        AddComponent(typeof(BitHorizontalSlider), "HorizontalSlider");
        AddComponent(typeof(BitVerticalSlider), "VerticalSlider");
        AddComponent(typeof(BitDrawTexture), "DrawTexture");
        AddComponent(typeof(BitScrollView), "ScrollView");
        AddComponent(typeof(BitGroup), "Group");
        AddComponent(typeof(BitList), "List");
        BitControlEditor.SnapToGrid = EditorGUILayout.Toggle("Snap to grid", BitControlEditor.SnapToGrid);
        if (BitControlEditor.SnapToGrid)
        {
            BitControlEditor.Grid = EditorGUILayout.Vector2Field("Grid", BitControlEditor.Grid);
        }

    }
    void AddComponent(Type component, string name)
    {
        if (GUILayout.Button(name))
        {
            if (Selection.activeGameObject != null)
            {
                GameObject o = new GameObject();
                BitControl c = (BitControl)o.AddComponent(component);
                o.transform.parent = Selection.activeTransform;
                o.name = name;
                c.Size = new Bitverse.Unity.Gui.Size(80, 20);
            }
        }

    }
}
