using Bitverse.Unity.Gui;
using UnityEngine;


public class BitContextMenuItem : BitButton, IBitContextMenuItem
{
	private BitContextMenu _parentContextMenu;

	public BitContextMenu ParentContextMenu
	{
		get
		{
			if (_parentContextMenu == null)
			{
				BitControl parent = Parent;
				if (!(parent is BitContextMenuOptions))
				{
					return null;
				}
				parent = parent.Parent;
				if (!(parent is BitContextMenu))
				{
					return null;
				}
				_parentContextMenu = (BitContextMenu) parent;
			}
			return _parentContextMenu;
		}
		set { _parentContextMenu = value; }
	}

	public bool HasSubMenu
	{
		get { return ContextMenu != null; }
	}


	#region Draw

	public bool ShowingSubmenu
	{
		get { return HasSubMenu && Equals(_parentContextMenu.ActiveSubmenu); }
	}

    protected override void DoAutoSize()
    {
        GUIStyle style = Style ?? DefaultStyle;

        Rect currentPosition = Position;

        float width, height;
        if (!style.wordWrap)
        {
            Vector2 size = style.CalcSize(Content);
            width = size.x;
            height = size.y;
        }
        else
        {
            width = currentPosition.width;
            height = style.CalcHeight(Content, Position.width);
        }

        if (ParentContextMenu != null)
        {
            GUIStyle s = ParentContextMenu.SubmenuIndicatorStyle ?? DefaultStyle;

            width += style.contentOffset.x + (HasSubMenu?s.fixedWidth:0.0f);
            height += style.contentOffset.y;
            height = Mathf.Max(height,s.fixedHeight);

            //Debug.Log(" triangulo " + s.fixedWidth + s.fixedHeight);
        }

        if (currentPosition.height == height && currentPosition.width == width)
        {
            return;
        }

        Location = new Point(
            ((Anchor & AnchorStyles.Right) == AnchorStyles.Right) ? currentPosition.x - (width - currentPosition.width) : currentPosition.x,
            ((Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom) ? currentPosition.y - (height - currentPosition.height) : currentPosition.y);
        Size = new Size(width, height);
    }


    protected override bool UserEventsBeforeDraw()
    {
        BitContextMenu menu = ParentContextMenu;
        GUIStyle menuStyle = menu.Style ?? menu.DefaultStyle;
        GUIStyle style = Style ?? menu.DefaultMenuItemStyle ?? DefaultStyle;
        //GUIStyle arrowStyle = menu.SubmenuIndicatorStyle ?? DefaultStyle;
        //float arrowWidth = (HasSubMenu && arrowStyle != null) ? arrowStyle.fixedWidth : 0.0f;

        Position = new Rect(0,
                            Position.y,
                            menu.Position.width - (menuStyle.padding.horizontal + style.margin.horizontal + menuStyle.contentOffset.x),
                            Position.height);

        return base.UserEventsBeforeDraw();
    }

	protected override void DoDraw()
	{
		BitContextMenu menu = ParentContextMenu;
		GUIStyle style = Style ?? menu.DefaultMenuItemStyle ?? DefaultStyle;
        GUIStyle arrowStyle = menu.SubmenuIndicatorStyle ?? DefaultStyle;

        if (Event.current.type == EventType.repaint) 
            style.Draw(Position, Content, IsHover || ShowingSubmenu, IsActive, IsOn, Focus);

		if (IsHover)
		{
			_parentContextMenu.ActiveSubmenu = this;
		}

		if (!HasSubMenu || EditMode)
		{
            // TODO fix submenu
			return;
		}

        if(arrowStyle != null)
		    arrowStyle.Draw(new Rect(Position.width - arrowStyle.fixedWidth - style.padding.right,
		                    Position.y + ((Position.height - arrowStyle.fixedHeight) / 2),
		                    arrowStyle.fixedWidth,
		                    arrowStyle.fixedHeight),
                   IsHover, IsActive, IsOn | ForceOnState, false);

		if (IsHover && !ContextMenu.Visible)
		{
            // Checks Position to spawn
            float x = AbsolutePosition.x + Position.width;
            float y = AbsolutePosition.y;

		    if (ParentContextMenu.IsOpenLeft)
		        x = AbsolutePosition.x - ContextMenu.Position.width;

            if (ContextMenu.Position.height + y > Screen.height)
                y = AbsolutePosition.y - ContextMenu.Position.height + Position.height;

            if (ContextMenu != null)
            {
                //Debug.Log(ContextMenu + ", " + new Point(x,y));
                ContextMenu.IsOpenLeft = ParentContextMenu.IsOpenLeft;
                ContextMenu.Show(new Point(x, y), this);
            }
		}
	}

	#endregion


	public override void Awake()
	{
		//Debug.Log("awakening me!");
		base.Awake();
		FixedHeight = true;
		FixedWidth = true;
		CanShowContextMenu = false;
	}
}