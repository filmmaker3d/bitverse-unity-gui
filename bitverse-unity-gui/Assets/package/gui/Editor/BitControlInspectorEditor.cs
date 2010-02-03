using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;


public partial class BitControlEditor
{
	private bool _anchorFoldout = true;
	private readonly bool[] _anchorFlags = new bool[4];


	public override void OnInspectorGUI()
	{
		if (!(target is BitControl))
		{
			return;
		}

		BitControl control = (BitControl) target;

		base.OnInspectorGUI();
		MakeAnchorEditor(control);
		MakeSpecificEditors(control);
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
			_anchorFlags[0] = GUILayout.Toggle((control.Anchor & AnchorStyles.Top) == AnchorStyles.Top, "", "button",
			                                   GUILayout.Width(10), GUILayout.Height(20));
			EditorGUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Space(40);
			GUILayout.BeginVertical();
			GUILayout.Space(13);
			_anchorFlags[1] = GUILayout.Toggle((control.Anchor & AnchorStyles.Left) == AnchorStyles.Left, "", "button",
			                                   GUILayout.Width(20), GUILayout.Height(10));
			GUILayout.EndVertical();

			GUILayout.BeginVertical();
			GUILayout.Box("", GUILayout.Width(40), GUILayout.Height(30));
			GUILayout.Space(5);
			GUILayout.EndVertical();

			EditorGUILayout.BeginVertical();
			GUILayout.Space(13);
			_anchorFlags[3] = GUILayout.Toggle((control.Anchor & AnchorStyles.Right) == AnchorStyles.Right, "", "button",
			                                   GUILayout.Width(20), GUILayout.Height(10));
			EditorGUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(79);
			_anchorFlags[2] = GUILayout.Toggle((control.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom, "",
			                                   "button", GUILayout.Width(10), GUILayout.Height(20));
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.EndVertical();

			if (GUI.changed)
			{
				control.Anchor = (_anchorFlags[0])
				                 	? control.Anchor | AnchorStyles.Top
				                 	: control.Anchor & ~AnchorStyles.Top;
				control.Anchor = (_anchorFlags[1])
				                 	? control.Anchor | AnchorStyles.Left
				                 	: control.Anchor & ~AnchorStyles.Left;
				control.Anchor = (_anchorFlags[2])
				                 	? control.Anchor | AnchorStyles.Bottom
				                 	: control.Anchor & ~AnchorStyles.Bottom;
				control.Anchor = (_anchorFlags[3])
				                 	? control.Anchor | AnchorStyles.Right
				                 	: control.Anchor & ~AnchorStyles.Right;
			}
		}
	}

	protected virtual void MakeSpecificEditors(BitControl control)
	{
	}
}