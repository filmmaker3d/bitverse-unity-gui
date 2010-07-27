using Bitverse.Unity.Gui;
using UnityEngine;
using ValueType=Bitverse.Unity.Gui.ValueType;


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
            if (_value != value)
            {
                _value = value;
                RaiseValueChangedEvent(value);
                //UFMAudioSource.Play(AudioConstants.ScrollChanged);
            }
		}
	}

	public float Top
	{
		get { return _top; }
		set { _top = value; }
	}

	public float Bottom
	{
		get { return _botton; }
		set { _botton = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
        Rect rect = new Rect(Position.x,Position.y,Position.width,Position.height);
		float val = ValueType == ValueType.Float
                        ? GUI.VerticalScrollbar(rect, Value, VisibleSize, Top, Bottom, Style ?? DefaultStyle)
                        : GUI.VerticalScrollbar(rect, (int)Value, VisibleSize, Top, Bottom, Style ?? DefaultStyle);

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