using Bitverse.Unity.Gui;
using UnityEditor;
using UnityEngine;


internal class ResizeHandler : AbstractHandler
{
	private GameObject[] _lastSelection;
	private Vector2 _lastPosition;

	public ResizeHandler(BitControlEditor editor)
	{
		_editor = editor;
	}

	public override void Execute()
	{
		//keep selection
		if (_lastSelection == null)
		{
			_lastSelection = Selection.gameObjects;
			_lastPosition = GuiEditorUtils.MousePosition;
		}
		else
		{
			Selection.objects = _lastSelection;
		}

		DrawLabels(_editor.ComponentList, Lightgray);

		//if (_editor.IsDrag)
		//{
		foreach (GameObject o in _lastSelection)
		{
			BitControl control = o.GetComponent<BitControl>();
            if (typeof(BitStage).IsAssignableFrom(control.GetType()))
                continue;
			Vector2 delta = GuiEditorUtils.MousePosition - _lastPosition;
			Rect abs = control.AbsolutePosition;

			ResizeCorner(control, Corners.Left, delta,
						 new Rect(abs.x + delta.x, abs.y, abs.width - delta.x, abs.height),
						 (abs.x + delta.x) < (abs.x + abs.width));
			ResizeCorner(control, Corners.Right, delta,
						 new Rect(abs.x, abs.y, abs.width + delta.x, abs.height),
						 abs.x < (abs.x + abs.width + delta.x));
			ResizeCorner(control, Corners.Top, delta,
						 new Rect(abs.x, abs.y + delta.y, abs.width, abs.height - delta.y),
						 (abs.y + delta.y) < (abs.y + abs.height));
			ResizeCorner(control, Corners.Bottom, delta,
						 new Rect(abs.x, abs.y, abs.width, abs.height + delta.y),
						 abs.y < (abs.y + abs.height + delta.y));
			ResizeCorner(control, Corners.TopLeft, delta,
						 new Rect(abs.x + delta.x, abs.y + delta.y, abs.width - delta.x, abs.height - delta.y),
						 (abs.y < (abs.y + abs.height + delta.y) && ((abs.x + delta.x) < (abs.x + abs.width))));
			ResizeCorner(control, Corners.TopRight, delta,
						 new Rect(abs.x, abs.y + delta.y, abs.width + delta.x, abs.height - delta.y),
						 (abs.x < (abs.x + abs.width + delta.x)) && ((abs.y + delta.y) < (abs.y + abs.height)));
			ResizeCorner(control, Corners.BottomLeft, delta,
						 new Rect(abs.x + delta.x, abs.y, abs.width - delta.x, abs.height + delta.y),
						 ((abs.x + delta.x) < (abs.x + abs.width)) && (abs.y < (abs.y + abs.height + delta.y)));
			ResizeCorner(control, Corners.BottomRight, delta,
						 new Rect(abs.x, abs.y, abs.width + delta.x, abs.height + delta.y),
						 (abs.x < (abs.x + abs.width + delta.x)) && (abs.y < (abs.y + abs.height + delta.y)));
			GuiEditorUtils.DrawControlRect(control.AbsolutePosition, Color.white, control.gameObject.name, Color.white);
			EditorUtility.SetDirty(o);
		}

		//}

		_lastPosition = GuiEditorUtils.MousePosition;
		//change mode
		if (_editor.IsMouseUp)
		{
			_editor.Mode = typeof(SelectHandler);
			_lastSelection = null;
		}
	}

	private static void ResizeCorner(BitControl control, Corners testcorner, Vector2 delta, Rect newrect,
									 bool condition)
	{
		//Rect abs = control.AbsolutePosition;
		if (SelectHandler.CornerInfo.Corner == testcorner)
		{
			if (condition)
			{
				control.AbsolutePosition = newrect;
				control.Size = new Size(newrect.width, newrect.height);
			}
		}
	}
}