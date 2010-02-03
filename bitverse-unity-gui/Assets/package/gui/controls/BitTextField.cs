using Bitverse.Unity.Gui;
using UnityEngine;


public class BitTextField : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.textField; }
	}

	#endregion


	#region Behaviour

	[SerializeField]
	private int _maxLenght = -1;

	public int MaxLenght
	{
		get { return _maxLenght; }
		set { _maxLenght = value; }
	}

	#endregion


	#region Data

	public string Text
	{
		get { return Content.text; }
		set
		{
			Content.text = value;
			RaiseValueChangedEvent(value);
		}
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		string t = MaxLenght < 0
		           	? GUI.TextField(Position, Text, Style ?? DefaultStyle)
		           	: GUI.TextField(Position, Text, MaxLenght, Style ?? DefaultStyle);

		if (Text != t)
		{
			Text = t;
		}
	}

	#endregion


	#region Events

	public event ValueChangedEventHandler TextChanged;

	private void RaiseValueChangedEvent(string text)
	{
		if (TextChanged == null)
		{
			return;
		}
		TextChanged(this, new ValueChangedEventArgs(text));
	}

	#endregion
}