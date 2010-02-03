using Bitverse.Unity.Gui;
using UnityEngine;


public class BitGridList : AbstractBitList
{
	#region Draw

	protected override void PopulateAndDraw(BitControl listRenderer, IBitListModel model, IBitListPopulator populator)
	{
		GUIStyle rendererStyle = ListRenderer.Style ?? ListRenderer.DefaultStyle;
		GUIStyle scrollStyle = Skin.scrollView ?? DefaultStyle;

		Rect rendererPosition = ListRenderer.Position;
		float xpos = scrollStyle.padding.left;
		float ypos = scrollStyle.padding.top;
		int lineCount = 0;

		for (int i = 0, count = model.Count; i < count; i++)
		{
			float step = rendererPosition.width + rendererStyle.margin.horizontal;
			if (xpos + step > _scrollView.width)
			{
				xpos = scrollStyle.padding.left;
				if (i > 0)
				{
					ypos += rendererPosition.height + rendererStyle.margin.bottom;
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

			if (itemPosition.y > _scrollRect.height + _scrollPosition.y)
			{
				ypos += (itemPosition.height + rendererStyle.margin.vertical) * (count - i - 1) / (lineCount == 0 ? 1 : lineCount) + scrollStyle.padding.bottom;
				break;
			}

			xpos += step;

			if (itemPosition.yMax < _scrollPosition.y)
			{
				continue;
			}
			object data = model[i];
			bool selected = CheckSelection(data, itemPosition);
			populator.Populate(listRenderer, data, i, selected);
			//listRenderer.Location = new Point(itemPosition.x, itemPosition.y);
			//listRenderer.Draw();
			rendererStyle.Draw(itemPosition, listRenderer.Content, itemPosition.Contains(Event.current.mousePosition), true, selected, false);
		}


		_scrollView.height = ypos + ListRenderer.Position.height + rendererStyle.margin.vertical + scrollStyle.padding.bottom;
		_showScroll = (_scrollView.height > _scrollRect.height);
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
		ListRenderer.Location = new Point(rs.margin.left + ss.padding.left, rs.margin.top);
		ListRenderer.Draw();
	}

	#endregion
}