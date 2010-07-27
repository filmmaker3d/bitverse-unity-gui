using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Default implementation of <see cref="IListModel"/> with a list of strings.
/// </summary>
public class DefaultBitListModel : IListModel, IEnumerable
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

    public bool Contains(object item)
    {
        return _data.Contains(item);
    }

    public void Add(object item)
    {
        _data.Add(item);
    }

    public void Remove(object item)
    {
        if (!_data.Contains(item))
        {
            return;
        }
        _data.Remove(item);
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

    public void Insert(int index, object item)
    {
        _data.Insert(index, item);
    }

    #region Implementation of IEnumerable

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)_data).GetEnumerator();
    }

    #endregion
}