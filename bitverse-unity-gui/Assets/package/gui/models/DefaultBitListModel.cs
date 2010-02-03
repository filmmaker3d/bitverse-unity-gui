using System.Collections.Generic;


/// <summary>
/// Default implementation of <see cref="IBitListModel"/> with a list of strings.
/// </summary>
public class DefaultBitListModel : IBitListModel
{
	private readonly List<object> _data = new List<object>();

	public object this[int index]
	{
		get { return VerifyIndex(index) ? _data[index] : null; }
		set
		{
			if (!VerifyIndex(index))
			{
				return;
			}
			_data[index] = value;
		}
	}

	private bool VerifyIndex(int index)
	{
		return index >= 0 && index < _data.Count;
	}

	public int Count
	{
		get { return _data.Count; }
	}

	public void Add(object data)
	{
		_data.Add(data);
	}

	public void Remove(object data)
	{
		if (!_data.Contains(data))
		{
			return;
		}
		_data.Remove(data);
	}

	public void RemoveAt(int index)
	{
		if (!VerifyIndex(index))
		{
			return;
		}
		_data.RemoveAt(index);
	}

	public void Clear()
	{
		_data.Clear();
	}

	public int IndexOf(object item)
	{
		return item != null ? _data.IndexOf(item) : -1;
	}
}