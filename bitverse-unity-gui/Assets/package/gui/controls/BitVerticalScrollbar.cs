using UnityEngine;
using Bitverse.Unity.Gui;


public class BitVerticalScrollbar : BitControl
{
	#region Appearance

    public override string DefaultStyleName
	{
		get { return "verticalscrollbar"; }
	}

	#endregion


	#region Behaviour


    [SerializeField]
    private ValueType _valueType = ValueType.Float;
    
    [SerializeField]
    private float _visibleSize;


    public ValueType ValueType
    {
        get { return _valueType; }
        set { _valueType = value; }
    }

    public float VisibleSize
    {
        get { return _visibleSize; }
        set { _visibleSize = value; }
    }

	#endregion


	#region Data
	
    [SerializeField]
    private float _value;
    
    [SerializeField]
    private float _top = 100;
    
    [SerializeField]
    private float _botton;

    public float Value
    {
        get { return _value; }
        set
        {
            _value = value;
            if (ValueChanged != null)
            	ValueChanged(this, new ValueChangedEventArgs(_value));
        }
    }
    
    public float Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public float Botton
    {
        get { return _botton; }
        set { _botton = value; }
    }

	#endregion


	#region Draw

    public override void DoDraw()
    {
        float val;

        if (ValueType == ValueType.Float)
        {
            if (Style != null)
            {
				val = GUI.VerticalScrollbar(Position, Value, VisibleSize, Top, Botton, Style);
            }
            else
            {
				val = GUI.VerticalScrollbar(Position, Value, VisibleSize, Top, Botton);
            }
        }
        else
        {
            if (Style != null)
            {
				val = GUI.VerticalScrollbar(Position, (int) Value, VisibleSize, Top, Botton, Style);
            }
            else
            {
				val = GUI.VerticalScrollbar(Position, (int) Value, VisibleSize, Top, Botton);
            }
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

