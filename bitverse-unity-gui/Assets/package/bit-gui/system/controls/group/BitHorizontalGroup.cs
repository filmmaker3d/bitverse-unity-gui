using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitHorizontalGroup : AbstractBitLayoutGroup
{
    #region Draw

    public override void FitContent()
    {
        GUIStyle containerStyle = Style ?? DefaultStyle;
        float horizontalMargin = 0;
        float minWidth = 0;
        int numberOfNonFixedWidths = 0;

        // Precompute free space and number of adjusable items
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
            horizontalMargin += cStyle.margin.left + cStyle.margin.right;

            if (c.FixedWidth)
            {
                minWidth += c.Position.width;
            }
            else
            {
                numberOfNonFixedWidths++;
            }

            firstVisibleElementStyle = (firstVisibleElementStyle ?? cStyle);
            lastVisibleElementStyle = cStyle;
        }
        if (Invert)
        {
            horizontalMargin -= ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.right);
            horizontalMargin -= ((lastVisibleElementStyle == null) ? 0 : lastVisibleElementStyle.margin.left);
        }
        else
        {
            horizontalMargin -= ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.left);
            horizontalMargin -= ((lastVisibleElementStyle == null) ? 0 : lastVisibleElementStyle.margin.right);
        }


        float verticalBorder = containerStyle.border.top + containerStyle.border.bottom;
        float horizontalBorder = containerStyle.border.left + containerStyle.border.right;


        float divWidth = (numberOfNonFixedWidths == 0) ? 0 : (Position.width - minWidth - horizontalMargin - horizontalBorder) / numberOfNonFixedWidths;
        divWidth = divWidth <= 0 ? 0 : divWidth;

        // compute final size and location of each item
        float xoffset;

        if (Invert)
        {
            xoffset = containerStyle.border.right - ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.right);
        }
        else
        {
            xoffset = containerStyle.border.left - ((firstVisibleElementStyle == null) ? 0 : firstVisibleElementStyle.margin.left);
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
                newSize.Width = (c.FixedWidth) ? c.Position.width : divWidth;

                //compute vertical size and alignment
                newSize.Height = (c.FixedHeight)
                                    ? c.Position.height
                                    : Position.height - verticalBorder;

                float ypos = c.Position.y;
                if (c.FixedHeight)
                {
                    switch (c.Alignment)
                    {
                        case GrouppingAligments.Top:
                            ypos = 0 + containerStyle.border.top;
                            break;
                        case GrouppingAligments.Center:
                            ypos = (Position.height / 2) - (c.Position.height / 2);
                            break;
                        case GrouppingAligments.Bottom:
                            ypos = Position.height - containerStyle.border.bottom - c.Position.height;
                            break;
                    }
                }
                else
                {
                    ypos = 0 + containerStyle.border.top;
                }
                if (Invert)
                {
                    c.Size = newSize;
                    xoffset += cStyle.margin.right;
                    c.Location = new Point(Position.width - c.Size.Width - xoffset, ypos);
                    xoffset += newSize.Width + cStyle.margin.left;
                }
                else
                {
                    c.Size = newSize;
                    xoffset += cStyle.margin.left;
                    c.Location = new Point(xoffset, ypos);
                    xoffset += newSize.Width + cStyle.margin.right;
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

        float minFixY = float.MaxValue;
        float maxFixY = float.MinValue;

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

            if (!c.FixedHeight)
            {
                continue;
            }
            minFixY = Math.Min(c.Position.y, minFixY);
            maxFixY = Math.Max(c.Position.y + c.Position.height, maxFixY);
        }

        miny = (minFixY != float.MaxValue) ? minFixY : miny;
        maxy = (maxFixY != float.MinValue) ? maxFixY : maxy;

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