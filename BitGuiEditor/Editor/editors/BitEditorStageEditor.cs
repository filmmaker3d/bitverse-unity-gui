using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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
