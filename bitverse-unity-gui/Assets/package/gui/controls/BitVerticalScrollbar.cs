using Bitverse.Unity.Gui;
using UnityEngine;


public class BitVerticalScrollbar : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.verticalScrollbar; }
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
	private float _botton;

	[SerializeField]
	private float _top = 100;

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

	protected override void DoDraw()
	{
		float val = ValueType == ValueType.Float
		            	? GUI.VerticalScrollbar(Position, Value, VisibleSize, Top, Botton, Style ?? DefaultStyle)
		            	: GUI.VerticalScrollbar(Position, (int) Value, VisibleSize, Top, Botton, Style ?? DefaultStyle);

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