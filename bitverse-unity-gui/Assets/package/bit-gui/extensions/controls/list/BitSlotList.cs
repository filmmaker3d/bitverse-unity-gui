using System;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitSlotList : AbstractBitList<ISlotListModel, IPopulator>
{
    #region Appearance

    [SerializeField]
    private Vector2 _slotSize = new Vector2(32, 32);

    public Vector2 SlotSize
    {
        get { return _slotSize; }
        set { _slotSize = value; }
    }

    [SerializeField]
    private string _slotStyleName;

    public string SlotStyleName
    {
        get { return _slotStyleName; }
        set { _slotStyleName = value; }
    }

    [HideInInspector]
    [SerializeField]
    private GUIStyle _slotStyle;

    public GUIStyle SlotStyle
    {
        get
        {
            if (_slotStyle != null && !string.IsNullOrEmpty(_slotStyle.name))
            {
                return _slotStyle;
            }

            if (!string.IsNullOrEmpty(_slotStyleName))
            {
                GUISkin s = Skin;
                if (s != null)
                {
                    return s.FindStyle(_slotStyleName);
                }
            }
            return null;
        }
        set { _slotStyle = value; }
    }

    public GUIStyle DefaultSlotStyle
    {
        get { return GUI.skin.box; }
    }

    public bool LimitRows;

    public bool LimitColumns;

    public Vector2 MaximumSize = new Vector2(10, 10);

    #endregion


    #region Behaviour

    public bool NoExtraRow;

    private int _initialPosx;
    private int _initialPosy;
    private float _stepx;
    private float _stepy;

    private int _cols;
    private int _rows;
    private int _topIgnored;
    private int _bottomIgnored;
    private float _rx;
    private float _ry;

    //[SerializeField]
    //private bool _autoAjustItems;

    //public bool AutoAjustItems
    //{
    //    get { return _autoAjustItems; }
    //    set { _autoAjustItems = value; }
    //}

    #endregion


    #region Draw

    protected override void BeforeDrawChildren(GUIStyle listStyle, GUIStyle scrollStyle)
    {
        base.BeforeDrawChildren(listStyle, scrollStyle);

        GUIStyle slotStyle = SlotStyle ?? DefaultSlotStyle;
        //_slotSize = new Size(slotStyle.fixedWidth == 0 ? DefaultSlotSize : slotStyle.fixedWidth, slotStyle.fixedHeight == 0 ? DefaultSlotSize : slotStyle.fixedHeight);

        _initialPosx = listStyle.padding.left + scrollStyle.padding.left + slotStyle.padding.left;
        _initialPosy = listStyle.padding.top + scrollStyle.padding.top + slotStyle.padding.top;
        _stepx = _slotSize.x + slotStyle.margin.horizontal;
        _stepy = _slotSize.y + slotStyle.margin.vertical;

        _cols = (int)(ScrollView.width / _stepx);

        if ((LimitColumns) && (MaximumSize.y > 0))
        {
            _cols = Math.Min(_cols, (int)MaximumSize.y);
        }

        _rows = (Model == null)
                    ? (int)(ScrollRect.height / _stepy)
                    : (int)Mathf.Max(Mathf.Ceil((Model.GetLastSlot() + 1) / (float)_cols) + (NoExtraRow ? 0 : 1), Mathf.Round(ScrollRect.height / _stepy));

        if ((LimitRows) && (MaximumSize.x > 0))
        {
            _rows = Math.Min(_rows, (int)MaximumSize.x);
        }

        if (_rows == 0 || _cols == 0)
        {
            return;
        }

        float y = _initialPosy;

        _topIgnored = -1;
        _bottomIgnored = int.MaxValue;

        for (int i = 0; i < _rows; i++)
        {
            if (y + _stepy - ScrollPosition.y < 0)
            {
                y += _stepy;
                _topIgnored = i;
                continue;
            }

            float x = _initialPosx;
            for (int j = 0; j < _cols; j++)
            {
                Rect pos = new Rect(x, y, _slotSize.x, _slotSize.y);
                slotStyle.Draw(pos, pos.Contains(Event.current.mousePosition) && Stage.HoverWindow == TopWindow, false, false, false);
                x += _stepx;
            }
            y += _stepy;

            if (y - ScrollPosition.y > ScrollRect.yMax)
            {
                _bottomIgnored = i + 1;
                break;
            }
        }

        ScrollView.height = (_stepy * _rows) + scrollStyle.padding.vertical + listStyle.padding.vertical + scrollStyle.margin.vertical;
        ShowScroll = (ScrollView.height > ScrollRect.height);
    }

    protected override void PopulateAndDraw(BitControl listRenderer, ISlotListModel model, IPopulator populator)
    {
        //GUIStyle rendererStyle = Renderer.Style ?? Renderer.DefaultStyle;
        //GUIStyle slotStyle = SlotStyle ?? DefaultSlotStyle;
        //if (_autoAjustItems)
        //{

        //    listRenderer.Size = new Size(
        //        _slotSize.x - (rendererStyle.margin.horizontal+ slotStyle.padding.horizontal)*4, 
        //        _slotSize.y - (rendererStyle.margin.vertical + slotStyle.padding.vertical)*8);
        //}

        _rx = (_slotSize.x - listRenderer.Position.width) / 2;
        _ry = (_slotSize.y - listRenderer.Position.height) / 2;


        for (int i = 0, count = model.Count; i < count; i++)
        {
            object data = model[i];
            if (data == null)
            {
                continue;
            }
            int slot = model.GetSlotOfIndex(i);
            int row = (int)Mathf.Floor(slot / (float)_cols);
            int col = slot - (row * _cols);
            if (row <= _topIgnored)
            {
                continue;
            }
            if (row >= _bottomIgnored)
            {
                break;
            }

            listRenderer.Location = new Point(_rx + _initialPosx + (col * _stepx), _ry + _initialPosx + (row * _stepy));

            //bool selected = CheckSelection(data, listRenderer.Position);
            bool selected = IsSelected(data);
            IsOn = selected;

            using (BitGuiContext.Push(this, listRenderer, data, i, IsOn))
            {
                populator.Populate(listRenderer, data, i, selected);

                listRenderer.Draw();
            }
        }
    }

    protected override void DrawInEditMode()
    {
    }

    #endregion

    public override object GetObjectDataAt(Vector2 mousePosition)
    {
        int index = GetSlotIndexAt(mousePosition);

        if (index == -1)
            return null;

        return Model.GetDataAtSlot(index);

        //if (!AbsolutePosition.Contains(mousePosition))
        //{
        //    return null;
        //}

        //int row = (int)Mathf.Floor(((mousePosition.y - AbsolutePosition.y) - _initialPosy - ScrollPosition.y) / _stepy);
        //int col = (int)Mathf.Floor(((mousePosition.x - AbsolutePosition.x) - _initialPosx - ScrollPosition.x) / _stepx);

        //return Model.GetDataAtSlot((row * _cols) + col);
    }

    public int GetSlotIndexAt(Vector2 mousePosition)
    {
        if (!AbsolutePosition.Contains(mousePosition))
        {
            return -1;
        }

        int row = (int)Mathf.Floor(((mousePosition.y - AbsolutePosition.y) - _initialPosy - ScrollPosition.y) / _stepy);
        int col = (int)Mathf.Floor(((mousePosition.x - AbsolutePosition.x) - _initialPosx - ScrollPosition.x) / _stepx);

        return ((row * _cols) + col);
    }
}
