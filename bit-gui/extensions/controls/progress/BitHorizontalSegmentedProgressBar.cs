using UnityEngine;


public class BitHorizontalSegmentedProgressBar : AbstractBitSegmentedProgressBar
{
	#region Draw

	protected override Rect CalcNextCellPlacement(Rect placement, float cellWidth, float cellHeight)
	{
        if (CellOffset > 0)
            cellWidth = CellOffset;
		placement.x += cellWidth;
		return placement;
	}

	protected override void DrawCellFraction(float cellCount, float cellWidth, float cellHeight, Rect placement, Texture2D tex)
	{
		GUI.DrawTexture(new Rect(
		                	placement.x,
		                	placement.y,
		                	cellWidth * (cellCount - (int) cellCount),
		                	cellHeight),
		                tex, ScaleMode.StretchToFill);
	}

	#endregion
}