
public interface IBitDragHandler
{
    BitDragManager DragManager { get; }

    IBitDragProxy DragProxy { get; set; }

    IBitDragTransferable Transferable { get; }

    BitContainer ContainerControl { get; }

    void StartDrag(IBitDragTransferable transferable, BitControl draggedControl);

    void StopDrag(IBitDragHandlerAcessor targetAcessor);

    IBitDragHandlerAcessor Accessor { get; }
}
