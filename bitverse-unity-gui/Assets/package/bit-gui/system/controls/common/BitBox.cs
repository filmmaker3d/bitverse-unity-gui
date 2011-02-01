using UnityEngine;


public class BitBox : BitControl
{
    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.box; }
    }

    #endregion

    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint)
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, false);
        //GUI.Box(Position, Content, Style ?? DefaultStyle);
    }

    #endregion


}