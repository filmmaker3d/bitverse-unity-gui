using UnityEditor;
using UnityEngine;


internal sealed class MoveHandler : AbstractHandler
{
#pragma warning disable 414
    private bool _enableSnap;
	private bool _enableGrid;

	internal MoveHandler(BitControlEditor editor)
	{
		_editor = editor;
		Shortcuts.Add(new Shortcut(EventType.KeyDown, KeyCode.Alpha1, EnableSnapShortcut));
		Shortcuts.Add(new Shortcut(EventType.KeyUp, KeyCode.Alpha1, DisableSnapShortcut));
		Shortcuts.Add(new Shortcut(EventType.KeyDown, KeyCode.Alpha2, EnableGridShortcut));
		Shortcuts.Add(new Shortcut(EventType.KeyUp, KeyCode.Alpha2, DisableGridShortcut));
	}

	private void DisableGridShortcut(Shortcut s)
	{
		_enableGrid = false;
	}

	private void EnableGridShortcut(Shortcut s)
	{
		_enableGrid = true;
	}

	private void EnableSnapShortcut(Shortcut s)
	{
		_enableSnap = true;
	}

	private void DisableSnapShortcut(Shortcut s)
	{
		_enableSnap = false;
	}

	public override void OnDisable()
	{
		_enableSnap = false;
		_enableGrid = false;
	}

	private GameObject[] _lastSelection;
	private Vector2 _lastPosition;


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
        //    foreach (GameObject o in _lastSelection)
        //    {
        //        BitControl control = o.GetComponent<BitControl>();
        //        if (typeof(BitStage).IsAssignableFrom(control.GetType()))
        //            continue;
        //        Vector2 delta = GuiEditorUtils.MousePosition - _lastPosition;
        //        Rect abs = control.AbsolutePosition;
        //        if (_enableSnap)
        //        {
        //            delta = DoSnapping(_editor.ComponentList, control, delta);
        //        }
        //        if (_enableGrid)
        //        {
        //            delta = new Vector2(0, 0);
        //            Vector2 mousePos = GuiEditorUtils.MousePosition;
        //            abs = new Rect(((int)(mousePos.x / 10)) * 10, ((int)(mousePos.y / 10)) * 10, abs.width, abs.height);
        //        }
        //        control.AbsolutePosition = new Rect(abs.x + delta.x, abs.y + delta.y, abs.width, abs.height);

        //        GuiEditorUtils.DrawControlRect(abs, Color.white, control.gameObject.name, Color.white);
        //        EditorUtility.SetDirty(o);
        //    }
        //}

        //_lastPosition = GuiEditorUtils.MousePosition;
        ////change mode
        //if (_editor.IsMouseUp)
        //{
        //    _editor.Mode = typeof(SelectHandler);
        //    _lastSelection = null;
        //}
	}

	//private Vector2 DoGridSnap(BitControl control, Vector2 delta)
	//{
	//    return delta;
	//}
}