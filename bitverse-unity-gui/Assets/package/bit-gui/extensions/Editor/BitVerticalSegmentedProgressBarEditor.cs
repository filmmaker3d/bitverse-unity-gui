using Bitverse.Unity.Gui;
using UnityEditor;


[CustomEditor(typeof (BitVerticalSegmentedProgressBar))]
public class BitVerticalSegmentedProgressBarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(20, 160);
    }
}