using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BitVerticalScrollbar))]
public class BitVerticalScrollbarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        BitControl p = control.Parent;
        if (p != null)
        {
            Rect parentPosition = p.Position;
            control.Location = new Point(parentPosition.width - 20, 0);
            control.Size = new Size(20, parentPosition.height);
            control.Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
        }
        else
        {
            control.Size = new Size(20, 100);
        }
    }
}
 
