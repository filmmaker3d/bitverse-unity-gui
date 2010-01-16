using UnityEngine;
using Bitverse.Unity.Gui;


public class BitPasswordField : BitControl
{
	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "textfield"; }
	}

	#endregion

	#region Behaviour

    [SerializeField]
    private int _maxLenght = -1;
    
    public int MaxLenght
    {
        get { return _maxLenght; }
        set { _maxLenght = value; }
    }

	#endregion


	#region Data

    [SerializeField]
    private char _maskChar = '*';

    public string Text
    {
        get { return Content.text; }
        set
        {
            Content.text = value;
			if (TextChanged != null)
				TextChanged(this, new ValueChangedEventArgs(Text));
        }
    }



    public char MaskChar
    {
        get { return _maskChar; }
        set { _maskChar = value; }
    }

	#endregion


	#region Draw

    public override void DoDraw()
    {
        string t;

        if (Style != null)
        {
            if (MaxLenght == -1)
            {
                t = GUI.PasswordField(Position, Text, MaskChar, Style);
            }
            else
            {
                t = GUI.PasswordField(Position, Text, MaskChar, MaxLenght, Style);
            }
        }
        else
        {
            if (MaxLenght == -1)
            {
                t = GUI.PasswordField(Position, Text, MaskChar);
            }
            else
            {
                t = GUI.PasswordField(Position, Text, MaskChar, MaxLenght);
            }
        }

        if (Text != t)
        {
            Text = t;
        }
    }

	#endregion


	#region Events

	public event ValueChangedEventHandler TextChanged;

	#endregion
}
