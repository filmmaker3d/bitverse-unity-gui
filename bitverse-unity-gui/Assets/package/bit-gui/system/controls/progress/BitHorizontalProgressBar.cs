using UnityEngine;


public class BitHorizontalProgressBar : AbstractBitProgressBar
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.FindStyle("horizontal progress") ?? GUI.skin.box; }
	}

	public override GUIStyle DefaultFillStyle
	{
		get { return GUI.skin.FindStyle("horizontal progress fill") ?? GUI.skin.box; }
	}

	public override GUIStyle DefaultLabelStyle
	{
		get { return GUI.skin.FindStyle("horizontal progress label") ?? GUI.skin.label; }
	}

	#endregion


	#region Draw

	protected override void DrawFill(GUIStyle progressStyle, GUIStyle fillStyle, float ratio)
    {
        float totalHeight = Position.height - progressStyle.padding.vertical - fillStyle.margin.vertical;
        float totalWidth = Position.width - progressStyle.padding.horizontal - fillStyle.margin.horizontal;
	    float width = Mathf.Ceil(totalWidth * ratio);
		Rect fillPoss = new Rect(
			Position.x + progressStyle.padding.left + fillStyle.margin.left,
			Position.y + progressStyle.padding.top + fillStyle.margin.top,
            width,
			totalHeight
			);

        if (InverseDirection)
            fillPoss.x = fillPoss.x - width + totalWidth;

        if (StrechBar)
        {
            if (Event.current.type == EventType.Repaint)
                fillStyle.Draw(fillPoss, IsHover, IsActive, IsOn | ForceOnState, false);
        }
        else
        {
            GUIClipPush(fillPoss);

            Rect completePos = new Rect(InverseDirection ? (width - totalWidth) : 0, 0, totalWidth, totalHeight);

            if (Event.current.type == EventType.Repaint)
                fillStyle.Draw(completePos, IsHover, IsActive, IsOn | ForceOnState, false);

            GUIClipPop();
        }
	}

	#endregion
}