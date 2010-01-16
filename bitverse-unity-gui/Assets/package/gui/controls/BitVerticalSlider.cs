using UnityEngine;
using Bitverse.Unity.Gui;


public class BitVerticalSlider : BitControl
{
    public event ValueChangedEventHandler ValueChanged;

    [SerializeField]
    private float _value = 0;
    [SerializeField]
    private ValueType _valueType = ValueType.Float;
    [SerializeField]
    private float _max = 100;
    [SerializeField]
    private float _min = 0;

    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            if (ValueChanged != null)
            {
                ValueChanged(this, new ValueChangedEventArgs(_value));
            }
        }
    }

    public ValueType ValueType
    {
        get { return _valueType; }
        set { _valueType = value; }
    }

    public float Max
    {
        get { return _max; }
        set { _max = value; }
    }

    public float Min
    {
        get { return _min; }
        set { _min = value; }
    }

    public override void DoDraw()
    {
        float val;

        if (ValueType == ValueType.Float)
        {
            if (Style != null)
            {
                val = GUI.VerticalSlider(Position, Value, Max, Min, Style, Style);
            }
            else
            {
                val = GUI.VerticalSlider(Position, Value, Max, Min);
            }
        }
        else
        {
            if (Style != null)
            {
                val = GUI.VerticalSlider(Position, (int)Value, Max, Min, Style, Style);
            }
            else
            {
                val = GUI.VerticalSlider(Position, (int)Value, Max, Min);
            }
        }

        if (val != Value)
        {
            Value = val;

        }


    }

}
