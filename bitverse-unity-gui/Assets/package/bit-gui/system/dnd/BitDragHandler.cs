
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitDragHandler : IBitDragHandler
{
    public BitDragHandler(BitContainer containerControl, IBitDragHandlerAcessor acessor, IBitDragProxy dragProxy)
    {
        _dragManager = acessor.DragManager;
        _dragProxy = dragProxy;
        _containerControl = containerControl;
        _acessor = acessor;
        _acessor.DragHandler = this;
        _containerControl.MouseUp += DisableDrag;
        _containerControl.Stage.MouseUp += StageMouseUpCallback;
        _containerControl.Stage.MouseMove += StageMouseMoveCallback;
        _maxCount++;
    }

    private void DisableDrag(object sender, MouseEventArgs e)
    {
        StopDrag(null);
    }

    public void Dispose()
    {
        _containerControl.MouseUp -= DisableDrag;
        _containerControl.Stage.MouseUp -= StageMouseUpCallback;
        _containerControl.Stage.MouseMove -= StageMouseMoveCallback; 
    }

    private IBitDragProxy _dragProxy;

    public BitContainer ContainerControl
    {
        get { return _containerControl; }
    }

    private readonly BitContainer _containerControl;

    private BitDragManager _dragManager;

    public BitDragManager DragManager
    {
        get { return _dragManager; }
        set { _dragManager = value; }
    }

    public IBitDragProxy DragProxy
    {
        get { return _dragProxy; }
        set { _dragProxy = value; }
    }

    private IBitDragTransferable _transferable;
    private IBitDragHandlerAcessor _acessor;

    public IBitDragTransferable Transferable
    {
        get { return _transferable; }
    }

    public void StartDrag(IBitDragTransferable transferable, BitControl draggedControl)
    {
        transferable.SourceDragHandler = this;
        _dragProxy.DraggedControl = draggedControl;
        _dragManager.StartDrag(_acessor);
        _transferable = transferable;
        _dragProxy.Visible = true;
    }

    public void StopDrag(IBitDragHandlerAcessor targetAcessor)
    {
        _dragManager.StopDrag();
        _dragProxy.Visible = false;
        _acessor.DragEnd(targetAcessor);
    }

    public IBitDragHandlerAcessor Accessor
    {
        get { return _acessor; }
    }

    private static int _count;
    private static int _maxCount;

    private void StageMouseUpCallback(object sender, MouseEventArgs e)
    {
        if (e.MouseButton != MouseButtons.Left)
            return;

        IBitDragHandlerAcessor sourceAcessor = _dragManager.CurrentAcessor;
        if (!_dragManager.IsDragging)
        {
            return;
        }

        //TODO Drop inside itself
        if (_containerControl.AbsolutePosition.Contains(e.MousePosition) && _containerControl.IsVisible())
        {
            _count = _maxCount;

            IBitDragHandlerAcessor targetAcessor = _acessor;
            if (sourceAcessor.DragHandler.DragProxy != null)
                if (sourceAcessor.DragHandler.DragProxy.Visible)
                {
                    if (targetAcessor.CanDrop(sourceAcessor.DragHandler.Transferable))
                    {
                        Rect abs = targetAcessor.DragHandler.ContainerControl.AbsolutePosition;
                        targetAcessor.OnDrop(sourceAcessor.DragHandler.Transferable,
                                             new Vector2(e.MousePosition.x - abs.x,
                                                         e.MousePosition.y - abs.y));
                    }
                    sourceAcessor.DragHandler.StopDrag(targetAcessor);
                }
            return;
        }

        _count--;

        if (_count == 0)
        {
            sourceAcessor.OnDropNowhere(sourceAcessor.DragHandler.Transferable);
            _count = _maxCount;
        }
    }

    private void StageMouseMoveCallback(object sender, Vector2 mousePosition)
    {
        if (_dragProxy == null || !_dragProxy.Visible)
        {
            return;
        }
        if (_dragManager != null)
        {
            _dragManager.OnDrag(mousePosition);
        }
    }
}