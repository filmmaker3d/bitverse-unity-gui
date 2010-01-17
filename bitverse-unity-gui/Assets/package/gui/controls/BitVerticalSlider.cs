using UnityEngine;
using Bitverse.Unity.Gui;


public class BitVerticalSlider : BitControl
{
	#region Appearance

    public override string DefaultStyleName
	{
		get { return "verticalslider"; }
	}

	#endregion


	#region Behaviour

	[SerializeField]
	private ValueType _valueType = ValueType.Float;

	[SerializeField]
	private float _max = 100;

	[SerializeField]
	private float _min;

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

	#endregion

	#region Data

	[SerializeField]
	private float _value;

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

	#endregion


	#region Draw

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
				val = GUI.VerticalSlider(Position, (int) Value, Max, Min, Style, Style);
			}
			else
			{
				val = GUI.VerticalSlider(Position, (int) Value, Max, Min);
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