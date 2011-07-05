using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;

//using Irrelevant.Assets;


public partial class BitControlEditor
{
    private static bool _displayChanges;

    private static bool _groupFoldout;

    private static bool _positionFoldout;
    private static bool _anchorFoldout;
    private static bool[] AnchorFlags = new bool[4];
    private static bool[] ResizeAnchorFlags = new bool[4];
    private static readonly bool[] SizeFoldout = new bool[2];
    private static bool _mouseButtonFoldout;

    private static bool _animationFoldout;
    private static bool _animationPositionFoldout;
    private static bool _animationScalePivotFoldout;
    private static bool _animationScaleFoldout;
    private static bool _animationRotationPivotFoldout;

    private bool defaultIsStatic = false;
    public override void OnInspectorGUI()
    {
        if (!(target is BitControl))
        {
            return;
        }

        BitControl control = (BitControl)target;

        if (GUILayout.Button("New ID"))
        {
            control.ID = Guid.NewGuid();
        }

        base.OnInspectorGUI();
        EditorGUI.indentLevel = 0;
        MakeMouseButtonEditor(control);
        MakeAnimationEditor();
        MakeChildInGroup(control);
        MakePositionEditor(control);
        MakeSizeEditorMinSize(control);
        MakeSizeEditorMaxSize(control);
        EditorGUI.indentLevel = 0;
        control.Anchor = MakeAnchorEditor("Anchor", control.Anchor, ref AnchorFlags);
        EditorGUI.indentLevel = 0;
        MakeSpecificEditors(control);
        //MakeAudioEditor(control);

        if (_displayChanges)
        {
            ForceDisplayChanges();
            _displayChanges = false;
        }

        CheckThisContent(control, defaultIsStatic);
        GUILayout.Space(10);

        defaultIsStatic = GUILayout.Toggle(defaultIsStatic, "Use Static as Default");

        if (defaultIsStatic)
        {
            if (GUILayout.Button("Change all blank Text kind to STATIC"))
            {
                TextKindCheck(defaultIsStatic);
            }
        }
        else
        {
            if (GUILayout.Button("Change all blank Text kind to DINAMIC"))
            {
                TextKindCheck(defaultIsStatic);
            }
        }


    }

    protected static void MakeSizeEditorMinSize(BitControl control)
    {
        EditorGUI.indentLevel = 0;

        SizeFoldout[0] = EditorGUILayout.Foldout(SizeFoldout[0], "Min Size");
        if (SizeFoldout[0])
        {
            EditorGUI.indentLevel = 2;
            float width = EditorGUILayout.FloatField("Width", control.MinSize.Width);
            float height = EditorGUILayout.FloatField("Height", control.MinSize.Height);
            if (GUI.changed)
            {
                control.MinSize = new Size(width, height);
                _displayChanges = true;
            }
        }
    }

    protected static void MakeSizeEditorMaxSize(BitControl control)
    {
        EditorGUI.indentLevel = 0;

        SizeFoldout[1] = EditorGUILayout.Foldout(SizeFoldout[1], "Max Size");
        if (SizeFoldout[1])
        {
            EditorGUI.indentLevel = 2;
            float width = EditorGUILayout.FloatField("Width", control.MaxSize.Width);
            float height = EditorGUILayout.FloatField("Height", control.MaxSize.Height);
            if (GUI.changed)
            {
                control.MaxSize = new Size(width, height);
                _displayChanges = true;
            }
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
            ((BitControl)target).transform.Rotate(360.0f, 0.0f, 0.0f);
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

    public virtual void MakePositionEditor(BitControl control)
    {
        const int foldoutIndentSize = 2;

        _positionFoldout = EditorGUILayout.Foldout(_positionFoldout, "Position");
        if (_positionFoldout)
        {
            EditorGUI.indentLevel += foldoutIndentSize;

            EditorGUILayout.BeginVertical();

            float x = EditorGUILayout.IntField("x", (int)control.Position.x);
            float y = EditorGUILayout.IntField("y", (int)control.Position.y);


            float width = EditorGUILayout.IntField("Width", (int)control.Size.Width);
            float height = EditorGUILayout.IntField("Height", (int)control.Size.Height);

            EditorGUILayout.EndVertical();


            EditorGUI.indentLevel -= foldoutIndentSize;

            if (GUI.changed)
            {
                //control.Position = new Rect(x, y, width, height);
                control.Location = new Point(x, y);
                control.Size = new Size(width, height);
                _displayChanges = true;
            }
        }
    }

    protected virtual AnchorStyles MakeAnchorEditor(string label, AnchorStyles anchorstyles, ref bool[] flags)
    {
        string anchors = "";
        if (anchorstyles == AnchorStyles.None)
        {
            anchors = "None";
        }
        else
        {
            if ((anchorstyles & AnchorStyles.Top) == AnchorStyles.Top)
            {
                anchors += "Top; ";
            }
            if ((anchorstyles & AnchorStyles.Left) == AnchorStyles.Left)
            {
                anchors += "Left; ";
            }
            if ((anchorstyles & AnchorStyles.Bottom) == AnchorStyles.Bottom)
            {
                anchors += "Bottom; ";
            }
            if ((anchorstyles & AnchorStyles.Right) == AnchorStyles.Right)
            {
                anchors += "Right;";
            }
        }
        _anchorFoldout = EditorGUILayout.Foldout(_anchorFoldout, label + " : " + anchors);
        if (_anchorFoldout)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(78);
            flags[0] = GUILayout.Toggle((anchorstyles & AnchorStyles.Top) == AnchorStyles.Top, "", "button",
                                              GUILayout.Width(10), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            GUILayout.BeginVertical();
            GUILayout.Space(13);
            flags[1] = GUILayout.Toggle((anchorstyles & AnchorStyles.Left) == AnchorStyles.Left, "", "button",
                                              GUILayout.Width(20), GUILayout.Height(10));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Box("", GUILayout.Width(40), GUILayout.Height(30));
            GUILayout.Space(5);
            GUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            GUILayout.Space(13);
            flags[3] = GUILayout.Toggle((anchorstyles & AnchorStyles.Right) == AnchorStyles.Right, "", "button",
                                              GUILayout.Width(20), GUILayout.Height(10));
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(79);
            flags[2] = GUILayout.Toggle((anchorstyles & AnchorStyles.Bottom) == AnchorStyles.Bottom, "",
                                              "button", GUILayout.Width(10), GUILayout.Height(20));
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                anchorstyles = (flags[0])
                                    ? anchorstyles | AnchorStyles.Top
                                    : anchorstyles & ~AnchorStyles.Top;
                anchorstyles = (flags[1])
                                    ? anchorstyles | AnchorStyles.Left
                                    : anchorstyles & ~AnchorStyles.Left;
                anchorstyles = (flags[2])
                                    ? anchorstyles | AnchorStyles.Bottom
                                    : anchorstyles & ~AnchorStyles.Bottom;
                anchorstyles = (flags[3])
                                    ? anchorstyles | AnchorStyles.Right
                                    : anchorstyles & ~AnchorStyles.Right;

                anchorstyles &= AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
        }
        return anchorstyles;
    }

    protected virtual void MakeSpecificEditors(BitControl control)
    {
        if (control.GetType().IsAssignableFrom(typeof(BitVerticalGroup)))
        {
            bool invert = EditorGUILayout.BeginToggleGroup("Invert", ((BitVerticalGroup)control).Invert);
            if (GUI.changed)
            {
                ((AbstractBitLayoutGroup)control).Invert = invert;
                _displayChanges = true;
            }
        }
        if (control.GetType().IsAssignableFrom(typeof(BitHorizontalGroup)))
        {
            bool invert = EditorGUILayout.BeginToggleGroup("Invert", ((BitHorizontalGroup)control).Invert);
            if (GUI.changed)
            {
                ((AbstractBitLayoutGroup)control).Invert = invert;
                _displayChanges = true;
            }
        }
        if (control.GetType().IsAssignableFrom(typeof(BitResizeHandler)))
        {
            ((BitResizeHandler)control).ResizeAnchor = MakeAnchorEditor("ResizeAnchor", ((BitResizeHandler)control).ResizeAnchor, ref ResizeAnchorFlags);
        }
    }

    /*protected virtual void MakeAudioEditor(BitControl control)
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
    }*/

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