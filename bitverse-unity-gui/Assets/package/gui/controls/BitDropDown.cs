using Bitverse.Unity.Gui;
using UnityEngine;


public class BitDropDown : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get
		{
			GUISkin s = Skin;
			return s != null && s.styles != null ? (GUIStyle)s.styles["dropdown"] : base.DefaultStyle;
		}
	}

	#endregion


	#region Data

	[SerializeField]
	private BitPopup _options;

	public bool OptionsVisible
	{
		get { return _options != null ? _options.Visible : false; }
		set
		{
			if (_options == null)
			{
				return;
			}
			if (value)
			{
				Rect a = AbsolutePosition;
				_options.Size = new Size(Position.width, _options.Position.height);
				_options.Show(new Point(a.x, a.y + a.height), Skin);
				_options.SelectionChanged += OptionsSelectionChanged;
			}
			else
			{
				_options.Hide();
				_options.SelectionChanged -= OptionsSelectionChanged;
			}
		}
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		GUIStyle s = Style ?? DefaultStyle ?? EmptyStyle;
		bool optionsVisible = OptionsVisible;

		bool value = GUI.Toggle(Position, optionsVisible, Content, s);

		if (value != optionsVisible)
		{
			OptionsVisible = value;
		}
	}

	#endregion


	#region Events

	private void OptionsSelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		//TODO FILL RENDERER
		Content.text = e.Selection[0].ToString();
	}

	#endregion
}