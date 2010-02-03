using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;


internal class BitGuiEditorToolbox : EditorWindow
{
	[MenuItem("Window/BitGUI Toolbox")]
	public static void MenuOption()
	{
		EditorWindow window = GetWindow(typeof(BitGuiEditorToolbox));
		window.autoRepaintOnSceneChange = true;
		window.title = "BitGUI Toolbox";
		window.Show();
	}


	private Vector2 _scrollPos;
	private bool _componentsFoldout = true;
	//private bool _optionsFoldout = true;
	private bool _commonFoldout = true;
	private bool _containersFoldout = true;
	private bool _scrollFoldout;
	private bool _advancedFoldout;

	private static void BeginGroup()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Box("", GUIStyle.none, GUILayout.Width(20f));
		GUILayout.BeginVertical();
	}

	private static void EndGroup()
	{
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	public void OnGUI()
	{
		_scrollPos = GUILayout.BeginScrollView(_scrollPos);
		DrawControls();
		//DrawOptions();
		GUILayout.EndScrollView();
	}

	private void DrawControls()
	{
		_componentsFoldout = EditorGUILayout.Foldout(_componentsFoldout, "Controls");
		if (!_componentsFoldout)
		{
			return;
		}
		BeginGroup();
		_commonFoldout = EditorGUILayout.Foldout(_commonFoldout, "Common Controls");
		if (_commonFoldout)
		{
			BeginGroup();

			AddComponent(typeof(BitLabel), true);
			AddComponent(typeof(BitButton), true);
			AddComponent(typeof(BitRepeatButton), true);
			AddComponent(typeof(BitToggle), true);
			AddComponent(typeof(BitTextField), false);
			AddComponent(typeof(BitTextArea), false);
			AddComponent(typeof(BitPasswordField), false);
			AddComponent(typeof(BitHorizontalSlider), false);
			AddComponent(typeof(BitVerticalSlider), false);
			AddComponent(typeof(BitBox), false);
			AddComponent(typeof(BitList), false);
			AddComponent(typeof(BitGridList), false);
			AddComponent(typeof(BitDropDown), false);
			AddSpecialComponent(typeof(BitPopup), false);
			//AddComponent(typeof(BitMenu), false);
			//AddComponent(typeof(BitMenuItem));

			EndGroup();
		}

		_containersFoldout = EditorGUILayout.Foldout(_containersFoldout, "Containers");
		if (_containersFoldout)
		{
			BeginGroup();
			AddSpecialComponent(typeof(BitWindow), true);
			AddSpecialComponent(typeof(BitForm), false);
			AddComponent(typeof(BitGroup), false);
			EndGroup();
		}

		_scrollFoldout = EditorGUILayout.Foldout(_scrollFoldout, "Scroll");
		if (_scrollFoldout)
		{
			BeginGroup();
			AddComponent(typeof(BitScrollView), false);
			AddComponent(typeof(BitVerticalScrollbar), false);
			AddComponent(typeof(BitHorizontalScrollbar), false);
			EndGroup();
		}

		_advancedFoldout = EditorGUILayout.Foldout(_advancedFoldout, "Advanced");
		if (_advancedFoldout)
		{
			BeginGroup();
			AddComponent(typeof(BitDrawTexture), false);
			EndGroup();
		}
		EndGroup();
	}

	//private void DrawOptions()
	//{
	//    _optionsFoldout = EditorGUILayout.Foldout(_optionsFoldout, "Options");
	//    if (_optionsFoldout)
	//    {
	//        BeginGroup();
	//        BitControlEditor.SnapToGrid = EditorGUILayout.BeginToggleGroup("Snap to grid", BitControlEditor.SnapToGrid);
	//        if (BitControlEditor.SnapToGrid)
	//        {
	//            BitControlEditor.Grid = EditorGUILayout.Vector2Field("Grid", BitControlEditor.Grid);
	//        }

	//        BitControlEditor.DrawPadding = GUILayout.Toggle(BitControlEditor.DrawPadding, "Draw Padding Rect");
	//        BitControlEditor.DrawMargin = GUILayout.Toggle(BitControlEditor.DrawMargin, "Draw Margin Rect");
	//        EndGroup();
	//    }
	//}

	public static Type ControlTypeToCreate;
	private static bool GenerateDefaultContent;

	private static void AddComponent(Type controlType, bool generateDefaultContent)
	{
		GUILayout.BeginHorizontal();
		bool toggle = GUILayout.Toggle(ControlTypeToCreate == controlType, DefaultName(controlType));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		if (!GUI.changed)
			return;
		if (toggle)
		{
			ControlTypeToCreate = controlType;
			GenerateDefaultContent = generateDefaultContent;
		}
		else
		{
			ControlTypeToCreate = null;
		}
		GUI.changed = false;
	}

	private static void AddSpecialComponent(Type controlType, bool generateDefaultContent)
	{
		GUILayout.BeginHorizontal();

		if (GUILayout.Button(DefaultName(controlType)))
		{
			if (Selection.activeGameObject != null)
			{
				string name = GenerateSecureName(controlType);
				BitContainer parent = (BitContainer)Selection.activeTransform.GetComponent(typeof(BitContainer));

				BitControl control = null;
				if (parent != null)
				{
					control = parent.AddControl(controlType, name);
					if (generateDefaultContent)
					{
						control.Content.text = name;
					}
					Selection.activeTransform = control.transform;
				}
				else
				{
					GameObject go = new GameObject();
					Component c = go.AddComponent(controlType);
					go.transform.parent = Selection.activeTransform;
					go.name = name;
					if (c is BitControl)
					{
						control = ((BitControl)c);
						if (generateDefaultContent)
						{
							control.Content.text = name;
						}
					}
					Selection.activeGameObject = go;
				}
				AddControlCount(controlType);
				BitControlEditor.AddControl(control);
			}
			else
			{
				EditorApplication.Beep();
				EditorUtility.DisplayDialog("Error", "You must select a parent on Hierarchy panel to add this Control.",
											"OK");
			}
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}

	public static BitControl CreateComponent(BitContainer parent, Vector2 mousePosition)
	{
		if (ControlTypeToCreate == null || parent == null)
		{
			return null;
		}

		string name = GenerateSecureName(ControlTypeToCreate);
		BitControl c = parent.AddControl(ControlTypeToCreate, name);
		Rect ap = c.AbsolutePosition;
		c.AbsolutePosition = new Rect(mousePosition.x, mousePosition.y, ap.width, ap.height);
		if (GenerateDefaultContent)
		{
			c.Content.text = name;
		}
		Selection.activeTransform = c.transform;

		AddControlCount(ControlTypeToCreate);

		return c;
	}

	private static readonly Dictionary<Type, int> ControlsCount = new Dictionary<Type, int>();

	private static readonly Regex NameRegex = new Regex(@"^([^\d]+)(\d+)$");

	private static string GenerateSecureName(Type type)
	{
		string defaultName = DefaultName(type);
		Component parent = (Component)FindObjectOfType(typeof(BitForm));
		if (parent == null)
		{
			return defaultName + "1";
		}

		Component[] c = parent.GetComponentsInChildren(type);
		if (c == null || c.Length == 0)
		{
			return defaultName + "1";
		}

		int maxValue = 1;
		foreach (Component component in c)
		{
			Match m = NameRegex.Match(component.name);
			if (!m.Success || !m.Groups[1].ToString().Equals(defaultName))
			{
				continue;
			}
			int value = int.Parse(m.Groups[2].ToString());
			if (value > maxValue)
			{
				maxValue = value;
			}
		}
		SetControlCount(type, maxValue);
		return defaultName + (maxValue + 1);
	}

	private static string DefaultName(Type type)
	{
		return type.Name.Substring("Bit".Length);
	}


	private static void SetControlCount(Type type, int value)
	{
		if (ControlsCount.ContainsKey(type))
		{
			ControlsCount[type] = value;
		}
		else
		{
			ControlsCount.Add(type, value);
		}
	}

	private static void AddControlCount(Type type)
	{
		if (ControlsCount.ContainsKey(type))
		{
			ControlsCount[type]++;
		}
		else
		{
			ControlsCount.Add(type, 0);
		}
	}
}