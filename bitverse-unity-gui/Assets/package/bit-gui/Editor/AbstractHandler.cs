using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


internal abstract class AbstractHandler : ModeHandler
{
	protected BitControlEditor _editor;
	private readonly List<Shortcut> _shortcuts = new List<Shortcut>();

	protected static readonly Color Lightgray = new Color(0.75f, 0.75f, 0.75f);

	public virtual void OnEnable()
	{
	}

	public virtual void OnDisable()
	{
	}

	public abstract void Execute();

	protected static void DrawLabels(Object[] components, Color color)
	{
		foreach (BitControl comp in components)
		{
			if (typeof (BitStage).IsAssignableFrom(comp.GetType()))
				continue;
			GuiEditorUtils.DrawLabel(comp.AbsolutePosition.x + 4, comp.AbsolutePosition.y + 3, comp.gameObject.name, color);
		}
	}

	public virtual List<Shortcut> Shortcuts
	{
		get { return _shortcuts; }
	}

	//SNAP STUFF... TOTAL MESS, but works ok.

	private const float Snap = 10;
	private float _accX;
	private float _accY;

	private bool _forgetFirstDeltaX;
	private bool _forgetFirstDeltaY;

	protected Vector2 DoSnapping(Object[] components, BitControl control, Vector2 delta)
	{
		CornerInfo cornerInfoX = GetClosestCornerInfoX(components, control);
		CornerInfo cornerInfoY = GetClosestCornerInfoY(components, control);
		bool anysnap = ((cornerInfoX.Control != null) || (cornerInfoY.Control != null));

		if (!anysnap)
		{
			return delta;
		}

		Rect controlrect = control.AbsolutePosition;

		if (cornerInfoX.Control != null)
		{
			_accX += delta.x;
		}
		if (cornerInfoY.Control != null)
		{
			_accY += delta.y;
		}

		if (cornerInfoX.Control != null)
		{
			Rect bestrect = cornerInfoX.Control.AbsolutePosition;
			switch (cornerInfoX.Corner)
			{
				case (Corners.Left):
					delta.x = bestrect.x - controlrect.x;
					DrawXLine(controlrect.x, controlrect.yMin, controlrect.yMax, bestrect.yMin, bestrect.yMax);
					break;
				case (Corners.Right):
					delta.x = bestrect.xMax - controlrect.xMax;
					DrawXLine(controlrect.xMax, controlrect.yMin, controlrect.yMax, bestrect.yMin, bestrect.yMax);
					break;
			}
		}

		if (cornerInfoY.Control != null)
		{
			Rect bestrect = cornerInfoY.Control.AbsolutePosition;
			switch (cornerInfoY.Corner)
			{
				case (Corners.Top):
					delta.y = bestrect.y - controlrect.y;
					DrawYLine(controlrect.y, controlrect.xMin, controlrect.xMax, bestrect.xMin, bestrect.xMax);
					break;
				case (Corners.Bottom):
					delta.y = bestrect.yMax - controlrect.yMax;
					DrawYLine(controlrect.yMax, controlrect.xMin, controlrect.xMax, bestrect.xMin, bestrect.xMax);
					break;
			}
		}

		if (cornerInfoX.Control != null)
		{
			if (!_forgetFirstDeltaX)
			{
				_forgetFirstDeltaX = true;
				_accX -= delta.x;
			}

			if (Math.Abs(_accX) > Snap)
			{
				delta.x = _accX;
				_accX = 0;
				_forgetFirstDeltaX = false;
			}
		}

		if (cornerInfoY.Control != null)
		{
			if (!_forgetFirstDeltaY)
			{
				_forgetFirstDeltaY = true;
				_accY -= delta.y;
			}

			if (Math.Abs(_accY) > Snap)
			{
				delta.y = _accY;
				_accY = 0;
				_forgetFirstDeltaY = false;
			}
		}
		return delta;
	}

	private static void DrawXLine(float x1, float ymin1, float ymax1, float ymin2, float ymax2)
	{
		Handles.color = Color.cyan;
		Vector3 line1P1 = new Vector3(x1, 0, Math.Min(ymin1, ymin2));
		Vector3 line1P2 = new Vector3(x1, 0, Math.Max(ymax1, ymax2));
		Handles.DrawLine(line1P1, line1P2);
	}

	private static void DrawYLine(float y1, float xmin1, float xmax1, float xmin2, float xmax2)
	{
		Handles.color = Color.cyan;
		Vector3 line1P1 = new Vector3(Math.Min(xmin1, xmin2), 0, y1);
		Vector3 line1P2 = new Vector3(Math.Max(xmax1, xmax2), 0, y1);
		Handles.DrawLine(line1P1, line1P2);
	}

	private static CornerInfo GetClosestCornerInfoX(IEnumerable<Object> components, BitControl control)
	{
		BitControl best = null;
		Corners corners = Corners.None;
		Rect abs = control.AbsolutePosition;
		float closestSnapX = float.MaxValue;
		foreach (BitControl o in components)
		{
			if (o == control)
				continue;
			BitControl ctr = o;
			Rect posrect = ctr.AbsolutePosition;
			float diff = Diff(abs.x, posrect.x);
			if (diff < closestSnapX)
			{
				best = o;
				closestSnapX = diff;
				corners = Corners.Left;
			}
			diff = Diff(abs.xMax, posrect.xMax);
			if (diff >= closestSnapX)
			{
				continue;
			}
			best = o;
			closestSnapX = diff;
			corners = Corners.Right;
		}
		CornerInfo result = new CornerInfo();
		if (closestSnapX < Snap)
		{
			result.Corner = corners;
			result.Control = best;
		}
		return result;
	}

	private static CornerInfo GetClosestCornerInfoY(IEnumerable<Object> components, BitControl control)
	{
		BitControl best = null;
		Corners corners = Corners.None;
		Rect abs = control.AbsolutePosition;
		float closestSnapY = float.MaxValue;
		foreach (BitControl o in components)
		{
			if (o == control)
				continue;
			BitControl ctr = o;
			Rect posrect = ctr.AbsolutePosition;
			float diff = Diff(abs.y, posrect.y);
			if (diff < closestSnapY)
			{
				best = o;
				closestSnapY = diff;
				corners = Corners.Top;
			}
			diff = Diff(abs.yMax, posrect.yMax);
			if (diff >= closestSnapY)
			{
				continue;
			}
			best = o;
			closestSnapY = diff;
			corners = Corners.Bottom;
		}
		CornerInfo result = new CornerInfo();
		if (closestSnapY < Snap)
		{
			result.Corner = corners;
			result.Control = best;
		}
		return result;
	}

	private static float Diff(float a, float b)
	{
		return Math.Max(a, b) - Math.Min(a, b);
	}
}