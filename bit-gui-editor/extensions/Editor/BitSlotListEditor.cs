using Bitverse.Unity.Gui;
using UnityEditor;


[CustomEditor(typeof (BitSlotList))]
public class BitSlotListEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(260, 160);
    }
}