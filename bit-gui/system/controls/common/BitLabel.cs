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
            //Rect source = Position;
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, false);
        }
    }

    #endregion

    public override void OnDrawGizmos()
    {
        OnDrawGizmos(SelectedInEditor ? Color.yellow : Color.magenta);
    }
}