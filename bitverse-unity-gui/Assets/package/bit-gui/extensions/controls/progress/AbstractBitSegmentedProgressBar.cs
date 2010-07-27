using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEngine;


public abstract class AbstractBitSegmentedProgressBar : AbstractBitProgressBar
{
	#region Appearance

	public override GUIStyle DefaultFillStyle
	{
		get { return null; }
	}

	public override GUIStyle DefaultLabelStyle
	{
		get { return GUI.skin.label; }
	}

	[SerializeField]
	private int _cellCount = 10;

	public int CellCount
	{
		get { return _cellCount; }
		set { _cellCount = value; }
	}

    [SerializeField]
    private int _cellOffset = 0;

    public int CellOffset
    {
        get { return _cellOffset; }
        set { _cellOffset = value; }
    }

	#endregion


	#region Behavior

	[SerializeField]
	private bool _onlyFullCells = true;

	public bool OnlyFullCells
	{
		get { return _onlyFullCells; }
		set { _onlyFullCells = value; }
	}

	#endregion


	#region Data

	[SerializeField]
	private SegmentedProgressBarCell _background = new SegmentedProgressBarCell("Segmented ProgressBar Background", 0);

	public SegmentedProgressBarCell Background
	{
		get { return _background; }
	}

	[SerializeField]
	private SegmentedProgressBarCell _mainCell = new SegmentedProgressBarCell("Segmented ProgressBar Main Cell", 0);

	public SegmentedProgressBarCell MainCell
	{
		get { return _mainCell; }
	}

	[SerializeField]
	private SegmentedProgressBarCell[] _segmentedProgressBarCells =
		{
			new SegmentedProgressBarCell("Segmented ProgressBar Cell 1", 0)
		};

	public SegmentedProgressBarCell[] SegmentedProgressBarCells
	{
		get { return _segmentedProgressBarCells; }
		set { _segmentedProgressBarCells = value; }
	}

	#endregion


	#region Draw

	protected override void DrawFill(GUIStyle progressStyle, GUIStyle fillStyle, float ratio)
	{
		//does nothing
	}

	protected override void DoDraw()
	{
		(Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn, false);

		if (_segmentedProgressBarCells == null)
		{
			return;
		}

		//The first layer is used as a background and fills all the number of cells, ignoring the value
		float cellWidth, cellHeight;
		DrawBackground(out cellWidth, out cellHeight);

		if (MinValue > MaxValue)
		{
			MinValue = MaxValue;
		}
		float range = MaxValue - MinValue;

		_mainCell.Value = Value;

	    int index = 0;
        Rect placement = ComputeFirstPlacement(cellWidth,cellHeight);
		DrawValueCells(ref placement, ref index, _mainCell, cellWidth, cellHeight, range);

		if (_segmentedProgressBarCells == null)
		{
			return;
		}


// 	    SegmentedProgressBarCell[] sortedSegmentedProgressBarCells = (SegmentedProgressBarCell[]) _segmentedProgressBarCells.Clone();
// 	    Array.Sort<SegmentedProgressBarCell>(sortedSegmentedProgressBarCells,
// 	                                         delegate(SegmentedProgressBarCell x, SegmentedProgressBarCell y)
//                                              { return (x.Value - y.Value <= 0) ? -1 : 1; });

        index = 0;
        placement = ComputeFirstPlacement(cellWidth, cellHeight);
//        foreach (SegmentedProgressBarCell cell in _segmentedProgressBarCells)
        for (int cellIndex = _segmentedProgressBarCells.Length; cellIndex > 0; cellIndex-- )
        {
            SegmentedProgressBarCell cell = _segmentedProgressBarCells[cellIndex-1];
            DrawValueCells(ref placement, ref index, cell, cellWidth, cellHeight, range);
        }
	}

    private Rect ComputeFirstPlacement(float cellWidth, float cellHeight)
    {
        return new Rect(Position.x, Position.yMax - cellHeight, cellWidth, cellHeight);
    }

	private void DrawBackground(out float cellWidth, out float cellHeight)
	{
		GUIStyle style = Skin.FindStyle(_background.StyleName);
		cellWidth = Position.width / CellCount;
		cellHeight = Position.height;
		if (style == null)
		{
			return;
		}

		Texture2D tex = style.normal.background;
		if (tex == null)
		{
			return;
		}
		cellWidth = style.fixedWidth == 0 ? cellWidth : style.fixedWidth;
		cellHeight = style.fixedHeight == 0 ? cellHeight : style.fixedHeight;
		Rect placement = new Rect(Position.x, Position.yMax - cellHeight, cellWidth, cellHeight);

		for (int i = 0; i < CellCount; i++)
		{
			GUI.DrawTexture(placement, tex, ScaleMode.StretchToFill);
			placement = CalcNextCellPlacement(placement, cellWidth, cellHeight);
		}
	}

	// TODO Optimize this avoiding overlapping
//    private void DrawValueCells(SegmentedProgressBarCell cell, float cellWidth, float cellHeight, float range)
    private void DrawValueCells(ref Rect placement, ref int index, SegmentedProgressBarCell cell, float cellWidth, float cellHeight, float range)
    {
		cell.Value = (int) Mathf.Clamp(cell.Value, MinValue, MaxValue);

		//Get the Style and draw entire cells
		GUIStyle style = Skin.FindStyle(cell.StyleName);
		if (style == null)
		{
			return;
		}

		Texture2D tex = style.normal.background;
		if (tex == null)
		{
			return;
		}
        
        //if (index == 0)
        //    placement = new Rect(Position.x, Position.yMax - cellHeight, cellWidth, cellHeight);

		int value = (int) (cell.Value - MinValue);
		float cellCount = (value / range) * CellCount;
		if (_onlyFullCells)
		{
			cellCount = Mathf.Round(cellCount);
		}
		for (; index < (int) cellCount; index++)
		{
			GUI.DrawTexture(placement, tex);
			placement = CalcNextCellPlacement(placement, cellWidth, cellHeight);
		}



		if (!_onlyFullCells)
		{
			DrawCellFraction(cellCount, cellWidth, cellHeight, placement, tex);
		}
	}

	protected abstract Rect CalcNextCellPlacement(Rect placement, float cellWidth, float cellHeight);

	protected abstract void DrawCellFraction(float cellCount, float cellWidth, float cellHeight, Rect placement, Texture2D tex);

	#endregion
}