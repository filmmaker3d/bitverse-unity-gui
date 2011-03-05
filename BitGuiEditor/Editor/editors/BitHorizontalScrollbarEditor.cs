using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(BitHorizontalScrollbar))]
public class BitHorizontalScrollbarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        BitControl p = control.Parent;
        if (p != null)
        {
            Rect parentPosition = p.Position;
            control.Location = new Point(0, parentPosition.height - 20);
            control.Size = new Size(parentPosition.width, 20);
            control.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
        }
        else
        {
            control.Size = new Size(200, 20);
        }
    }
}
 
