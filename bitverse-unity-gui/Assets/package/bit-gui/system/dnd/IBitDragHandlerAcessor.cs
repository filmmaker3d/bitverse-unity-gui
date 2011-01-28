using UnityEngine;


public interface IBitDragHandlerAcessor
{
	BitDragManager DragManager { get; }

	IBitDragHandler DragHandler { get; set; }

	bool CanDrop(IBitDragTransferable dragTransferable);

	void OnDrop(IBitDragTransferable dragTransferable, Vector2 mousePosition);

	void OnDropNowhere(IBitDragTransferable dragTransferable);

	void DragEnd(IBitDragHandlerAcessor targetAcessor);
}