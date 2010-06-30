using Bitverse.Unity.Gui;
using UnityEditor;


#region Group

[CustomEditor(typeof (BitHorizontalGroup))]
public class BitHorizontalStackListEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(200, 100);
	}
}


[CustomEditor(typeof (BitVerticalGroup))]
public class BitVerticalStackListEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(100, 200);
	}
}

#endregion


#region list

[CustomEditor(typeof (BitSlotList))]
public class BitSlotListEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(260, 160);
	}
}

#endregion


#region Progress

[CustomEditor(typeof (BitHorizontalSegmentedProgressBar))]
public class BitHorizontalSegmentedProgressBarEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(160, 20);
	}
}


[CustomEditor(typeof (BitVerticalSegmentedProgressBar))]
public class BitVerticalSegmentedProgressBarEditor : BitControlEditor
{
	protected override void OnAddControl(BitControl control)
	{
		control.Size = new Size(20, 160);
	}
}

#endregion


#region Text

[CustomEditor(typeof (BitTipTextField))]
public class BitTipTextFieldEditor : BitControlEditor
{
}

#endregion


#region Resize

[CustomEditor(typeof(BitResizeHandler))]
public class BitResizeHandlerEditor : BitControlEditor
{
}

#endregion