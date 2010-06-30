using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor (typeof (GUISkin))]
public class GUISkinInspector : Editor
{
    private static GUIStyle copiedStyle;

    private List<GUIStyle> customStyles = new List<GUIStyle>();
    private List<string> customStylesNames = new List<string>();

    private int selectedStyle;
    private bool drawDefaultInspector;

    public void OnEnable()
    {
        RefreshLists();
    }

    private GUISkin Skin
    {
        get { return (GUISkin) target; }
    }

    #region CopyNPaste

    private void CopySelectedStyle()
    {
        copiedStyle = Skin.customStyles[selectedStyle];
        Debug.Log("Copied GUIStyle: " + copiedStyle.name);
    }

    private void PasteSelectedStyle()
    {
        if (copiedStyle == null)
            return;

        GUIStyle clone = new GUIStyle(copiedStyle);
        AddStyle(clone);
        Debug.Log("Pasted GUIStyle: " + clone.name);
    }

    #endregion

    #region ListManagement

    private bool IsNameOnCustomStyles(string name)
    {
        foreach (GUIStyle style in customStyles)
        {
            if (style.name == name)
                return true;
        }
        return false;
    }

    private string CheckRepeatedNames(GUIStyle guiStyle)
    {
        foreach (GUIStyle style in customStyles)
        {
            if (style.name == guiStyle.name && guiStyle != style)
            {
                int count = 1;

                while (IsNameOnCustomStyles(guiStyle.name + "_" + count))
                    count++;

                guiStyle.name = guiStyle.name + "_" + count;

                break;
            }
        }

        return guiStyle.name;
    }

    private void RefreshLists()
    {
        if (Skin.customStyles.Length == 0)
            return;

        GUIStyle selectedStyleObject = Skin.customStyles[selectedStyle];

        customStyles.Clear();
        customStylesNames.Clear();

        foreach (GUIStyle guiStyle in Skin.customStyles)
            AddStyleToLists(guiStyle);

        OrganizeStyles();

        for (int i = 0; i < customStyles.Count; i++)
            if (customStyles[i] == selectedStyleObject)
            {
                selectedStyle = i;
                break;
            }

        EditorUtility.SetDirty(Skin);
    }

    private void OrganizeStyles()
    {
        customStylesNames.Sort();
        customStyles.Sort(delegate(GUIStyle p1, GUIStyle p2) { return p1.name.CompareTo(p2.name); });
        Skin.customStyles = customStyles.ToArray();
    }

    #endregion

    #region AddNDelete

    private void AddNewStyle()
    {
        GUIStyle newStyle = new GUIStyle();
        Debug.Log("1");
        newStyle.name = "New GUIStyle";
        Debug.Log("2");
        AddStyle(newStyle);
        Debug.Log("3");
    }

    private void AddStyle(GUIStyle guiStyle)
    {
        AddStyleToLists(guiStyle);
        Debug.Log("4");
        OrganizeStyles();
        Debug.Log("5");

        for (int i = 0; i < customStyles.Count; i++)
            if (guiStyle == customStyles[i])
            {
                selectedStyle = i;
                break;
            }
        Debug.Log("6");
        EditorUtility.SetDirty(Skin);
    }

    private void AddStyleToLists(GUIStyle guiStyle)
    {
        guiStyle.name = CheckRepeatedNames(guiStyle);
        customStyles.Add(guiStyle);
        customStylesNames.Add(guiStyle.name);
    }

    private void DeleteStyle()
    {
        if (!EditorUtility.DisplayDialog("Delete GUIStyle", "Are you sure you want to delete the GUIStyle : " + Skin.customStyles[selectedStyle].name, "Yes", "No"))
            return;

        customStyles.RemoveAt(selectedStyle);
        customStylesNames.RemoveAt(selectedStyle);
        Skin.customStyles = customStyles.ToArray();
        selectedStyle = 0;
        EditorUtility.SetDirty(Skin);
    }

    #endregion

    #region GUI

    private void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Copy"))
            CopySelectedStyle();
        if (GUILayout.Button("Paste"))
            PasteSelectedStyle();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New"))
            AddNewStyle();
        if (GUILayout.Button("Delete"))
            DeleteStyle();
        EditorGUILayout.EndHorizontal();
    }

    private void CheckEvents()
    {
        if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.C && Event.current.shift)
            CopySelectedStyle();
        if (Event.current.type == EventType.keyUp && Event.current.keyCode == KeyCode.V && Event.current.shift)
            PasteSelectedStyle();
    }

    private void DrawDefault()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        drawDefaultInspector = EditorGUILayout.Foldout(drawDefaultInspector, "Default", EditorStyles.foldout);

        if (drawDefaultInspector)
            DrawDefaultInspector();

        EditorGUILayout.EndVertical();
    }

    private void DrawStyles()
    {

        EditorGUILayout.BeginVertical();
        selectedStyle = EditorGUILayout.Popup("CustomStyles", selectedStyle, customStylesNames.ToArray());

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (customStyles.Count > 0)
        {
            SerializedObject serializedGuiStyle = new SerializedObject(target);
            SerializedProperty iterator = serializedGuiStyle.FindProperty("customStyles.Array.data[" + selectedStyle + "]");

            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren) && !iterator.propertyPath.Contains("customStyles.Array.data[" + (selectedStyle + 1) + "]") && iterator.propertyPath != "m_Settings")
                enterChildren = EditorGUILayout.PropertyField(iterator);

            serializedGuiStyle.ApplyModifiedProperties();

            if (Skin.customStyles[selectedStyle].name == string.Empty)
                Skin.customStyles[selectedStyle].name = "Empty";

            Skin.customStyles[selectedStyle].name = CheckRepeatedNames(Skin.customStyles[selectedStyle]);

            EditorUtility.SetDirty(Skin);

            RefreshLists();
        }

        EditorGUILayout.EndVertical();
      
    }


    public override void OnInspectorGUI()
    {
        CheckEvents();

        DrawButtons();

        DrawStyles();

        DrawDefault();
    }

    #endregion
}