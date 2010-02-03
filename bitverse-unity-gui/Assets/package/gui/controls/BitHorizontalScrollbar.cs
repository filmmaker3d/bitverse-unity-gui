using Bitverse.Unity.Gui;
using UnityEngine;


public class BitHorizontalScrollbar : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.horizontalScrollbar; }
	}

	#endregion


	#region Behaviour

	[SerializeField]
	private ValueType _valueType = ValueType.Float;

	[SerializeField]
	private float _visibleSize;

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
	private float _left;

	[SerializeField]
	private float _right = 100;

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

	protected override void DoDraw()
	{
		float val = ValueType == ValueType.Float
						? GUI.HorizontalScrollbar(Position, Value, VisibleSize, LeftValue, RightValue, Style ?? DefaultStyle)
						: GUI.HorizontalScrollbar(Position, (int)Value, VisibleSize, LeftValue, RightValue, Style ?? DefaultStyle);

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
		if (ValueChanged != null)
		{
			ValueChanged(this, new ValueChangedEventArgs(value));
		}
	}

	#endregion
}