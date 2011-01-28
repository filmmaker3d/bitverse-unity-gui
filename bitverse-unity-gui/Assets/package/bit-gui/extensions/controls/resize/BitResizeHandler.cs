using Bitverse.Unity.Gui;
using UnityEngine;


public class BitResizeHandler : BitControl
{
    [HideInInspector]
    [SerializeField]
    private AnchorStyles _resizeAnchor = AnchorStyles.Invalid;

    public AnchorStyles ResizeAnchor
    {
        get { return _resizeAnchor; }
        set
        {
            _resizeAnchor = value;
        }
    }

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

    protected override void RaiseMouseDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        OnDrag(mouseButton, mousePosition, positionOffset);
        base.RaiseMouseDrag(mouseButton, mousePosition, positionOffset);
    }

    protected override void RaiseMouseStartDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        OnDrag(mouseButton, mousePosition, positionOffset);
        base.RaiseMouseStartDrag(mouseButton, mousePosition, positionOffset);
    }

    protected void OnDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        Debug.Log("Event " + Event.current.type + " OnDrag " + Event.current.button + " " + Event.current.mousePosition + " " + positionOffset);
        Rect p = Parent.Position;
        bool l = false;
        bool t = false;
        if ((ResizeAnchor & AnchorStyles.Left) == AnchorStyles.Left)
        {
            p.x += (int)positionOffset.x;
            p.width -= (int)positionOffset.x;
            l = true;
        }
        else if ((ResizeAnchor & AnchorStyles.Right) == AnchorStyles.Right)
        {
            p.width += (int)positionOffset.x;
        }

        else if ((ResizeAnchor & AnchorStyles.Top) == AnchorStyles.Top)
        {
            p.y += (int)positionOffset.y;
            p.height -= (int)positionOffset.y;
            t = true;
        }
        else if ((ResizeAnchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
        {
            p.height += (int)positionOffset.y;
        }
        Parent.Position = p;

        if (t || l)
        {
            MouseStatus ms = GetMouseStatus();
            MouseButtonStatus mbs;
            GetMouseButtonStatus(ms, out mbs);
            mbs.LastDragPosition.x = l ? (int)mbs.MouseDownPosition.x : (int)mbs.LastDragPosition.x;
            mbs.LastDragPosition.y = t ? (int)mbs.MouseDownPosition.y : (int)mbs.LastDragPosition.y;
            SetMouseButtonStatus(ref ms, mbs);
            SaveMouseStatus(ms);
        }
    }

    #endregion

    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint)
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, Focus);
    }

    #endregion
}