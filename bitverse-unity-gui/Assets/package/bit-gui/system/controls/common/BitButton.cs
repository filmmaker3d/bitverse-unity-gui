using UnityEngine;


public class BitButton : BitControl
{
    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.button; }
    }

    #endregion


    #region Event

    protected override bool ConsumeEvent(EventType type)
    {
        return true;
    }

    #endregion

    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint)
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, Focus);
    }

    #endregion


    public override void OnDrawGizmos()
    {
         OnDrawGizmos(SelectedInEditor ? Color.yellow : Color.green);
    }

}