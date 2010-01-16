using UnityEngine;
using Bitverse.Unity.Gui;


public class BitTextArea : BitControl
{
	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "textarea"; }
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
            if (MaxLenght < 0)
            {
                t = GUI.TextArea(Position, Text, Style);
            }
            else
            {
                t = GUI.TextArea(Position, Text, MaxLenght, Style);
            }
        }
        else
        {
            if (MaxLenght < 0)
            {
                t = GUI.TextArea(Position, Text);
            }
            else
            {
                t = GUI.TextArea(Position, Text, MaxLenght);
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
