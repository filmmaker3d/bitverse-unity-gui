using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BitTextArea))]
public class BitTextAreaEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(200, 100);
    }
}
 
