using System.Text.RegularExpressions;
using Bitverse.Unity.Gui;
using UnityEngine;


public abstract class AbstractBitTextField : BitControl
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

    public bool AcceptOnlyNumbers;


    public int MaxLenght
    {
        get { return _maxLenght; }
        set { _maxLenght = value; }
    }

    #endregion


    #region Data

    public override string Text
    {
        get { return base.Text; }
        set
        {
            bool valueChanged = (Text != value);
            base.Text = value;
            if (valueChanged)
            {
                RaiseValueChangedEvent(value);
            }
        }
    }

    /// <summary>
    /// Sets the content text without raising value changed event.
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        Content.text = text ?? "";
    }

    protected abstract bool IsMultiline();

    #endregion


    #region Draw

    protected GUIContent TempContent = new GUIContent();

    protected override void DoDraw()
    {
        TempContent.text = Text;

        // hAcK
        // Windows are processed from front to back window for mouse events and from back to front window for repaint events
        // This causes confusions with ControlID
        if (TopWindow.IsFocused)
        {
            // It will only use DoTextField if we are editing the its text (To improve click area by texture background)
            if (ControlID == GUIUtility.keyboardControl)
            {
                GUI.DoTextField(Position, ControlID, TempContent, IsMultiline(), MaxLenght, Style ?? DefaultStyle);
                Text = AcceptOnlyNumbers ? Regex.Match(TempContent.text, @"\d+").Value : TempContent.text;
            }
            else
                (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
        }
        else
        {
            (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
        }
    }

    #endregion

    #region Events

    protected override void RaiseMouseDown(int mouseButton, Vector2 mousePosition)
    {
        // HaCk
        //GUI.FocusControl(_idToString);
        base.RaiseMouseDown(mouseButton, mousePosition);
    }

    protected override bool ConsumeEvent(EventType type)
    {
        return true;
    }

    protected override bool UserEventsBeforeDraw()
    {
        KeyboardEvent();
        return false;
    }

    public override FocusType FocusType
    {
        get { return FocusType.Keyboard; }
    }

    public override FocusType GetFocusType()
    {
        return FocusType; // TODO In future use this code with Tab Stop Check(TabStop) ? FocusType : FocusType.Passive;
    }

    /*protected override bool CheckFocusEvent()
    {
        return true;
    }*/

    private void KeyboardEvent()
    {
        Event e = Event.current;
        if (e.isKey && TopWindow.IsFocused && GUIUtility.keyboardControl == ControlID)
        {
            //UFMAudioSource.Play(AudioConstants.TextChanged);
            if (e.type == EventType.KeyUp && !e.functionKey)
            {
                RaiseKeyPressedEvent(e.keyCode, e.character, e.alt, e.command, e.control, e.shift);
                Event.current.Use();
            }
        }
    }

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