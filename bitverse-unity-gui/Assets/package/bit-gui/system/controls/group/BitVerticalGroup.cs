using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitVerticalGroup : AbstractBitLayoutGroup
{
    #region Draw

    public override void FitContent()
    {
        GUIStyle containerStyle = Style ?? DefaultStyle;
        float verticalMargin = 0;
        float minHeight = 0;
        int numberOfNonFixedHeights = 0;

        // Precompute free space and number of adjustable items
        //for (int i = 0; i < InternalControlCount; i++)
        GUIStyle firstVisibleElementStyle = null;
        GUIStyle lastVisibleElementStyle = null;
        for (int i = 0; i < IndexMap.Count; i++)
        {
            //BitControl c = InternalGetControlAt(i);
            BitControl c = IndexMap[i];

            if (c == null || !c.Visible)
            {
                continue;
            }

            GUIStyle cStyle = c.Style ?? c.DefaultStyle;
            verticalMargin += cStyle.margin.top + cStyle.margin.bottom;

            if (c.FixedHeight)
            {
                c.RecursiveAutoSize();
                minHeight += c.Position.height;
            }
            else
            {
                numberOfNonFixedHeights++;
            }

            firstVisibleElementStyle = (firstVisibleElementStyle ?? cStyle);
            lastVisibleElementStyle = cStyle;
        }
        if (Invert)
        {
            verticalMargin -= ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.bottom);
            verticalMargin -= ((lastVisibleElementStyle == null) ? 0 : lastVisibleElementStyle.margin.top);
        }
        else
        {
            verticalMargin -= ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.top);
            verticalMargin -= ((lastVisibleElementStyle == null) ? 0 : lastVisibleElementStyle.margin.bottom);
        }
        float verticalBorder = containerStyle.border.top + containerStyle.border.bottom;
        float horizontalBorder = containerStyle.border.left + containerStyle.border.right;


        float divHeight = (numberOfNonFixedHeights == 0) ? 0 : (Position.height - minHeight - verticalMargin - verticalBorder) / numberOfNonFixedHeights;
        divHeight = divHeight <= 0 ? 0 : divHeight;

        // compute final size and location of each item
        float yoffset;
        if (Invert)
        {
            yoffset = containerStyle.border.bottom + ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.bottom);
        }
        else
        {
            yoffset = containerStyle.border.top + ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.top);
        }

        //for (int i = 0; i < InternalControlCount; i++)
        for (int i = 0; i < IndexMap.Count; i++)
        {
            //BitControl c = InternalGetControlAt(i);
            BitControl c = IndexMap[i];

            if (c == null)
                continue;

            if (c.Visible)
            {
                Size newSize = new Size(0, 0);

                GUIStyle cStyle = c.Style ?? c.DefaultStyle;

                // compute horizontal size and update offset
                newSize.Height = (c.FixedHeight) ? c.Position.height : divHeight;

                //compute vertical size and alignment
                newSize.Width = (c.FixedWidth)
                                    ? c.Position.width
                                    : Position.width - horizontalBorder;

                float xpos = c.Position.x;
                if (c.FixedWidth)
                {
                    switch (c.Alignment)
                    {
                        case GrouppingAligments.Left:
                            xpos = 0 + containerStyle.border.left;
                            break;
                        case GrouppingAligments.Center:
                            xpos = (Position.width / 2) - (c.Position.width / 2);
                            break;
                        case GrouppingAligments.Right:
                            xpos = Position.width - containerStyle.border.right - c.Position.width;
                            break;
                    }
                }
                else
                {
                    xpos = 0 + containerStyle.border.left;
                }

                c.Size = newSize;
                if (Invert)
                {
                    yoffset += cStyle.margin.bottom;
                    c.Location = new Point(xpos, Position.height - c.Size.Height - yoffset);
                    yoffset += newSize.Height + cStyle.margin.top;
                }
                else
                {
                    yoffset += cStyle.margin.top;
                    c.Location = new Point(xpos, yoffset);
                    yoffset += newSize.Height + cStyle.margin.bottom;
                }
            }
        }
    }

    protected override void DoAutoSize()
    {
        GUIStyle currStyle = Style ?? DefaultStyle;
        float minx = float.MaxValue;
        float miny = float.MaxValue;
        float maxx = float.MinValue;
        float maxy = float.MinValue;

        float minFixX = float.MaxValue;
        float maxFixX = float.MinValue;

        for (int i = 0; i < ControlCount; i++)
        {
            BitControl c = InternalGetControlAt(i);
            if (!c.Visible)
            {
                continue;
            }
            minx = Math.Min(c.Position.x, minx);
            miny = Math.Min(c.Position.y, miny);

            maxx = Math.Max(c.Position.x + c.Position.width, maxx);
            maxy = Math.Max(c.Position.y + c.Position.height, maxy);

            if (!c.FixedWidth)
            {
                continue;
            }
            minFixX = Math.Min(c.Position.x, minFixX);
            maxFixX = Math.Max(c.Position.x + c.Position.width, maxFixX);
        }

        minx = (minFixX != float.MaxValue) ? minFixX : minx;
        maxx = (maxFixX != float.MinValue) ? maxFixX : maxx;

        //If nothing changed or no children, dont change the size of the window
        if (minx == float.MaxValue || maxx == float.MinValue)
            return;

        //Move all children
        for (int i = 0; i < ControlCount; i++)
        {
            BitControl c = InternalGetControlAt(i);
            if (c.Visible)
            {
                c.Position = new Rect(c.Position.x - minx + currStyle.border.left /* + currStyle.padding.left*/, c.Position.y - miny
                                                                                                                 + currStyle.border.top /* + currStyle.padding.top*/, c.Position.width,
                                      c.Position.height);
            }
        }

        //Pack
        Position = new Rect(Position.x, Position.y, maxx - minx + currStyle.border.left + currStyle.border.right /* + currStyle.padding.left + currStyle.padding.right*/,
                            maxy - miny + currStyle.border.top + currStyle.border.bottom /* + currStyle.padding.top + currStyle.padding.bottom*/);
    }

    #endregion
}