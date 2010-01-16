using UnityEngine;
using Bitverse.Unity.Gui;


public class BitToggle : BitControl
{
    public event ValueChangedEventHandler ValueChanged;

    [SerializeField]
    private bool _value = false;

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
            if (ValueChanged != null) ValueChanged(this, new ValueChangedEventArgs(_value));
        }
    }

    public override void DoDraw()
    {
        bool val;

        if (Style != null)
        {
            val = UnityEngine.GUI.Toggle(Position, Value, Content, Style);
        }
        else
        {
            val = UnityEngine.GUI.Toggle(Position, Value, Content);
        }

        if (val != Value)
        {
            Value = val;
        }
    }


}

