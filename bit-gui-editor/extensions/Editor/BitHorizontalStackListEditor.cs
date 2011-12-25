using Bitverse.Unity.Gui;
using UnityEditor;

[CustomEditor(typeof (BitHorizontalGroup))]
public class BitHorizontalStackListEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(200, 100);
	}
}



