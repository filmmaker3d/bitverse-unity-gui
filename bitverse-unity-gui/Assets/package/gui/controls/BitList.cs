using UnityEngine;


public class BitList : AbstractBitList
{
	#region Draw

	protected override void PopulateAndDraw(BitControl listRenderer, IBitListModel model, IBitListPopulator populator)
	{
		GUIStyle rendererStyle = ListRenderer.Style ?? ListRenderer.DefaultStyle;
		GUIStyle scrollStyle = Skin.scrollView;

		float xpos = rendererStyle.margin.left + scrollStyle.padding.left + scrollStyle.contentOffset.x;
		float ypos = scrollStyle.padding.top + scrollStyle.contentOffset.y;
		float width = (rendererStyle.fixedWidth <= 0 ? _scrollView.width - rendererStyle.margin.horizontal : rendererStyle.fixedWidth) - scrollStyle.contentOffset.x;
		float height = rendererStyle.fixedHeight <= 0 ? listRenderer.Size.Height : rendererStyle.fixedHeight;

		for (int i = 0, count = model.Count; i < count; i++)
		{
			Rect itemPosition = new Rect(xpos, rendererStyle.margin.top + ypos, width, height);
			ypos += itemPosition.height + rendererStyle.margin.vertical;

			if (itemPosition.y > _scrollRect.height + _scrollPosition.y)
			{
				ypos += (itemPosition.height + rendererStyle.margin.vertical) * (count - i - 1);
				break;
			}

			if (itemPosition.yMax < _scrollPosition.y)
			{
				continue;
			}

			object data = model[i];
			bool selected = CheckSelection(data, itemPosition);
			populator.Populate(listRenderer, data, i, selected);
			//listRenderer.Size = new Size(itemPosition.width, itemPosition.height);
			//listRenderer.Location = new Point(itemPosition.x, itemPosition.y);
			//listRenderer.Draw();
			rendererStyle.Draw(itemPosition, listRenderer.Content, itemPosition.Contains(Event.current.mousePosition), true, selected, false);
		}

		_showScroll = (ypos > _scrollRect.height);
		_scrollView.height = ypos + scrollStyle.contentOffset.y;
	}

	#endregion


	#region Editor

	protected override void DrawInEditMode()
	{
		if (ListRenderer == null)
		{
			return;
		}
		GUIStyle ss = Skin.scrollView;
		GUIStyle rs = ListRenderer.Style ?? ListRenderer.DefaultStyle;
		//ListRenderer.Size = new Size(_scrollView.width - rs.margin.horizontal, ListRenderer.Size.Height);
		//ListRenderer.Location = new Point(rs.margin.left + ss.padding.left, rs.margin.top);
		rs.Draw(new Rect(rs.margin.left + ss.padding.left, rs.margin.top, _scrollView.width - rs.margin.horizontal, ListRenderer.Size.Height), ListRenderer.Content, false, true, false, false);
		//ListRenderer.Draw();
	}

	#endregion
}