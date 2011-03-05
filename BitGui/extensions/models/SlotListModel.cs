using System.Collections;


public class SlotListModel : ISlotListModel, IFilteredListModel, IEnumerable
{
    private readonly SortedList _data = new SortedList();

    //MANERA: We won't use the generic version of SortedList because we need to access 
    //it's values and keys in a bunch of times, and that causes the overhead of creating 
    //"SortedList<TKey, TValue>.Keys" and "SortedList<TKey, TValue>.Values" each time.
    //private readonly SortedList<int, object> _data = new SortedList<int, object>();

    private bool VerifyIndex(int index)
    {
        return index >= 0 && index < _data.Count;
    }

    public object this[int index]
    {
        get { return GetDataAtIndex(index); }
        set
        {
            if (VerifyIndex(index))
            {
                _data[_data.GetKey(index)] = value;
            }
        }
    }

    public int Count
    {
        get { return _data.Count; }
    }

    public void Add(object item)
    {
        Add(GetNextEmptySlot(), item);
    }

    public void Add(int slot, object item)
    {
        if (slot < 0 || item == null)
        {
            return;
        }
        if (!_data.ContainsKey(slot))
        {
            _data.Add(slot, item);
        }
        else
        {
            _data.Add(GetNextEmptySlot(), item);
        }
    }

    public void Remove(object item)
    {
        if (item != null && _data.ContainsValue(item))
        {
            _data.RemoveAt(_data.IndexOfValue(item));
        }
    }

    public void RemoveAt(int index)
    {
        if (VerifyIndex(index))
        {
            _data.RemoveAt(index);
        }
    }

    public bool Contains(object item)
    {
        return _data.ContainsValue(item);
    }

    public void Clear()
    {
        _data.Clear();
    }

    public int IndexOf(object item)
    {
        if (item != null)
            return _data.IndexOfValue(item);
        return -1;
    }

    //--

    public int GetNextEmptySlot()
    {
        int lastKey = GetLastSlot();
        for (int i = 0; i < lastKey; i++)
        {
            if (!_data.ContainsKey(i))
            {
                return i;
            }
        }

        return lastKey + 1;
    }

    public object GetDataAtIndex(int index)
    {
        if (Filter == null)
        {
            return _data[_data.GetKey(index)];
        }
        return VerifyIndex(index) && IsFilteredValidItem(_data[_data.GetKey(index)]) ? _data[_data.GetKey(index)] : null;
    }

    public object GetDataAtSlot(int slot)
    {
        return _data.ContainsKey(slot) ? _data[slot] : null;
    }

    public int GetSlotOf(object item)
    {
        if (item != null && _data.ContainsValue(item))
        {
            return (int) _data.GetKey(_data.IndexOfValue(item));
        }
        return -1;
    }

    public int GetSlotOfIndex(int index)
    {
        if (VerifyIndex(index))
        {
            return (int) _data.GetKey(index);
        }
        return -1;
    }

    public void SetSlotOf(object item, int slot)
    {
        if (slot < 0 || item == null || !_data.ContainsValue(item))
        {
            return;
        }
        int index = _data.IndexOfValue(item);
        _data.RemoveAt(index);
        _data.Add(slot, item);
    }

    public void RemoveAtSlot(int slot)
    {
        if (_data.ContainsKey(slot))
        {
            _data.Remove(slot);
        }
    }

    public int GetLastSlot()
    {
        return (int) (_data.Count > 0 ? _data.GetKey(_data.Count - 1) : -1);
    }

    public bool ContainsSlot(int slot)
    {
        return _data.ContainsKey(slot);
    }

    private object _filter;

    public object Filter
    {
        get { return _filter; }
        set { _filter = value; }
    }

    public virtual bool IsFilteredValidItem(object item)
    {
        return true;
    }

    public object GetDataIgnoringFilter(int index)
    {
        return VerifyIndex(index) ? _data[_data.GetKey(index)] : null;
    }


    #region Implementation of IEnumerable

    public IEnumerator GetEnumerator()
    {
        return _data.Values.GetEnumerator();
    }

    #endregion
}
