using System;


public class BitItemDragProxy : IBitDragProxy
{
	private BitControl _draggedControl;
	private bool _visible;

	public BitControl DraggedControl
	{
		get { return _draggedControl; }
		set { _draggedControl = value; }
	}

	public bool Visible
	{
		get { return _visible; }
		set
		{
			_visible = value;
			if (_visible)
			{
				if (DraggedControl == null)
				{
					throw new Exception("Sorry... trying to drag something without setting draggedControl");
				}
				DraggedControl.Enabled = true;
				DraggedControl.Visible = true;
			}
			else
			{
				if (DraggedControl != null)
				{
					DraggedControl.Enabled = false;
					DraggedControl.Visible = false;
				}
			}
		}
	}


	public BitItemDragProxy()
	{
	}

	public BitItemDragProxy(BitControl draggedControl)
	{
		_draggedControl = draggedControl;
	}
}