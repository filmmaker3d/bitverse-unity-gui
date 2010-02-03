using Bitverse.Unity.Gui;
using UnityEngine;


public class BitToggle : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.toggle; }
	}

	#endregion


	#region Data

	[SerializeField]
	private bool _value;

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
			RaiseValueChangedEvent(value);
		}
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		bool val = GUI.Toggle(Position, Value, Content, Style ?? DefaultStyle);

		if (val != Value)
		{
			Value = val;
		}
	}

	#endregion


	#region Events

	public event ValueChangedEventHandler ValueChanged;

	private void RaiseValueChangedEvent(bool value)
	{
		if (ValueChanged == null)
		{
			return;
		}
		ValueChanged(this, new ValueChangedEventArgs(value));
	}

	#endregion
}