using UnityEngine;


public class BitTab : BitGroup
{
	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.FindStyle("tab content") ?? GUI.skin.box; }
	}

	//public override Rect Position
	//{
	//    internal set
	//    {
	//        //HACK Yeah, I know, this is really ugly

	//        BitControl parent = Parent;
	//        if (parent is BitTabbedPane)
	//        {
	//            base.Position = GetPosition((BitTabbedPane) parent, Style ?? DefaultStyle);
	//        }
	//        else
	//        {
	//            base.Position = value;
	//        }
	//    }
	//}

	//public override Rect AbsolutePosition
	//{
	//    get
	//    {
	//        BitControl p = Parent;
	//        if (p != null)
	//        {
	//            Rect ab = p.AbsolutePosition;
	//            return new Rect(ab.x + Position.x, ab.y + Position.y, Position.width, Position.height);
	//        }
	//        return Position;
	//    }
	//    set
	//    {
	//        //HACK Yeah, I know, this is really ugly

	//        BitControl parent = Parent;
	//        if (parent is BitTabbedPane)
	//        {
	//            base.AbsolutePosition = GetPosition((BitTabbedPane) parent, Style ?? DefaultStyle);
	//        }
	//        else
	//        {
	//            base.AbsolutePosition = value;
	//        }
	//    }
	//}

	protected override void DoDraw()
	{
		if (Event.current.type == EventType.Repaint)
		{
			(Style ?? DefaultStyle).Draw(Position, IsHover, IsActive, IsOn, false);
		}
		GUIClipPush(Position);
		DrawChildren();
		GUIClipPop();
	}




	//public override void Awake()
	//{
	//    BitControl parent = Parent;
	//    if (parent == null)
	//    {
	//        return;
	//    }

	//    if (!typeof(BitTabbedPane).IsAssignableFrom(parent.GetType()))
	//    {
	//        if (!EditMode)
	//        {
	//            Debug.LogError("BitTab must be inside a BitTabbedPane.");
	//        }
	//        ((BitContainer)parent).RemoveControl(this);
	//        return;
	//    }

	//    base.Awake();
	//}

	//public override void OnDrawGizmos()
	//{
	//    //Unselectable = !Visible;
	//    Position = new Rect();
	//    base.OnDrawGizmos();
	//}
}