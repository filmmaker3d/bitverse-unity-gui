using UnityEngine;


public class BitVerticalSegmentedProgressBar : AbstractBitSegmentedProgressBar
{
	#region Draw

	protected override Rect CalcNextCellPlacement(Rect placement, float cellWidth, float cellHeight)
	{
		placement.y -= cellHeight;
		return placement;
	}

	protected override void DrawCellFraction(float cellCount, float cellWidth, float cellHeight, Rect placement, Texture2D tex)
	{
		float fragment = cellCount - (int) cellCount;
		GUI.DrawTexture(new Rect(
		                	placement.x,
		                	placement.y + (1 - fragment) * cellHeight,
		                	cellWidth,
		                	cellHeight * fragment),
		                tex, ScaleMode.StretchToFill);
	}

	#endregion
}