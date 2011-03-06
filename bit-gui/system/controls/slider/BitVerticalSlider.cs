using Bitverse.Unity.Gui;
using UnityEngine;
using ValueType=Bitverse.Unity.Gui.ValueType;


public class BitVerticalSlider : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.verticalSlider; }
	}

	private GUIStyle _thumbStyle;

	public GUIStyle ThumbStyle
	{
		get { return _thumbStyle; }
		set { _thumbStyle = value; }
	}

	#endregion


	#region Behaviour

	[SerializeField]
	private float _max = 100;

	[SerializeField]
	private float _min;

	[SerializeField]
	private ValueType _valueType = ValueType.Float;

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
			RaiseValueChangedEvent(value);
		}
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		float val = ValueType == ValueType.Integer
		            	? GUI.VerticalSlider(Position, (int) Value, Min, Max, Style ?? DefaultStyle, _thumbStyle ?? GUI.skin.verticalSliderThumb)
		            	: GUI.VerticalSlider(Position, Value, Min, Max, Style ?? DefaultStyle, _thumbStyle ?? GUI.skin.verticalSliderThumb);

		if (val != Value)
		{
			Value = val;
		}
	}

	#endregion


	#region Events

	public event ValueChangedEventHandler ValueChanged;

	private void RaiseValueChangedEvent(float value)
	{
		if (ValueChanged == null)
		{
			return;
		}
		ValueChanged(this, new ValueChangedEventArgs(value));
	}

	#endregion
}