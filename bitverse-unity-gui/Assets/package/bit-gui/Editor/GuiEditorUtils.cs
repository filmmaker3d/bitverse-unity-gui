using System;
using UnityEditor;
using UnityEngine;


internal enum Corners
{
	None,
	Top,
	Left,
	Bottom,
	Right,
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight
}


internal class CornerInfo
{
	internal Corners Corner = Corners.None;
	internal BitControl Control;
}


internal static class GuiEditorUtils
{
	private const int Pix = 6;
	private const int Pixs = (Pix * 2);
	private const float DetectMargin = 5;

	internal static CornerInfo CurrentCorner(object[] comps)
	{
		CornerInfo info = new CornerInfo();

		foreach (BitControl comp in comps)
		{
			Rect normal = comp.AbsolutePosition;
			Rect abs = new Rect(normal.x - DetectMargin, normal.y - DetectMargin, normal.width + (DetectMargin * 2), normal.height + (DetectMargin * 2));
			//RectOffset margin = (comp.Style==null)?new RectOffset():comp.Style.margin;
			//Rect abs = new Rect(abspos.x-margin.left,abspos.y-margin.top,abspos.width+margin.horizontal,abspos.height+margin.vertical);
			Rect left = new Rect(abs.xMin, abs.yMin + Pix, Pix, abs.height - Pixs);
			Rect right = new Rect(abs.xMax - Pix, abs.yMin + Pix, Pix, abs.height - Pixs);
			Rect top = new Rect(abs.xMin + Pix, abs.yMin, abs.width - Pixs, Pix);
			Rect bottom = new Rect(abs.xMin + Pix, abs.yMax - Pix, abs.width - Pixs, Pix);
			Rect topleft = new Rect(abs.xMin, abs.yMin, Pix, Pix);
			Rect topright = new Rect(abs.xMax - Pix, abs.yMin, Pix, Pix);
			Rect bottomleft = new Rect(abs.xMin, abs.yMax - Pix, Pix, Pix);
			Rect bottomright = new Rect(abs.xMax - Pix, abs.yMax - Pix, Pix, Pix);

			if (info.Corner != Corners.None)
				break;

			if (CheckCornerAndChangeCursor(left, MouseCursor.ResizeHorizontal))
				info.Corner = Corners.Left;
			else if (CheckCornerAndChangeCursor(right, MouseCursor.ResizeHorizontal))
				info.Corner = Corners.Right;
			else if (CheckCornerAndChangeCursor(top, MouseCursor.ResizeVertical))
				info.Corner = Corners.Top;
			else if (CheckCornerAndChangeCursor(bottom, MouseCursor.ResizeVertical))
				info.Corner = Corners.Bottom;
			else if (CheckCornerAndChangeCursor(topleft, MouseCursor.ResizeUpLeft))
				info.Corner = Corners.TopLeft;
			else if (CheckCornerAndChangeCursor(topright, MouseCursor.ResizeUpRight))
				info.Corner = Corners.TopRight;
			else if (CheckCornerAndChangeCursor(bottomleft, MouseCursor.ResizeUpRight))
				info.Corner = Corners.BottomLeft;
			else if (CheckCornerAndChangeCursor(bottomright, MouseCursor.ResizeUpLeft))
				info.Corner = Corners.BottomRight;

		}
		return info;
	}


	private static bool CheckCornerAndChangeCursor(Rect rect, MouseCursor cursor)
	{
		if (rect.Contains(MousePosition))
		{
			//if (draw)
			//	DrawRect(Color.cyan, rect);
			EditorGUIUtility.AddCursorRect(ToGuiRect(rect), cursor);
			return true;
		}
		return false;
	}

	private static Rect ToGuiRect(Rect r)
	{
		Vector2 v = HandleUtility.WorldToGUIPoint(new Vector3(r.x, 0, r.y));
		Vector2 z = HandleUtility.WorldToGUIPoint(new Vector3(0, 0, 0));
		Vector2 s = HandleUtility.WorldToGUIPoint(new Vector3(r.width, 0, r.height));
		return new Rect(v.x, v.y, s.x - z.x, s.y - z.y);
	}

	public static Rect FromGuiRect(Rect r)
	{
		Vector2 v = HandleUtility.GUIPointToWorldRay(new Vector3(r.x, 0, r.y)).origin;
		Vector2 z = HandleUtility.GUIPointToWorldRay(new Vector3(0, 0, 0)).origin;
		Vector2 s = HandleUtility.GUIPointToWorldRay(new Vector3(r.width, 0, r.height)).origin;
		return new Rect(v.x, v.y, s.x - z.x, s.y - z.y);
	}

	public static void DrawRect(Rect rect, Color color)
	{
		Handles.color = color;
		Handles.DrawLine(new Vector3(rect.x, 0, rect.y), new Vector3(rect.xMax, 0, rect.y));
		Handles.DrawLine(new Vector3(rect.xMax, 0, rect.y), new Vector3(rect.xMax, 0, rect.y + rect.height));
		Handles.DrawLine(new Vector3(rect.xMax, 0, rect.yMax), new Vector3(rect.x, 0, rect.yMax));
		Handles.DrawLine(new Vector3(rect.x, 0, rect.yMax), new Vector3(rect.x, 0, rect.y));
	}
	private static readonly GUIStyle GlobalStyle = new GUIStyle();

	public static void DrawLabel(float posx, float posy, string text, Color color)
	{
		GlobalStyle.normal.textColor = color;
		try
		{
			Handles.Label(new Vector3(posx, 0, posy), text, GlobalStyle);
		} catch
		{
			
		}
	}

	public static void DrawControlRect(Rect rect, Color color, string text, Color textColor)
	{
		DrawRect(rect, color);
		DrawLabel(rect.x + 4, rect.y + 3, text, textColor);
	}

	private const int Sel1 = 3;
	private const int Sel2 = 6;

	public static void DrawSelected(Rect rect, Color color)
	{
		DrawRect(rect, color);
        DrawRect(new Rect(rect.x-Sel1-1,rect.y-Sel1-1,6,6),color);
        DrawRect(new Rect(rect.xMax - Sel1 + 1, rect.y - Sel1 - 1, 6, 6), color);
        DrawRect(new Rect(rect.x - Sel1 - 1, rect.yMax - Sel1 + 1, 6, 6), color);
        DrawRect(new Rect(rect.xMax - Sel1 + 1, rect.yMax - Sel1 + 1, 6, 6), color);
        //DrawSelectionRect(rect.x - 0.5f, rect.y - 0.5f, color);
        //DrawSelectionRect(rect.xMax - 0.5f, rect.y - 0.5f, color);
        //DrawSelectionRect(rect.x - 0.5f, rect.yMax - 0.5f, color);
        //DrawSelectionRect(rect.xMax - 0.5f, rect.yMax - 0.5f, color);

		float xmed = (rect.x + rect.xMax) / 2;
		float ymed = (rect.y + rect.yMax) / 2;
        DrawRect(new Rect(xmed, rect.y - Sel1 - 1, 6, 6), color);
        DrawRect(new Rect(xmed, rect.yMax - Sel1 + 1, 6, 6), color);
        DrawRect(new Rect(rect.x - Sel1 - 1, ymed, 6, 6), color);
        DrawRect(new Rect(rect.xMax - Sel1 + 1, ymed, 6, 6), color);
       // DrawSelectionRect(rect.x - 0.5f, ymed - 0.5f, color);
       // DrawSelectionRect(rect.xMax - 0.5f, ymed - 0.5f, color);
       // DrawSelectionRect(xmed - 0.5f, rect.y - 0.5f, color);
       // DrawSelectionRect(xmed - 0.5f, rect.yMax - 0.5f, color);
	}


	private const float HelperSize = 16;
	private const float HelperMargin = 10;

	public static void DrawMoveHelperRect(Rect position)
	{
		DrawRect(position, Color.red);
		DrawRect(new Rect(
			position.x + HelperMargin, 
			position.y - (HelperSize/2),
			HelperSize, HelperSize
			), Color.blue);
	}



	//private static Texture2D _selectionRect;

    //not working well at small zoom
    /*private static void DrawSelectionRect(float x, float y, Color color)
    {
        while (_selectionRect == null)
        {
            _selectionRect = new Texture2D(Sel2, Sel2, TextureFormat.ARGB32, false);
            for (int i = 0; i < _selectionRect.width; i++)
            {
                for (int j = 0; j < _selectionRect.height; j++)
                {
                    _selectionRect.SetPixel(i, j, color);
                }
            }
            _selectionRect.Apply();
        }
        Handles.Label(new Vector3(x, 0, y), _selectionRect);
    }

    public static void DrawRect2(Color color, Rect rect)
    {
        Handles.color = color;
        for (float t = rect.x; t < (rect.xMax - 5); t += 10)
            Handles.DrawLine(new Vector3(t, 0, rect.y), new Vector3(t + 5, 0, rect.y));
        for (float t = rect.y; t < (rect.y + rect.height - 5); t += 10)
            Handles.DrawLine(new Vector3(rect.xMax, 0, t), new Vector3(rect.xMax, 0, t + 5));
        for (float t = rect.x; t < (rect.xMax - 5); t += 10)
            Handles.DrawLine(new Vector3(t, 0, rect.yMax), new Vector3(t + 5, 0, rect.yMax));
        for (float t = rect.y; t < (rect.yMax - 5); t += 10)
            Handles.DrawLine(new Vector3(rect.x, 0, t), new Vector3(rect.x, 0, t + 5));
    }*/

    private static Vector2 _mousePosition;

	public static Vector2 MousePosition
	{
		get { return _mousePosition; }
	}

	public static void UpdateMousePosition()
	{
		Ray raio = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		_mousePosition = new Vector2(raio.origin.x, raio.origin.z);
	}


	public static BitControl GetCompUnderMouse(object[] comps)
	{
		BitControl best = null;
		float bestDist = float.MaxValue;
		Vector2 mousePosition = MousePosition;
		foreach (BitControl comp in comps)
		{
			if (comp.Unselectable)
			{
				continue;
			}
			Rect abs = comp.AbsolutePosition;
			Rect bigger = new Rect(abs.x - DetectMargin, abs.y - DetectMargin, abs.width + (DetectMargin * 2), abs.height + (DetectMargin * 2));
			if (bigger.Contains(mousePosition))
			{
				//Debug.Log("contains "+ comp.name);
				float mx = mousePosition.x;
				float my = mousePosition.y;
				float d = Math.Min(Math.Abs(bigger.xMin - mx), Math.Abs(bigger.xMax - mx));
				d = Math.Min(Math.Abs(bigger.yMin - my), d);
				d = Math.Min(Math.Abs(bigger.yMax - my), d);
				if (d < bestDist)
				{
					best = comp;
					bestDist = d;
				}
			}
		}
		return best;
	}

}