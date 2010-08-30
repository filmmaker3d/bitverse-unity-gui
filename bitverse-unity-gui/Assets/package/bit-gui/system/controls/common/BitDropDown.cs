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
			GUIStyle result = s != null ? s.FindStyle("drop down") : GUI.skin.button;
			return result ?? base.DefaultStyle;
		}
	}

	#endregion


	#region Data

	[SerializeField]
	private BitPopup _options;

    public BitPopup Options
    {
        get { return _options; }
        set { _options = value; }
    }

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
		if (Event.current.type == EventType.repaint)
			(Style ?? DefaultStyle ?? EmptyStyle).Draw(Position, Content, IsHover, IsActive, IsOn, false);
	}

	#endregion


	#region Events

	private void OptionsSelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		//TODO FILL RENDERER
		Content.text = e.Selection[0].ToString();
	}

    protected override void RaiseMouseClick(int mouseButton, Vector2 mousePosition)
    {
        OptionsVisible = !OptionsVisible;
        base.RaiseMouseClick(mouseButton, mousePosition);
    }

	#endregion
}