using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BitVerticalSlider))]
public class BitVerticalSliderEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(10, 100);
    }
}
 
