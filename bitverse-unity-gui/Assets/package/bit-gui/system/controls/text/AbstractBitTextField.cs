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
                DoTextField(Position, ControlID, TempContent, IsMultiline(), MaxLenght, Style ?? DefaultStyle);
                Text = AcceptOnlyNumbers ? Regex.Match(TempContent.text, @"\d+").Value : TempContent.text;
            }
            else
            {
                if (Event.current.type == EventType.Repaint)
                    (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
            }
        }
        else
        {
            if (Event.current.type == EventType.Repaint)
                (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
        }
    }

    //TODO Please check if commented code is needed
    public static void DoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style)
    {
        if ((maxLength >= 0) && (content.text.Length > maxLength))
        {
            content.text = content.text.Substring(0, maxLength);
        }
        //GUIUtility.CheckOnGUI();
        TextEditor stateObject = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), id);
        stateObject.content.text = content.text;
        stateObject.SaveBackup();
        stateObject.position = position;
        stateObject.style = style;
        stateObject.multiline = multiline;
        stateObject.controlID = id;
        stateObject.ClampPos();
        Event current = Event.current;
        bool flag = false;
        switch (current.type)
        {
            case EventType.MouseDown:
                if (position.Contains(current.mousePosition))
                {
                    GUIUtility.hotControl = id;
                    GUIUtility.keyboardControl = id;
                    stateObject.MoveCursorToPosition(Event.current.mousePosition);
                    if ((Event.current.clickCount == 2) && GUI.skin.settings.doubleClickSelectsWord)
                    {
                        stateObject.SelectCurrentWord();
                        stateObject.DblClickSnap(TextEditor.DblClickSnapping.WORDS);
                        stateObject.MouseDragSelectsWholeWords(true);
                    }
                    if ((Event.current.clickCount == 3) && GUI.skin.settings.tripleClickSelectsLine)
                    {
                        stateObject.SelectCurrentParagraph();
                        stateObject.MouseDragSelectsWholeWords(true);
                        stateObject.DblClickSnap(TextEditor.DblClickSnapping.PARAGRAPHS);
                    }
                    current.Use();
                }
                goto Label_02E1;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == id)
                {
                    stateObject.MouseDragSelectsWholeWords(false);
                    GUIUtility.hotControl = 0;
                    current.Use();
                }
                goto Label_02E1;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl != id)
                {
                    goto Label_02E1;
                }
                if (!current.shift)
                {
                    stateObject.SelectToPosition(Event.current.mousePosition);
                    break;
                }
                stateObject.MoveCursorToPosition(Event.current.mousePosition);
                break;

            case EventType.KeyDown:
                if (GUIUtility.keyboardControl == id)
                {
                    if (stateObject.HandleKeyEvent(current))
                    {
                        current.Use();
                        flag = true;
                        content.text = stateObject.content.text;
                    }
                    else
                    {
                        if ((current.keyCode == KeyCode.Tab) || (current.character == '\t'))
                        {
                            return;
                        }
                        char character = current.character;
                        if (((character == '\n') && !multiline) && !current.alt)
                        {
                            return;
                        }
                        Font font = style.font;
                        if (font == null)
                        {
                            font = GUI.skin.font;
                        }
                        if (font.HasCharacter(character) || (character == '\n'))
                        {
                            stateObject.Insert(character);
                            flag = true;
                        }
                        else if (character == '\0')
                        {
                            /*if (GUIUtility.compositionString.Length > 0)
                            {
                                stateObject.ReplaceSelection(string.Empty);
                                flag = true;
                            }*/
                            current.Use();
                        }
                    }
                    goto Label_02E1;
                }
                return;

            case EventType.Repaint:
                if (GUIUtility.keyboardControl == id)
                {
                    stateObject.DrawCursor(content.text);
                }
                else
                {
                    style.Draw(position, content, id, false);
                }
                goto Label_02E1;

            default:
                goto Label_02E1;
        }
        current.Use();
    Label_02E1:
        if (GUIUtility.keyboardControl == id)
        {
            //GUIUtility.textFieldInput = true;
        }
        if (flag)
        {
            GUI.changed = true;
            content.text = stateObject.content.text;
            if ((maxLength >= 0) && (content.text.Length > maxLength))
            {
                content.text = content.text.Substring(0, maxLength);
            }
            current.Use();
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

    private void KeyboardEvent()
    {
        Event e = Event.current;


        if (e.isKey && TopWindow.IsFocused && GUIUtility.keyboardControl == ControlID)
        {
            //UFMAudioSource.Play(AudioConstants.TextChanged);
            if (e.type == EventType.KeyUp)
            {
                if (e.functionKey)
                    RaiseFunctionKeyPressedEvent(e.keyCode, e.character, e.alt, e.command, e.control, e.shift);
                else
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