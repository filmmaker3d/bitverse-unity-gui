using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;

//using Irrelevant.Assets;


public partial class BitControlEditor
{
	private static bool _displayChanges;

	private static bool _groupFoldout;
	private static bool _fixPositionFoldout;

	private static bool _positionFoldout;
	private static bool _audioFoldout;
	private static bool _anchorFoldout;
	private static readonly bool[] AnchorFlags = new bool[4];
	private static readonly bool[] SizeFoldout = new bool[2];
    private static bool _mouseButtonFoldout;

    private static bool _animationFoldout;
    private static bool _animationPositionFoldout;
    private static bool _animationScalePivotFoldout;
    private static bool _animationScaleFoldout;
    private static bool _animationRotationPivotFoldout;


    public override void OnInspectorGUI()
	{
		if (!(target is BitControl))
		{
			return;
		}

		BitControl control = (BitControl) target;

		base.OnInspectorGUI();
		EditorGUI.indentLevel = 0;
        MakeMouseButtonEditor(control);
	    MakeAnimationEditor();
		MakeChildInGroup(control);
		MakePositionEditor(control);
		control.MinSize = MakeSizeEditor(control.MinSize, "Min Size", ref SizeFoldout[0]);
		control.MaxSize = MakeSizeEditor(control.MaxSize, "Max Size", ref SizeFoldout[1]);
		EditorGUI.indentLevel = 0;
		MakeAnchorEditor(control);
		EditorGUI.indentLevel = 0;
		MakeSpecificEditors(control);
		MakeAudioEditor(control);

		if (_displayChanges)
		{
			ForceDisplayChanges();
			_displayChanges = false;
		}
	}

    private void MakeMouseButtonEditor(BitControl control)
    {
        _mouseButtonFoldout = EditorGUILayout.Foldout(_mouseButtonFoldout, "Mouse Buttons");
        if (_mouseButtonFoldout)
        {
            EditorGUI.indentLevel += 2;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(31);
            GUILayout.Label("Alignment");
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(GUI.skin.box);
            control.LeftButton = GUILayout.Toggle(control.LeftButton, "", "button", GUILayout.Width(20), GUILayout.Height(10));
            GUILayout.Space(10);
            control.MiddleButton = GUILayout.Toggle(control.MiddleButton, "", "button", GUILayout.Width(20), GUILayout.Height(10));
            GUILayout.Space(10);
            control.RightButton = GUILayout.Toggle(control.RightButton, "", "button", GUILayout.Width(20), GUILayout.Height(10));
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel -= 2;
        }
    }

    private Rect RectField(ref bool foldout, string foldoutName, Rect inputRect)
    {
        foldout = EditorGUILayout.Foldout(foldout, foldoutName);
        if (foldout)
        {
            EditorGUI.indentLevel += 2;

            inputRect.x = EditorGUILayout.FloatField("X", inputRect.x);
            inputRect.y = EditorGUILayout.FloatField("Y", inputRect.y);
            inputRect.width = EditorGUILayout.FloatField("Width", inputRect.width);
            inputRect.height = EditorGUILayout.FloatField("Height", inputRect.height);

            EditorGUI.indentLevel -= 2;
        }

        return inputRect;
    }

    private Vector2 Vector2Field(ref bool foldout, string foldoutName, Vector2 input)
    {
        foldout = EditorGUILayout.Foldout(foldout, foldoutName);
        if (foldout)
        {
            EditorGUI.indentLevel += 2;

            input.x = EditorGUILayout.FloatField("X", input.x);
            input.y = EditorGUILayout.FloatField("Y", input.y);

            EditorGUI.indentLevel -= 2;
        }

        return input;
    }

    private void MakeAnimationEditor()
    {
        if (!(target is BitControl)) return;
        BitControl control = (BitControl)target;

        EditorGUI.indentLevel = 0;

		_animationFoldout = EditorGUILayout.Foldout(_animationFoldout, "Animation Relative Values");
		if (_animationFoldout)
        {
            EditorGUI.indentLevel += 2;
            control.Animating = EditorGUILayout.Toggle("Animating", control.Animating);

            if (control.Animating)
            {
                control.AnimationColor = EditorGUILayout.ColorField("Color", control.AnimationColor);
                control.AnimationPosition = RectField(ref _animationPositionFoldout, "Position",
                                                      control.AnimationPosition);
                control.AnimationScalePivot = Vector2Field(ref _animationScalePivotFoldout, "Scale Pivot",
                                                           control.AnimationScalePivot);
                    //RectField(ref _animationScalePivotFoldout, "Scale Pivot", control.AnimationScalePivot);
                control.AnimationScale = Vector2Field(ref _animationScaleFoldout, "Scale", control.AnimationScale);
                control.AnimationRotationPivot = Vector2Field(ref _animationRotationPivotFoldout, "Rotation Pivot",
                                                              control.AnimationRotationPivot);
                EditorGUI.indentLevel += 1;
                control.AnimationRotationAngle = EditorGUILayout.FloatField("Angle", control.AnimationRotationAngle);
                EditorGUI.indentLevel -= 1;
            }

            EditorGUI.indentLevel -= 2;
		}
    }

	private void ForceDisplayChanges()
	{
		// SUGGESTION FROM http://forum.unity3d.com/viewtopic.php?p=230373
		if (target is BitControl)
		{
			((BitControl) target).transform.Rotate(360.0f, 0.0f, 0.0f);
		}
	}

	protected static Size MakeSizeEditor(Size size, string nameLabel, ref bool foldout)
	{
		EditorGUI.indentLevel = 0;
		float width = size.Width;
		float height = size.Height;

		foldout = EditorGUILayout.Foldout(foldout, nameLabel);
		if (foldout)
		{
			EditorGUI.indentLevel = 2;
			width = EditorGUILayout.FloatField("Width", width);
			height = EditorGUILayout.FloatField("Height", height);
		}

		return new Size(width, height);
	}

	protected virtual void MakePositionEditor(BitControl control)
	{
		const int foldoutIndentSize = 2;

		_positionFoldout = EditorGUILayout.Foldout(_positionFoldout, "Position");
		if (_positionFoldout)
		{
			EditorGUI.indentLevel += foldoutIndentSize;

			EditorGUILayout.BeginVertical();

			float x = EditorGUILayout.FloatField("x", control.Position.x);
			float y = EditorGUILayout.FloatField("y", control.Position.y);
			float width = EditorGUILayout.FloatField("Width", control.Position.width);
			float height = EditorGUILayout.FloatField("Height", control.Position.height);

			EditorGUILayout.EndVertical();


			EditorGUI.indentLevel -= foldoutIndentSize;

			if (GUI.changed)
			{
				control.Location = new Point(x, y);
				control.Size = new Size(width, height);
				_displayChanges = true;
			}
		}
	}

	protected virtual void MakeAnchorEditor(BitControl control)
	{
		string anchors = "";
		if (control.Anchor == AnchorStyles.None)
		{
			anchors = "None";
		}
		else
		{
			if ((control.Anchor & AnchorStyles.Top) == AnchorStyles.Top)
			{
				anchors += "Top; ";
			}
			if ((control.Anchor & AnchorStyles.Left) == AnchorStyles.Left)
			{
				anchors += "Left; ";
			}
			if ((control.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
			{
				anchors += "Bottom; ";
			}
			if ((control.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
			{
				anchors += "Right;";
			}
		}
		_anchorFoldout = EditorGUILayout.Foldout(_anchorFoldout, "Anchor: " + anchors);
		if (_anchorFoldout)
		{
			EditorGUILayout.BeginVertical();

			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(78);
			AnchorFlags[0] = GUILayout.Toggle((control.Anchor & AnchorStyles.Top) == AnchorStyles.Top, "", "button",
			                                  GUILayout.Width(10), GUILayout.Height(20));
			EditorGUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(40);
			GUILayout.BeginVertical();
			GUILayout.Space(13);
			AnchorFlags[1] = GUILayout.Toggle((control.Anchor & AnchorStyles.Left) == AnchorStyles.Left, "", "button",
			                                  GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Box("", GUILayout.Width(40), GUILayout.Height(30));
			GUILayout.Space(5);
			GUILayout.EndVertical();

			EditorGUILayout.BeginVertical();
			GUILayout.Space(13);
			AnchorFlags[3] = GUILayout.Toggle((control.Anchor & AnchorStyles.Right) == AnchorStyles.Right, "", "button",
			                                  GUILayout.Width(20), GUILayout.Height(10));
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(79);
			AnchorFlags[2] = GUILayout.Toggle((control.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom, "",
			                                  "button", GUILayout.Width(10), GUILayout.Height(20));
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.EndVertical();

			if (GUI.changed)
			{
				control.Anchor = (AnchorFlags[0])
				                 	? control.Anchor | AnchorStyles.Top
				                 	: control.Anchor & ~AnchorStyles.Top;
				control.Anchor = (AnchorFlags[1])
				                 	? control.Anchor | AnchorStyles.Left
				                 	: control.Anchor & ~AnchorStyles.Left;
				control.Anchor = (AnchorFlags[2])
				                 	? control.Anchor | AnchorStyles.Bottom
				                 	: control.Anchor & ~AnchorStyles.Bottom;
				control.Anchor = (AnchorFlags[3])
				                 	? control.Anchor | AnchorStyles.Right
				                 	: control.Anchor & ~AnchorStyles.Right;
			}
		}
	}

	protected virtual void MakeSpecificEditors(BitControl control)
	{
	}

	protected virtual void MakeAudioEditor(BitControl control)
	{
		EditorGUI.indentLevel = 0;
		_audioFoldout = EditorGUILayout.Foldout(_audioFoldout, "Audio");
		if (_audioFoldout)
		{
			EditorGUI.indentLevel = 2;
			EditorGUILayout.BeginVertical();

		    string mouseClickToggleOn = "";
		    string mouseclickToggleOff = "";
            BitToggle toggle = new BitToggle();
			string mouseUpText = EditorGUILayout.TextField("Mouse Up", control.MouseUpAudioName);
            string mouseDownText = EditorGUILayout.TextField("Mouse Down", control.MouseDownAudioName);
            string mouseClickText = EditorGUILayout.TextField("Mouse Click", control.MouseClickAudioName);
            string mouseEnterText = EditorGUILayout.TextField("Mouse Enter", control.MouseEnterAudioName);
            string mouseExitText = EditorGUILayout.TextField("Mouse Exit", control.MouseExitAudioName);
            if(control is BitToggle)
            {
                toggle = control as BitToggle;
                mouseClickToggleOn = EditorGUILayout.TextField("Mouse Click Toggle On", toggle.ToggleOn);
                mouseclickToggleOff = EditorGUILayout.TextField("Mouse Click Toggle Off", toggle.ToggleOff);
            }
		    EditorGUILayout.EndVertical();

			if (GUI.changed)
			{
				control.MouseUpAudioName = mouseUpText;
                control.MouseDownAudioName = mouseDownText;
                control.MouseClickAudioName = mouseClickText;
                control.MouseEnterAudioName = mouseEnterText;
                control.MouseExitAudioName = mouseExitText;
                if (control is BitToggle)
                {
                    toggle.ToggleOn = mouseClickToggleOn;
                    toggle.ToggleOff = mouseclickToggleOff;
                }
			}
		}
		EditorGUI.indentLevel = 0;
	}

	// Informations only for child of AbstractBitLayoutGroup
	protected virtual void MakeChildInGroup(BitControl control)
	{
		BitControl parent = control.Parent;
		if (!(parent is AbstractBitLayoutGroup))
		{
			return;
		}

		_groupFoldout = EditorGUILayout.Foldout(_groupFoldout, "Group");

		if (!_groupFoldout)
		{
			return;
		}

		EditorGUI.indentLevel += 2;
		EditorGUILayout.BeginVertical();

		int newIndex = EditorGUILayout.IntField("Index", control.Index);
		if (newIndex != control.Index)
		{
			control.Index = newIndex;
		}

		EditorGUILayout.BeginVertical();

		control.FixedWidth = EditorGUILayout.Toggle("Fixed Width", control.FixedWidth);
		control.FixedHeight = EditorGUILayout.Toggle("Fixed Height", control.FixedHeight);

		EditorGUILayout.EndVertical();


		bool before = false;
		bool center = false;
		bool after = false;

		switch (control.Alignment)
		{
			case GrouppingAligments.Top:
			case GrouppingAligments.Left:
				before = true;
				break;
			case GrouppingAligments.Center:
				center = true;
				break;
			case GrouppingAligments.Bottom:
			case GrouppingAligments.Right:
				after = true;
				break;
		}

		bool cbefore, ccenter, cafter;
		bool vertical = (parent is BitVerticalGroup);

		if (vertical)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(31);
			GUILayout.Label("Alignment");
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(GUI.skin.box);
			cbefore = GUILayout.Toggle(before, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.Space(10);
			ccenter = GUILayout.Toggle(center, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.Space(10);
			cafter = GUILayout.Toggle(after, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(31);
			GUILayout.Label("Alignment");
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(GUI.skin.box);
			cbefore = GUILayout.Toggle(before, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.Space(3);
			ccenter = GUILayout.Toggle(center, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.Space(3);
			cafter = GUILayout.Toggle(after, "", "button", GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}

		if (GUI.changed)
		{
			if (cbefore && !before)
			{
				control.Alignment = vertical ? GrouppingAligments.Left : GrouppingAligments.Top;
			}
			else if (ccenter && !center)
			{
				control.Alignment = GrouppingAligments.Center;
			}
			else if (cafter && !after)
			{
				control.Alignment = vertical ? GrouppingAligments.Right : GrouppingAligments.Bottom;
			}
		}


		EditorGUILayout.EndVertical();
		EditorGUI.indentLevel = 0;
	}
}