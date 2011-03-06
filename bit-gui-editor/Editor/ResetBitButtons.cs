using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResetBitButtons
{
	[MenuItem("Tools/GUI/Reset BitButtons")]
	private static void RunResetBitButtons()
	{
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);

        foreach(Object o in selection)
        {
            GameObject go = (GameObject)o;
            Component[] controls = go.GetComponentsInChildren(typeof(BitControl));
            foreach(BitControl control in controls)
            {
                BitButton b = control as BitButton;
                if (b != null)
                {
                    b.LeftButton = true;
                    b.MiddleButton = false;
                    b.RightButton = false;
                }
            }
        }
        Debug.Log("Reset Bit Buttons Ended");
	}
}
