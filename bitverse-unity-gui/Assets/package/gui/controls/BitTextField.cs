using UnityEngine;
using Bitverse.Unity.Gui;


public class BitTextField : BitControl
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

    public string Text
    {
        get { return Content.text; }
        set
        {
            Content.text = value;
			if (TextChanged != null)
				TextChanged(this, new ValueChangedEventArgs(Content.text));
        }
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
                t = GUI.TextField(Position, Text, Style);
            }
            else
            {
                t = GUI.TextField(Position, Text, MaxLenght, Style);
            }
        }
        else
        {
            if (MaxLenght == -1)
            {
                t = GUI.TextField(Position, Text);
            }
            else
            {
                t = GUI.TextField(Position, Text, MaxLenght);
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
