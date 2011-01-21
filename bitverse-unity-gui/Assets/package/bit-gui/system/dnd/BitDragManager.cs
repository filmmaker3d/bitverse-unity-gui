
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitDragManager
{

	private static Rect _dragWindowRect = new Rect(-100, -100, 0, 0);

	private BitStage _stage;

	public BitStage Stage
	{
		get { return _stage; }
	}

	private IBitDragHandlerAcessor _currentAcessor;

	public IBitDragHandlerAcessor CurrentAcessor
	{
		get { return _currentAcessor; }
		set { _currentAcessor = value; }
	}

	private bool _isDragging;

	public bool IsDragging
	{
		get { return _isDragging; }
		set { _isDragging = value; }
	}


	private BitWindow _draggedWindow;
	private BitControl _currentDraggedControl;

	public BitDragManager(BitStage stage)
	{
		_stage = stage;
		_draggedWindow = stage.FindControlInChildren<BitWindow>("_DragHelperWindow");
		if (_draggedWindow == null)
		{
            Debug.Log("Instantiating _DragHelperWindow");
			_draggedWindow = _stage.AddControl<BitWindow>("_DragHelperWindow");
			_draggedWindow.Draggable = false;
			_draggedWindow.Visible = false;
			_draggedWindow.Style = BitControl.EmptyStyle;
		}
		_draggedWindow.Position = _dragWindowRect;
		_draggedWindow.Visible = false;
		_draggedWindow.Unselectable = true;
	    _draggedWindow.FormMode = FormModes.Popup;
	}

	public void StartDrag(IBitDragHandlerAcessor acessor)
	{
		_currentAcessor = acessor;
		_isDragging = true;

		_currentDraggedControl = _currentAcessor.DragHandler.DragProxy.DraggedControl;
		_draggedWindow.Size = _currentDraggedControl.Size;
		_draggedWindow.AddControl(_currentDraggedControl);

		_currentDraggedControl.Location = new Point(0, 0);
		_draggedWindow.Visible = true;
	}

	public void StopDrag()
	{
		_isDragging = false;
		_draggedWindow.Visible = false;
		_draggedWindow.Position = _dragWindowRect;
		_draggedWindow.RemoveControl(_currentDraggedControl);
	}

	public void OnDrag(Vector2 position)
	{
		_draggedWindow.Location = new Point(position.x - _draggedWindow.Position.width/2, position.y - _draggedWindow.Position.height/2);
	}
}