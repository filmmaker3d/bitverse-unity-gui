using Bitverse.Unity.Gui;
using UnityEditor;


[CustomEditor(typeof (BitHorizontalSegmentedProgressBar))]
public class BitHorizontalSegmentedProgressBarEditor : BitControlEditor
{
    protected override void OnAddControl(BitControl control)
    {
        control.Size = new Size(160, 20);
    }
}