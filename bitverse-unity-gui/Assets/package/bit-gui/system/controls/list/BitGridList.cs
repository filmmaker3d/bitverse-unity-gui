using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitGridList : AbstractBitList<IListModel, IPopulator>
{
    #region Data

    public bool NotSelectableItems;

    #endregion

    #region Draw

    //TODO optime this!
    protected override void PopulateAndDraw(BitControl listRenderer, IListModel model, IPopulator populator)
    {
        GUIStyle rendererStyle = Renderer.Style ?? Renderer.DefaultStyle;
        GUIStyle scrollStyle = Skin.scrollView ?? DefaultStyle;

        Rect rendererPosition = Renderer.Position;
        float xpos = scrollStyle.padding.left;
        float ypos = scrollStyle.padding.top;
        int lineCount = 0;

        for (int i = 0, count = model.Count; i < count; i++)
        {
            float step = rendererPosition.width + rendererStyle.margin.horizontal;
            if (xpos + step > ScrollView.width)
            {
                xpos = scrollStyle.padding.left;
                if (i > 0)
                {
                    ypos += rendererPosition.height + rendererStyle.margin.vertical;
                }
                if (lineCount == 0)
                {
                    lineCount = i;
                }
            }

            Rect itemPosition = new Rect(
                rendererStyle.margin.left + xpos,
                rendererStyle.margin.top + ypos,
                rendererPosition.width,
                rendererPosition.height);

            if (itemPosition.y > ScrollRect.height + ScrollPosition.y)
            {
                //TODO optimization: remove this!!!
                // This is necessary!
                // ReSharper disable PossibleLossOfFraction
                ypos += (itemPosition.height + rendererStyle.margin.vertical) * ((count - i - 1) / Mathf.Max(1, lineCount));
                // ReSharper restore PossibleLossOfFraction
                break;
            }

            xpos += step;

            if (itemPosition.yMax < ScrollPosition.y)
            {
                continue;
            }
            while (i < count)
            {
                object data = model[i];
                using (BitGuiContext.Push(this, listRenderer, data, i, IsOn))
                {
                    listRenderer.Location = new Point(itemPosition.x, itemPosition.y);
                    //bool selected = CheckSelection(data, itemPosition);
                    bool selected = NotSelectableItems ? false : IsSelected(data);
                    populator.Populate(listRenderer, data, i, selected);
                    if (!listRenderer.Visible)
                    {
                        i++;
                    }
                    else
                    {
                        IsOn = selected;
                        listRenderer.Draw();
                        break;
                    }
                }
            }
            //rendererStyle.Draw(itemPosition, listRenderer.Content, itemPosition.Contains(Event.current.mousePosition), true, selected, false);
        }
        //TODO and this!
        ScrollView.height = ypos + rendererPosition.height + rendererStyle.margin.vertical + scrollStyle.padding.bottom;
        ShowScroll = (ScrollView.height > ScrollRect.height);

    }

    #endregion


    #region Editor

    protected override void DrawInEditMode()
    {
        if (Renderer == null)
        {
            return;
        }
        GUIStyle ss = Skin.scrollView;
        GUIStyle rs = Renderer.Style ?? Renderer.DefaultStyle;
        Renderer.Location = new Point(rs.margin.left + ss.padding.left, rs.margin.top);
        Renderer.Draw();
    }

    #endregion

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();
        Populator = new DefaultBitListPopulator();
        Model = new DefaultBitListModel();
    }

    #endregion

    public override object GetObjectDataAt(Vector2 position)
    {
        position.x -= Position.x;
        position.y -= Position.y;
        //TODO
        GUIStyle rendererStyle = Renderer.Style ?? Renderer.DefaultStyle;
        GUIStyle scrollStyle = Skin.scrollView ?? DefaultStyle;

        Rect rendererPosition = Renderer.Position;

        float fxi = (position.x - scrollStyle.padding.left) / (rendererPosition.width + rendererStyle.margin.horizontal);
        float fyi = (position.y + ScrollPosition.y - scrollStyle.padding.top) / (rendererPosition.height + rendererStyle.margin.vertical);
        int xi = Mathf.FloorToInt(fxi);
        int yi = Mathf.FloorToInt(fyi);

        float fhorizontalCount = (ScrollView.width - scrollStyle.padding.left) /
                                 (rendererPosition.width + rendererStyle.margin.horizontal);
        int horizontalCount = Mathf.FloorToInt(fhorizontalCount);

        if (xi >= horizontalCount) return null;

        int index = (yi * horizontalCount) + xi;

        //Debug.Log("padding: " + scrollStyle.padding.left + "#" + scrollStyle.padding.top);
        //Debug.Log("position: " + rendererPosition.x + "#" + rendererPosition.y + "#" + rendererPosition.width + "#" + rendererPosition.height);
        //Debug.Log("click: " + position.x + "#" + position.y);
        //Debug.Log("index: "+index+" of x " + xi+"#" + fxi + "; y " + yi+"#" + fyi+"; and horizontalCount " + horizontalCount+"#"+fhorizontalCount);
        if (index < _model.Count)
        {
            float xpos = scrollStyle.padding.left + (xi * (rendererPosition.width + rendererStyle.margin.horizontal)) + rendererStyle.margin.left;
            float ypos = scrollStyle.padding.top + (yi * (rendererPosition.height + rendererStyle.margin.vertical)) + rendererStyle.margin.top;
            //Debug.Log("itemPosition: " + xpos + "#" + ypos + "#" + rendererPosition.width + "#" + rendererPosition.height);
            Rect itemPosition = new Rect(xpos, ypos, rendererPosition.width, rendererPosition.y);

            if (itemPosition.Contains(position))
                return _model[index];
        }

        return null;
    }
}
