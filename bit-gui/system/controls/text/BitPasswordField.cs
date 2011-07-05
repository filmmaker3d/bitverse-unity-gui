using System;
using UnityEngine;


public class BitPasswordField : AbstractBitTextField
{
    #region Data

    // TODO expose this
    [SerializeField]
    private char _maskChar = '*';


    public char MaskChar
    {
        get { return _maskChar; }
        set { _maskChar = value; }
    }

    protected override bool IsMultiline()
    {
        return false;
    }

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        //Text = "INSERT_YOUR_PASSWORD_HERE"; //HARDCODED lol
        bool changed = GUI.changed;
        GUI.changed = false;
        if (Event.current.type == EventType.KeyDown)
        {
            if ((Event.current != null) && (((Event.current.keyCode == KeyCode.C) || (Event.current.keyCode == KeyCode.X)) && Event.current.control))
            {
                return;
            }

            if ((Event.current != null) && (Event.current.type == EventType.ValidateCommand) && (("Copy" == Event.current.commandName) || ("Cut" == Event.current.commandName)))
            {
                return;
            }

        }
        //base.DoDraw();

        //TempContent.text = GUI.PasswordFieldGetStrToShow(base.Text, _maskChar); ;
        TempContent.text = new string(_maskChar, base.Text.Length);

        // hAcK
        // Windows are processed from front to back window for mouse events and from back to front window for repaint events
        // This causes confusions with ControlID
        if (TopWindow.IsFocused)
        {
            // It will only use DoTextField if we are editing the its text (To improve click area by texture background)
            if (ControlID == GUIUtility.keyboardControl)
                DoTextField(Position, ControlID, TempContent, false, MaxLenght, Style ?? DefaultStyle);
            else
            {
                if (Event.current.type == EventType.Repaint)
                    (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
            }

            string text = !GUI.changed ? Text : TempContent.text;
            GUI.changed |= changed;

            Text = text;
        }
        else
        {
            if (Event.current.type == EventType.Repaint)
                (Style ?? DefaultStyle).Draw(Position, TempContent, IsHover, IsActive, IsOn | ForceOnState, false);
        }

    }

    #endregion
}