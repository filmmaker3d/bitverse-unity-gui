using Bitverse.Unity.Gui;
using UnityEditor;

[CustomEditor(typeof(BitWebImage))]
public class BitWebImageEditor : BitControlEditor
{

    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 100);
    }
}