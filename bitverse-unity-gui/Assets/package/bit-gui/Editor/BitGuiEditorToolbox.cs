using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
using Object = UnityEngine.Object;


public partial class BitGuiEditorToolbox : EditorWindow
{
	private class ControlInfo
	{
		public readonly Type Type;
		public readonly bool IsSpecial;
		public readonly bool GenerateDefaultContent;
		public readonly GUIContent Content;

		public ControlInfo(Type type, bool isSpecial, bool generateDefaultContent, GUIContent content)
		{
			Type = type;
			Content = content;
			IsSpecial = isSpecial;
			GenerateDefaultContent = generateDefaultContent;
		}
	}


	private class ButtonInfo
	{
		public bool Foldout;
		public string Title;
		public List<ControlInfo> Controls;

		public ButtonInfo(bool foldout, string title)
		{
			Foldout = foldout;
			Title = title;
			Controls = new List<ControlInfo>();
		}

		public void AddComponent(Type type, bool generateDefaultContent)
		{
			Controls.Add(new ControlInfo(type, false, generateDefaultContent, GetGUIContent(type)));
		}

		public void AddSpecialComponent(Type type, bool generateDefaultContent)
		{
			Controls.Add(new ControlInfo(type, true, generateDefaultContent, GetGUIContent(type)));
		}
	}


	private const float SpaceBetweenGroups = 5;

	private List<ButtonInfo> _buttonsInfo;


	[MenuItem("Window/BitGUI Toolbox")]
	public static void MenuOption()
	{
		BitGuiEditorToolbox window = (BitGuiEditorToolbox) GetWindow(typeof (BitGuiEditorToolbox));
		window.autoRepaintOnSceneChange = true;
		window.title = "BitGUI Toolbox";
		window.Show();
	}

	private static Dictionary<string, Texture2D> _buttonIcons;

	private Vector2 _scrollPos = new Vector2(0, 0);
	private bool _componentsFoldout = true;

	private static Dictionary<string, Texture2D> LoadTextures()
	{
		Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
		Object[] allImages = Resources.LoadAll("editor icons");
		foreach (Object o in allImages)
		{
			textures.Add(o.name.ToLower(), (Texture2D) o);
		}
		return textures;
	}

	private static void BeginGroup()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginVertical();
	}

	private static void EndGroup()
	{
		GUILayout.Space(SpaceBetweenGroups);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private GUIStyle _buttonStyle;

	public void OnGUI()
	{
		if (_buttonsInfo == null)
		{
			_buttonStyle = new GUIStyle(GUI.skin.button);
			_buttonStyle.alignment = TextAnchor.MiddleLeft;
			_buttonStyle.normal.background = GUI.skin.label.normal.background;
			_buttonIcons = LoadTextures();
			MakeControlsInfo();
			return;
		}

		try
		{
			_scrollPos = GUILayout.BeginScrollView(_scrollPos);
			DrawControls();
			//DrawOptions();
			GUILayout.EndScrollView();
		}
		catch
		{
			Repaint();
		}
	}

	private void MakeControlsInfo()
	{
		_buttonsInfo = new List<ButtonInfo>();
		ButtonInfo buttonInfo = new ButtonInfo(true, "Common");
		buttonInfo.AddComponent(typeof (BitLabel), true);
		buttonInfo.AddComponent(typeof (BitButton), true);
		buttonInfo.AddComponent(typeof (BitRepeatButton), true);
		buttonInfo.AddComponent(typeof (BitToggle), true);
        buttonInfo.AddComponent(typeof(BitBox), false);
        buttonInfo.AddComponent(typeof(BitPicture), false);
        buttonInfo.AddComponent(typeof(BitSprite), false);
		buttonInfo.AddComponent(typeof(BitDropDown), false);
		buttonInfo.AddSpecialComponent(typeof (BitWindow), true);
		AddEnhancedCommonControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(true, "Text");
		buttonInfo.AddComponent(typeof (BitTextField), false);
		buttonInfo.AddComponent(typeof (BitTextArea), false);
		buttonInfo.AddComponent(typeof (BitPasswordField), false);
		buttonInfo.AddComponent(typeof (BitFilteredTextField), false);
		AddEnhancedTextControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(true, "Group");
		buttonInfo.AddComponent(typeof (BitGroup), false);
		AddEnhancedGroupControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(true, "List");
		buttonInfo.AddComponent(typeof (BitList), false);
		buttonInfo.AddComponent(typeof (BitGridList), false);
		AddEnhancedListControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(true, "Popup");
		buttonInfo.AddSpecialComponent(typeof(BitPopup), false);
		buttonInfo.AddSpecialComponent(typeof(BitContextMenu), false);
		buttonInfo.AddComponent(typeof(BitContextMenuItem), true);
		AddEnhancedPopupControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(false, "Progress");
		buttonInfo.AddComponent(typeof (BitHorizontalProgressBar), false);
		buttonInfo.AddComponent(typeof (BitVerticalProgressBar), false);
		AddEnhancedProgressControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(false, "Scroll");
		buttonInfo.AddComponent(typeof (BitScrollView), false);
		buttonInfo.AddComponent(typeof (BitVerticalScrollbar), false);
		buttonInfo.AddComponent(typeof (BitHorizontalScrollbar), false);
		AddEnhancedScrollControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(false, "Slider");
		buttonInfo.AddComponent(typeof (BitHorizontalSlider), false);
		buttonInfo.AddComponent(typeof (BitVerticalSlider), false);
		AddEnhancedSliderControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(false, "Tab");
		buttonInfo.AddComponent(typeof (BitTabbedPane), false);
		buttonInfo.AddComponent(typeof (BitTab), true);
		AddEnhancedTabControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

        buttonInfo = new ButtonInfo(false, "Viewer");
        buttonInfo.AddComponent(typeof(BitModelViewer), false);
        AddEnhancedTabControls(buttonInfo);
        _buttonsInfo.Add(buttonInfo);

		buttonInfo = new ButtonInfo(true, "Stage");
		buttonInfo.AddSpecialComponent(typeof (BitStage), false);
		buttonInfo.AddSpecialComponent(typeof (BitEditorStage), false);
		AddEnhancedStageControls(buttonInfo);
		_buttonsInfo.Add(buttonInfo);

        buttonInfo = new ButtonInfo(true, "Resize");
        buttonInfo.AddSpecialComponent(typeof(BitResizeHandler), false);
        AddEnhancedStageControls(buttonInfo);
        _buttonsInfo.Add(buttonInfo);
	}

	private void DrawControls()
	{
		_componentsFoldout = EditorGUILayout.Foldout(_componentsFoldout, "Controls");
		if (!_componentsFoldout)
		{
			return;
		}

		BeginGroup();

		for (int i = 0, count = _buttonsInfo.Count; i < count; i++)
		{
			_buttonsInfo[i].Foldout = EditorGUILayout.Foldout(_buttonsInfo[i].Foldout, _buttonsInfo[i].Title);
			if (_buttonsInfo[i].Foldout)
			{
				BeginGroup();
				foreach (ControlInfo controlInfo in _buttonsInfo[i].Controls)
				{
					if (controlInfo.IsSpecial)
					{
						AddSpecialComponent(controlInfo);
					}
					else
					{
						AddComponent(controlInfo);
					}
				}
				EndGroup();
			}
			else
			{
				GUILayout.Space(SpaceBetweenGroups);
			}
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
	private static bool _generateDefaultContent;

	private void AddComponent(ControlInfo info)
	{
		GUILayout.BeginHorizontal();
		bool toggle = GUILayout.Toggle(ControlTypeToCreate == info.Type, info.Content, _buttonStyle, GUILayout.Width(210));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		if (!GUI.changed)
			return;
		if (toggle)
		{
			ControlTypeToCreate = info.Type;
			_generateDefaultContent = info.GenerateDefaultContent;
		}
		else
		{
			ControlTypeToCreate = null;
		}
		GUI.changed = false;
	}

	private void AddSpecialComponent(ControlInfo info)
	{
		GUILayout.BeginHorizontal();

		if (GUILayout.Button(info.Content, _buttonStyle, GUILayout.Width(160)))
		{
			if (Selection.activeGameObject != null)
			{
				string controlName = GenerateSecureName(info.Type);
				BitContainer parent = (BitContainer) Selection.activeTransform.GetComponent(typeof (BitContainer));

				BitControl control = null;
				if (parent != null)
				{
					control = parent.AddControl(info.Type, controlName);
					if (info.GenerateDefaultContent)
					{
						control.Content.text = controlName;
					}
					Selection.activeTransform = control.transform;
				}
				else
				{
					GameObject go = new GameObject();
					Component c = go.AddComponent(info.Type);
					go.transform.parent = Selection.activeTransform;
					go.name = controlName;
					if (c is BitControl)
					{
						control = ((BitControl) c);
						if (info.GenerateDefaultContent)
						{
							control.Content.text = controlName;
						}
					}
					Selection.activeGameObject = go;
				}
				AddControlCount(info.Type);
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
		if (_generateDefaultContent)
		{
			c.Content.text = name;
		}
		Selection.activeTransform = c.transform;

		AddControlCount(ControlTypeToCreate);

		return c;
	}

	private static GUIContent GetGUIContent(Type controlType)
	{
		string controlName = DefaultName(controlType);
		string s = controlName.ToLower();
		return new GUIContent(" " + controlName, _buttonIcons.ContainsKey(s) ? _buttonIcons[s] : null);
	}

	private static readonly Dictionary<Type, int> ControlsCount = new Dictionary<Type, int>();

	private static readonly Regex NameRegex = new Regex(@"^([^\d]+)(\d+)$");

	private static string GenerateSecureName(Type type)
	{
		string defaultName = DefaultName(type);
		Component parent = (Component) FindObjectOfType(typeof (BitStage));
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