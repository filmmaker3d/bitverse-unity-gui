using System;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;

public class BitList : AbstractBitList<IListModel, IPopulator>
{
    #region Data

    [SerializeField]
    public bool BottomAligned;

    #endregion

    #region Draw

    public bool EnableSelectedItems;
    public bool NotSelectableItems;

    protected override void PopulateAndDraw(BitControl listRenderer, IListModel model, IPopulator populator)
    {
        GUIStyle rendererStyle = Renderer.Style ?? Renderer.DefaultStyle;
        GUIStyle scrollStyle = Skin.scrollView;

        Rect itemPositionTemplate = new Rect(rendererStyle.margin.left + scrollStyle.padding.left + scrollStyle.contentOffset.x,
                                     rendererStyle.margin.top + scrollStyle.padding.top + scrollStyle.contentOffset.y,
                                     (rendererStyle.fixedWidth <= 0 ? ScrollView.width - rendererStyle.margin.horizontal : rendererStyle.fixedWidth) - scrollStyle.contentOffset.x,
                                     rendererStyle.fixedHeight <= 0 ? listRenderer.Size.Height : rendererStyle.fixedHeight);

        float contentHeight = rendererStyle.margin.top;

        ScrollView.height = scrollStyle.contentOffset.y;

        for (int i = 0; i < model.Count; i++)
        {
            object data = model[i];
            using (BitGuiContext.Push(this, listRenderer, data, i, IsOn))
            {
                bool selected = IsSelected(data);
                IsOn = NotSelectableItems ? false : selected;
                listRenderer.KeepEnabled = EnableSelectedItems ? IsOn : false;
                populator.Populate(listRenderer, data, i, selected);

                if (listRenderer.Visible)
                {
                    listRenderer.Position = new Rect(itemPositionTemplate.x,
                                                     contentHeight,
                                                     itemPositionTemplate.width,
                                                     itemPositionTemplate.height);
                    if (listRenderer.Position.y > (ScrollPosition.y + ScrollRect.height) || (listRenderer.Position.y+listRenderer.Position.height) < ScrollPosition.y)
                        listRenderer.SupressNextDraw = true;
                    listRenderer.Draw();

                    contentHeight += listRenderer.Position.height + rendererStyle.margin.vertical;
                    ScrollView.height += listRenderer.Position.height + rendererStyle.margin.vertical;
                }
            }
        }
        contentHeight -= rendererStyle.margin.bottom;
        ScrollView.height -= rendererStyle.margin.vertical;

        if (AutoSize)
        {
            ShowScroll = false;
            SecureChangeSize(new Size(Position.width,ScrollView.height));
        }
        else
        {
            ShowScroll = (ScrollView.height > ScrollRect.height);
            ScrollView.y = 0;
            if (!ShowScroll && BottomAligned)
            {
                ScrollView.y = -ScrollRect.height + ScrollView.height;
            }
        }
    }

    #endregion

    #region Layout


    //This will turn the window into a minimum bounding rectangle around the components.
    //It also involves moving the components so that the minimum x,y become 0,0
    protected override void DoAutoSize()
    {
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
		Renderer.Size = new Size(ScrollView.width - rs.margin.horizontal, Renderer.Size.Height);
		Renderer.Location = new Point(rs.margin.left + ss.padding.left, rs.margin.top);

		rs.Draw(new Rect(rs.margin.left + ss.padding.left, rs.margin.top, ScrollView.width - rs.margin.horizontal, Renderer.Size.Height), Renderer.Content, false, true, false,
		        false);
		//ScrollRenderer.Draw();
	}

	#endregion


	#region MonoBehaviour

	//public override void Awake()
	//{
	//    base.Awake();
	//    Populator = new DefaultBitListPopulator();
	//    Model = new DefaultBitListModel();
	//}

	#endregion


	
	public override object GetObjectDataAt(Vector2 mousePosition)
	{
		if (Renderer == null || _model == null)
		{
			return null;
		}

		GUIStyle rendererStyle = Renderer.Style ?? Renderer.DefaultStyle;
		GUIStyle scrollStyle = Skin.scrollView;

		float stepy = Renderer.Position.height + rendererStyle.margin.vertical;

		int row = (int) Mathf.Floor((mousePosition.y - scrollStyle.padding.top + ScrollPosition.y) / stepy);
		return _model[row];
	}
}