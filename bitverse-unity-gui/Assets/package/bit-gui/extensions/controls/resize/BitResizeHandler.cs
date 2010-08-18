using Bitverse.Unity.Gui;
using UnityEngine;


public class BitResizeHandler : BitControl
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
        //Debug.Log("Event " + Event.current.type + " OnDrag " + Event.current.button + " " + Event.current.mousePosition + " " + positionOffset);
        Rect p = Parent.Position;
        bool l = false;
        bool t = false;
        if ((Anchor & AnchorStyles.Left) == AnchorStyles.Left)
        {
            p.x += positionOffset.x;
            p.width -= positionOffset.x;
            l = true;
        }
        else if ((Anchor & AnchorStyles.Right) == AnchorStyles.Right)
        {
            p.width += positionOffset.x;
        }

        if ((Anchor & AnchorStyles.Top) == AnchorStyles.Top)
        {
            p.y += positionOffset.y;
            p.height -= positionOffset.y;
            t = true;
        }
        else if ((Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
        {
            p.height += positionOffset.y;
        }
        Parent.Position = p;

        if (t || l)
        {
            MouseStatus ms = GetMouseStatus();
            MouseButtonStatus mbs;
            GetMouseButtonStatus(ms, out mbs);
            mbs.LastDragPosition.x = l ? mbs.MouseDownPosition.x : mbs.LastDragPosition.x;
            mbs.LastDragPosition.y = t ? mbs.MouseDownPosition.y : mbs.LastDragPosition.y;
            SetMouseButtonStatus(ref ms, mbs);
            SaveMouseStatus(ms);
        }
    }

    #endregion
    
    #region Draw

    protected override void DoDraw()
    {
        (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, Focus);
	}

	#endregion
}