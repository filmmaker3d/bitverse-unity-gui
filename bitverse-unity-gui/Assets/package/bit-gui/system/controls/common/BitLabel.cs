using UnityEngine;


public class BitLabel : BitControl
{
    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.label; }
    }

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint)
        {
            Rect source = Position;
            Rect offsetPosition = new Rect(source.x, source.y + Stage.LabelYOffset, source.width, source.height);
            (Style ?? DefaultStyle).Draw(offsetPosition, Content, IsHover, IsActive, IsOn | ForceOnState, false);
        }
    }

    #endregion
}