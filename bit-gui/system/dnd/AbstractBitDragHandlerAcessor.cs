using UnityEngine;


public abstract class AbstractBitDragHandlerAcessor : IBitDragHandlerAcessor
{
	private BitDragManager _dragManager;

	public BitDragManager DragManager
	{
		get { return _dragManager; }
	}

	private IBitDragHandler _dragHandler;

	public IBitDragHandler DragHandler
	{
		get { return _dragHandler; }
		set { _dragHandler = value; }
	}

	public abstract bool CanDrop(IBitDragTransferable dragTransferable);

	public abstract void OnDrop(IBitDragTransferable dragTransferable, Vector2 mousePosition);

	public AbstractBitDragHandlerAcessor(BitDragManager dragManager)
	{
		_dragManager = dragManager;
	}

	public virtual void OnDropNowhere(IBitDragTransferable dragTransferable)
	{
		if (dragTransferable != null && dragTransferable.SourceDragHandler != null)
		{
			dragTransferable.SourceDragHandler.StopDrag(null);
		}
	}

	public virtual void DragEnd(IBitDragHandlerAcessor acessor)
	{
	}
}