using UnityEngine;


public class BitPasswordField : AbstractBitTextField
{
	#region Data

	// TODO expose this
	[SerializeField]
	private char _maskChar = '*';


	public char MaskChar
	{
		get { return _maskChar; }
		set { _maskChar = value; }
	}

	protected override bool IsMultiline()
	{
		return false;
	}

	#endregion


	#region Draw

	protected override void DoDraw()
    {
        bool changed = GUI.changed;
        GUI.changed = false;

        //base.DoDraw();

        TempContent.text = GUI.PasswordFieldGetStrToShow(base.Text, _maskChar); ;

        // hAcK
        // Windows are processed from front to back window for mouse events and from back to front window for repaint events
        // This causes confusions with ControlID
        if (TopWindow.IsFocused)
        {
            // It will only use DoTextField if we are editing the its text (To improve click area by texture background)
            //if (ControlID == GUIUtility.keyboardControl)
                GUIDoTextField(Position, ControlID, TempContent, false, MaxLenght, Style ?? DefaultStyle);
            //else if (Event.current.type == EventType.repaint)
            //    (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn, false);

            string text = !GUI.changed ? Text : TempContent.text;
            GUI.changed |= changed;

            Text = text;
        }
        else if (Event.current.type == EventType.repaint)
        {
            (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn, false);
        }

	}

	#endregion
}