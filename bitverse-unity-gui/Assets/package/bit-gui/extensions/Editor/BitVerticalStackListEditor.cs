using Bitverse.Unity.Gui;
using UnityEditor;


[CustomEditor(typeof (BitVerticalGroup))]
public class BitVerticalStackListEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(100, 200);
    }
}