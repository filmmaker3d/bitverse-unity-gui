public class BitDragTransferable : IBitDragTransferable
{
	private IBitDragHandler _sourceDragHandler;
	private object[] _data;

	public BitDragTransferable(object[] data)
	{
		//_sourceDragHandler = sourceDragHandler;
		_data = data;
	}

	public IBitDragHandler SourceDragHandler
	{
		get { return _sourceDragHandler; }
		set { _sourceDragHandler = value; }
	}

	public object[] Data
	{
		get { return _data; }
		set { _data = value; }
	}
}