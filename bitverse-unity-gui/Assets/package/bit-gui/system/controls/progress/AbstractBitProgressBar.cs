using UnityEngine;


public abstract class AbstractBitProgressBar : BitControl
{
	#region Appearance

	public abstract GUIStyle DefaultFillStyle { get; }

	[SerializeField]
	private string _fillStyleName;

	public string FillStyleName
	{
		get { return _fillStyleName; }
		set { _fillStyleName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private GUIStyle _fillStyle;

	public GUIStyle FillStyle
	{
		get
		{
			if (_fillStyle != null && !string.IsNullOrEmpty(_fillStyle.name))
			{
				return _fillStyle;
			}

			if (!string.IsNullOrEmpty(_fillStyleName))
			{
				GUISkin s = Skin;
				if (s != null)
				{
					return s.FindStyle(_fillStyleName);
				}
			}
			return null;
		}
		set { _fillStyle = value; }
	}

	public abstract GUIStyle DefaultLabelStyle { get; }

	[SerializeField]
	private string _labelStyleName;

	public string LabelStyleName
	{
		get { return _labelStyleName; }
		set { _labelStyleName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private GUIStyle _labelStyle;

	public GUIStyle LabelStyle
	{
		get
		{
			if (_labelStyle != null && !string.IsNullOrEmpty(_labelStyle.name))
			{
				return _labelStyle;
			}

			if (!string.IsNullOrEmpty(_labelStyleName))
			{
				GUISkin s = Skin;
				if (s != null)
				{
					return s.FindStyle(_labelStyleName);
				}
			}
			return null;
		}
		set { _labelStyle = value; }
	}

	#endregion


	#region Data

    [SerializeField]
    private bool _strechBar = true;

    public bool StrechBar
    {
        get { return _strechBar; }
        set { _strechBar = value; }
    }

    [SerializeField]
    private bool _inverseDirection;

    public bool InverseDirection
    {
        get { return _inverseDirection; }
        set { _inverseDirection = value; }
    }

	private float _ratio;

	public float Ratio
	{
		get { return _ratio; }
		set
		{
			_ratio = Mathf.Clamp01(value);
			_value = (_maxValue - _minValue) * _ratio;
		}
	}

	[SerializeField]
	private float _value;

	public virtual float Value
	{
		get { return _value; }
		set { _value = Mathf.Clamp(value, _minValue, _maxValue); }
	}

	[SerializeField]
	private float _minValue;

	public float MinValue
	{
		get { return _minValue; }
		set { _minValue = value; }
	}

	[SerializeField]
	private float _maxValue = 100f;

	public float MaxValue
	{
		get { return _maxValue; }
		set { _maxValue = value; }
	}

	[SerializeField]
	private bool _showText;

	public bool ShowText
	{
		get { return _showText; }
		set { _showText = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		if (_minValue >= _maxValue)
		{
			_minValue = _maxValue;
		}
		_value = Mathf.Clamp(_value, _minValue, _maxValue);

        if (_minValue >= _maxValue) _ratio = 1;
        else _ratio = _value / (_maxValue - _minValue);

		GUIStyle style = Style ?? DefaultStyle;

		style.Draw(Position, IsHover, IsActive, IsOn, false);

		if (_ratio > 0)
		{
		    DrawFill(style, FillStyle ?? DefaultFillStyle, _ratio);
		}

		if (_showText)
		{
			string text = string.IsNullOrEmpty(Content.text) ? _value.ToString() : Content.text;
			GUI.Label(Position, text, LabelStyle ?? DefaultLabelStyle);
		}
	}

    protected abstract void DrawFill(GUIStyle progressStyle, GUIStyle fillStyle, float ratio);

	#endregion
}