using UnityEngine;
using Bitverse.Unity.Gui;


public class BitToggle : BitControl
{

	#region Data

    [SerializeField]
    private bool _value;

    public string Text
    {
        get { return Content.text; }
        set { Content.text = value; }
    }

    public Texture Image
    {
        get { return Content.image; }
        set { Content.image = value; }
    }

    public bool Value
    {
        get { return _value; }
        set
        {
            _value = value;
            if (ValueChanged != null)
				ValueChanged(this, new ValueChangedEventArgs(_value));
        }
    }
    
	#endregion


	#region Draw

    public override void DoDraw()
    {
        bool val;

        if (Style != null)
        {
            val = GUI.Toggle(Position, Value, Content, Style);
        }
        else
        {
            val = GUI.Toggle(Position, Value, Content);
        }

        if (val != Value)
        {
            Value = val;
        }
    }
    
	#endregion


	#region Events

    public event ValueChangedEventHandler ValueChanged;

	#endregion
}

