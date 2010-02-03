using Bitverse.Unity.Gui;
using UnityEngine;


public class BitPasswordField : BitControl
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

	// TODO expose this
	[SerializeField]
	private char _maskChar = '*';

	public string Text
	{
		get { return Content.text; }
		set
		{
			Content.text = value;
			RaiseValueChangedEvent(value);
		}
	}


	public char MaskChar
	{
		get { return _maskChar; }
		set { _maskChar = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		string t;

		t = MaxLenght < 0
		    	? GUI.PasswordField(Position, Text, MaskChar, Style ?? DefaultStyle)
		    	: GUI.PasswordField(Position, Text, MaskChar, MaxLenght, Style ?? DefaultStyle);

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