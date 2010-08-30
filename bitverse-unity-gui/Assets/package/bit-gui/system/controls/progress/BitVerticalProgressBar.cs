using UnityEngine;


public class BitVerticalProgressBar : AbstractBitProgressBar
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.FindStyle("vertical progress") ?? GUI.skin.box; }
	}

	public override GUIStyle DefaultFillStyle
	{
		get { return GUI.skin.FindStyle("vertical progress fill") ?? GUI.skin.box; }
	}

	public override GUIStyle DefaultLabelStyle
	{
		get { return GUI.skin.FindStyle("vertical progress label") ?? GUI.skin.label; }
	}

	#endregion


	#region Draw

    protected override void DrawFill(GUIStyle progressStyle, GUIStyle fillStyle, float ratio)
    {
        float totalHeight = (Position.height - progressStyle.padding.vertical - fillStyle.margin.vertical);
        float totalWidth = (Position.width - progressStyle.padding.horizontal - fillStyle.margin.horizontal);
        float height = totalHeight * ratio;
        Rect fillPoss = new Rect(
            Position.x + progressStyle.padding.left + fillStyle.margin.left,
            Position.yMax - height - progressStyle.padding.bottom + fillStyle.margin.bottom,
            totalWidth,
            height
            );
        
        if (InverseDirection)
            fillPoss.y = fillPoss.y + height - totalHeight;

        if (StrechBar)
        {
            fillStyle.Draw(fillPoss, IsHover, IsActive, IsOn | ForceOnState, false);
        }
        else
        {
            GUIClipPush(fillPoss);

            Rect completePos = new Rect(0, InverseDirection?0:(height - totalHeight), totalWidth, totalHeight);
            fillStyle.Draw(completePos, IsHover, IsActive, IsOn | ForceOnState, false);

            GUIClipPop();
        }
    }

	#endregion
}