using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BitPicture))]
public class BitPictureEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 100);
    }
}

 
