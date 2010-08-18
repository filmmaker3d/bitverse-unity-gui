using UnityEngine;


public class BitGroup : BitContainer
{
	#region Draw

	protected override void DoDraw()
	{
		if (Event.current.type == EventType.Repaint)
		{
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, false);
		}
		GUIClip.Push(Position);
		DrawChildren();
		GUIClip.Pop();
	}

	#endregion
}