using UnityEngine;
using Bitverse.Unity.Gui;


public class BitHorizontalScrollbar : BitControl
{
	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "horizontalscrollbar"; }
	}

	#endregion


	#region Behaviour
	
    [SerializeField]
    private float _visibleSize;
    
    [SerializeField]
    private ValueType _valueType = ValueType.Float;

    public float VisibleSize
    {
        get { return _visibleSize; }
        set { _visibleSize = value; }
    }

    public ValueType ValueType
    {
        get { return _valueType; }
        set { _valueType = value; }
    }

	#endregion


	#region Data

    [SerializeField]
    private float _value;
    
    [SerializeField]
    private float _right = 100;
    
    [SerializeField]
    private float _left;

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


    public float RightValue
    {
        get { return _right; }
        set { _right = value; }
    }

    public float LeftValue
    {
        get { return _left; }
        set { _left = value; }
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
				val = GUI.HorizontalScrollbar(Position, Value, VisibleSize, LeftValue, RightValue, Style);
            }
            else
            {
				val = GUI.HorizontalScrollbar(Position, Value, VisibleSize, LeftValue, RightValue);
            }
        }
        else
        {
            if (Style != null)
            {
				val = GUI.HorizontalScrollbar(Position, (int) Value, VisibleSize, LeftValue, RightValue, Style);
            }
            else
            {
				val = GUI.HorizontalScrollbar(Position, (int) Value, VisibleSize, LeftValue, RightValue);
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
