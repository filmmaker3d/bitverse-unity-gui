using Bitverse.Unity.Gui;
using UnityEngine;


public class BitTipTextField : AbstractBitTextField
{
    #region Appearance

    [SerializeField]
    private string _tipTextStyleName = "TipTextField Tip";

    public string TipTextStyleName
    {
        get { return _tipTextStyleName; }
        set { _tipTextStyleName = value; }
    }

    [HideInInspector]
    [SerializeField]
    private GUIStyle _tipTextStyle;

    public GUIStyle TipTextStyle
    {
        get
        {
            if (_tipTextStyle != null && !string.IsNullOrEmpty(_tipTextStyle.name))
            {
                return _tipTextStyle;
            }

            if (!string.IsNullOrEmpty(_tipTextStyleName))
            {
                GUISkin s = Skin;
                if (s != null)
                {
                    return s.FindStyle(_tipTextStyleName);
                }
            }
            return null;
        }
        set { _tipTextStyle = value; }
    }

    public GUIStyle DrawStyle;

    #endregion


    #region Data

    protected override bool IsMultiline()
    {
        return false;
    }

    [SerializeField]
    private string _tipText = "Tip Text...";

    public string TipText
    {
        get { return _tipText; }
        set { _tipText = value; }
    }

    /*
    protected override void RaiseFocusGainEvent()
    {
        RemoveTipText();
        base.RaiseFocusGainEvent();
    }

	private void RemoveTipText()
	{
        //if (_tipText.Equals(Content.text))
        if (string.IsNullOrEmpty(Content.text))
        {
			Content.text = "";
			Style = DefaultStyle;
		}
	}

    protected override void RaiseFocusLostEvent()
    {
        InsertTipText();
        base.RaiseFocusLostEvent();
    }

	private void InsertTipText()
	{
		if (string.IsNullOrEmpty(Text))
		{
			Content.text = _tipText;
			Style = TipTextStyle;
		}
	}*/

    #endregion

    #region Draw

    protected override void DoDraw()
    {
        /*if (!_initialized)
        {
            _initialized = true;
            if (string.IsNullOrEmpty(_tipText))
            {
                _tipText = Content.text;
            }
            Content.text = _tipText;
            Style = TipTextStyle;
        }*/

        // hAcK
        // Windows are processed from front to back window for mouse events and from back to front window for repaint events
        // This causes confusions with ControlID
        if (TopWindow.IsFocused)
        {
            //ControlID = GUIUtility.GetControlID(GetFocusType());

            if (ControlID == GUIUtility.keyboardControl)
            {
                // Focused
                TempContent.text = Content.text;
                DrawStyle = Style ?? DefaultStyle;
                DoTextField(Position, ControlID, TempContent, IsMultiline(), MaxLenght, DrawStyle);
                Text = TempContent.text;
            }
            else
            {
                // Not Focused
                bool useTip = string.IsNullOrEmpty(Content.text);
                TempContent.text = useTip ? _tipText : Content.text;
                DrawStyle = (useTip ? TipTextStyle : Style) ?? DefaultStyle;
                if (Event.current.type == EventType.Repaint)
                    DrawStyle.Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
                //GUIDoTextField(Position, ControlID, TempContent, IsMultiline(), MaxLenght, DrawStyle);
            }
        }
        else
        {
            // Not Focused
            bool useTip = string.IsNullOrEmpty(Content.text);
            TempContent.text = useTip ? _tipText : Content.text;
            DrawStyle = (useTip ? TipTextStyle : Style) ?? DefaultStyle;
            if (Event.current.type == EventType.Repaint)
                DrawStyle.Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
        }

    }

    #endregion
}