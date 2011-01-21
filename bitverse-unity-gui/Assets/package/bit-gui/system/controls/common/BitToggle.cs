using Bitverse.Unity.Gui;
using UnityEngine;


public partial class BitToggle : BitControl
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

    public bool Value
    {
        get { return _value; }
        set
        {
            _value = value;
            RaiseValueChangedEvent(value);
        }
    }

    /// <summary>
    /// Change toogle value without calling event for changed
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(bool value)
    {
        _value = value;
    }

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint)
            (Style ?? DefaultStyle ?? EmptyStyle).Draw(Position, Content, IsHover, IsActive, IsOn || Value, false);
    }

    #endregion


    #region Events

    public event ValueChangedEventHandler ValueChanged;

    private void RaiseValueChangedEvent(bool value)
    {
        if (ValueChanged != null)
        {
            ValueChanged(this, new ValueChangedEventArgs(value));
        }
    }

    protected override void RaiseMouseClick(int mouseButton, Vector2 mousePosition)
    {
        if (!enabled)
        {
            return;
        }
        Value = !Value;
        if (Value)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.ToggleOn, AudioGuidToggleOn);
        else
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.ToggleOff, AudioGuidToggleOff);
        base.RaiseMouseClick(mouseButton, mousePosition);
    }

    protected override bool ConsumeEvent(EventType type)
    {
        return true;
    }

    /*protected override bool CheckFocusEvent()
    {
        return true;
    }*/

    #endregion
}