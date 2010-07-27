using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


internal class SelectHandler : AbstractHandler
{
	private GameObject[] _selectedItems;

	public GameObject[] SelectedItems
	{
		get { return _selectedItems; }
		set { _selectedItems = value; }
	}

	public SelectHandler(BitControlEditor editor)
	{
		_editor = editor;
	}

	public static CornerInfo CornerInfo = new CornerInfo();
	private bool _processed;

	public override void Execute()
	{
		if (_selectedItems == null)
			_selectedItems = Selection.gameObjects;
		else
			Selection.objects = _selectedItems;

		DrawLabels(_editor.ComponentList, Lightgray);

		if ((!_processed) && (_editor.IsMouseDown))
		{
			_processed = true;
			CornerInfo = GuiEditorUtils.CurrentCorner(_editor.ComponentList);
			CheckComponentClicked();
		}
		if (_editor.IsMouseUp)
		{
			_processed = false;
		}
		GuiEditorUtils.CurrentCorner(_editor.ComponentList);
		if (_editor.IsDrag)
		{
			_editor.Mode = CornerInfo.Corner == Corners.None ? typeof(MoveHandler) : typeof(ResizeHandler);
			return;
		}

		if (_selectedItems != null)
			DrawSelectedItems();

		//DrawHilite();
	}

	private void CheckComponentClicked()
	{
		BitControl controlUnderMouse = GuiEditorUtils.GetCompUnderMouse(_editor.ComponentList);
		if ((controlUnderMouse is BitContainer) && (BitGuiEditorToolbox.ControlTypeToCreate != null))
		{
			//ADDING COMPONENTS
			BitControl c = BitGuiEditorToolbox.CreateComponent((BitContainer)controlUnderMouse, GuiEditorUtils.MousePosition);
			BitControlEditor.AddControl(c);
			BitGuiEditorToolbox.ControlTypeToCreate = null;
		}
		else
		{
			//SELECTING COMPONENTS
			//TODO multiple select
			if (controlUnderMouse != null)
			{
				Selection.activeGameObject = controlUnderMouse.gameObject;
				_selectedItems = Selection.gameObjects;
			}
			else
			{
				BitStage stage = (BitStage)Object.FindObjectOfType(typeof(BitStage));
				if (stage != null)
				{
					Selection.activeGameObject = stage.gameObject;
				}
			}
		}
	}

	private void DrawSelectedItems()
	{
		Rect selectionPosition = new Rect(float.MaxValue, float.MaxValue, 0, 0);
		foreach (GameObject o in _selectedItems)
		{
			BitControl control = o.GetComponent<BitControl>();
            if (control == null)
                continue;
            if (typeof(BitStage).IsAssignableFrom(control.GetType()))
                continue;
			Rect abs = control.AbsolutePosition;
			GuiEditorUtils.DrawSelected(abs, Color.white);
			GuiEditorUtils.DrawLabel(abs.x + 4, abs.y + 3, control.gameObject.name, Color.white);
			EditorUtility.SetDirty(control);

			selectionPosition.x = Mathf.Min(selectionPosition.x, abs.x);
			selectionPosition.y = Mathf.Min(selectionPosition.y, abs.y);
			selectionPosition.width = Mathf.Max(selectionPosition.width, abs.width);
			selectionPosition.height = Mathf.Max(selectionPosition.height, abs.height);
		}

		//GuiEditorUtils.DrawMoveHelperRect(selectionPosition);
	}

	/*private void DrawHilite()
	{
		BitControl controlUnderMouse = GuiEditorUtils.GetCompUnderMouse(_editor.ComponentList);
		if (controlUnderMouse != null)
		{
			Rect abs = controlUnderMouse.AbsolutePosition;
			GuiEditorUtils.DrawRect2(Color.black, abs);
			//GuiEditorUtils.DrawLabel(abs.x, abs.y, controlUnderMouse.gameObject.name,Color.white);
			EditorUtility.SetDirty(controlUnderMouse);
		}
	}*/
}