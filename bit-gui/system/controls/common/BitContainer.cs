using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class
    BitContainer : BitControl
{
    #region Appearance

    public override Color Color
    {
        set
        {
            if (PropagateColors)
            {
                for (int i = 0, count = InternalControlCount; i < count; i++)
                {
                    BitControl c = InternalGetControlWithoutIndexCheck(i);
                    if (c != null)
                    {
                        c.Color = value;
                    }
                }

            }
            base.Color = value;
        }
    }

    public override bool MouseEnabled
    {
        set
        {
            for (int i = 0, count = InternalControlCount; i < count; i++)
            {
                BitControl c = InternalGetControlWithoutIndexCheck(i);
                if (c != null)
                {
                    c.MouseEnabled = value;
                }
            }
            base.MouseEnabled = value;
        }
    }

    /// <summary>
    /// Propagate color to child when changeColor.
    /// </summary>
    private bool _propagateColors = true;
    public bool PropagateColors
    {
        get { return _propagateColors; }
        set { _propagateColors = value; }
    }

    #endregion


    #region Draw

    protected virtual void DrawChildren()
    {
        if (gameObject.animation != null && gameObject.animation.isPlaying)
            return;

        int count = InternalControlCount;
        if (Event.current.type == EventType.Repaint)
        {
            for (int i = 0; i < count; i++)
            {
                BitControl c = InternalGetControlWithoutIndexCheck(i);
                if (c != null)
                {
                    c.Draw();
                }
            }
        }
        else
        {
            for (int i = count - 1; i >= 0; i--)
            {
                BitControl c = InternalGetControlWithoutIndexCheck(i);
                if (c != null)
                {
                    c.Draw();
                }
            }
        }
    }

    #endregion


    #region Hierarchy

    public int ControlCount
    {
        get { return InternalControlCount; }
    }

    /// <summary>
    /// Gets the control at <see cref="index"/>, child of this Container.
    /// </summary>
    /// <param name="index">Control index to get.</param>
    /// <returns>The BitControl at <see cref="index"/> or null if the index is invalid.</returns>
    public BitControl GetControlAt(int index)
    {
        return InternalGetControlAt(index);
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> and name <see cref="controlName"/> in this container and in all its children.
    /// Throws an 'ArgumentException' if the control is not found.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <param name="controlName">The Control's name to search.</param>
    /// <returns>The Control of given _type and name or null with there is no one. Or 'ArgumentException' if not found</returns>
    public T GetControlInChildren<T>(string controlName) where T : BitControl
    {
        T ret = InternalFindControlInChildren<T>(controlName);
        if (ret == null)
            throw new ArgumentException("Control not found: " + controlName);
        return ret;
    }

    /// <summary>
    /// Gets the <see cref="control"/> index inside this component.
    /// </summary>
    /// <param name="control">The <see cref="BitControl"/> to find out its index.</param>
    /// <returns>The index of the <see cref="control"/> or -1 if there is no such Control inside this Container.</returns>
    public int GetControlIndex(BitControl control)
    {
        return InternalGetControlIndex(control);
    }

    /// <summary>
    /// Creates and adds a <see cref="BitControl"/> to hierarchy.
    /// </summary>
    /// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
    /// <param name="controlName">Name of the Control.</param>
    /// <returns>A new instance of the Control of given _type and name.</returns>
    public BitControl AddControl(Type controlType, string controlName)
    {
        return InternalAddControl(controlType, controlName);
    }

    /// <summary>
    /// Creates and adds a Control to hierarchy with default name: the class name without the "Bit" prefix.
    /// </summary>
    /// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
    /// <returns>A new instance of the Control of given _type and automatically named.</returns>
    /// <seealso cref="AddControl(System.Type,string)"/>
    public BitControl AddControl(Type controlType)
    {
        return InternalAddControl(controlType, controlType.Name.Substring("Bit".Length));
    }

    /// <summary>
    /// Creates and adds a <see cref="BitControl"/> to hierarchy.
    /// </summary>
    /// <param name="controlName">Name of the Control.</param>
    /// <returns>A new instance of the Control of given _type and name.</returns>
    public T AddControl<T>(string controlName) where T : BitControl
    {
        return InternalAddControl<T>(controlName);
    }

    ///<summary>
    /// Adds an instantiated <see cref="BitControl"/> to hierarchy.
    ///</summary>
    ///<param name="control">Control to add.</param>
    public void AddControl(BitControl control)
    {
        InternalAddControl(control);
    }

    /// <summary>
    /// Adds a Control with default name: the class name without the "Bit" prefix.
    /// </summary>
    /// <returns>A new instance of the Control of given _type and automatically named.</returns>
    /// <seealso cref="AddControl(System.Type,string)"/>
    public T AddControl<T>() where T : BitControl
    {
        return InternalAddControl<T>(typeof(T).Name.Substring("Bit".Length));
    }

    /// <summary>
    /// Removes the given Control from the hierarchy.
    /// </summary>
    /// <param name="control">Control to remove.</param>
    public void RemoveControl(BitControl control)
    {
        InternalRemoveControl(control);
    }

    /// <summary>
    /// Removes the Control with given name from the hierarchy.
    /// </summary>
    /// <param name="controlName">Name of the Control to remove.</param>
    public void RemoveControl(string controlName)
    {
        InternalRemoveControl(FindControl<BitControl>(controlName));
    }

    /// <summary>
    /// Finds the first Control direct children of _type <see cref="T"/> in this container.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <returns>The Control of given _type or null with there is no one.</returns>
    public T FindControl<T>() where T : BitControl
    {
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            Component c = transform.GetChild(i).GetComponent<T>();
            if (c == null)
            {
                continue;
            }
            return (T)c;
        }
        return null;
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> and name <see cref="controlName"/> in this container.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <param name="controlName">The Control's name to search.</param>
    /// <returns>The Control of given _type or null with there is no one.</returns>
    public T FindControl<T>(string controlName) where T : BitControl
    {
        return InternalFindControl<T>(controlName);
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> in this container and in all its children.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <returns>The Control of given _type or null with there is no one.</returns>
    public T FindControlInChildren<T>() where T : BitControl
    {

        Transform transform1 = transform;
        for (int i = transform1.childCount; --i >= 0; )
        {
            BitControl c = transform1.GetChild(i).GetComponent<BitControl>();
            if (c != null)
            {
                if (c is T)
                    return (T)c;
            }
        }

        for (int i = transform1.childCount; --i >= 0; )
        {
            BitContainer c = transform1.GetChild(i).GetComponent<BitContainer>();
            if (c != null)
            {
                BitControl x = c.FindControlInChildren<T>();
                if (x != null)
                {
                    return (T)x;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> and name <see cref="controlName"/> in this container and in all its children.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <param name="controlName">The Control's name to search.</param>
    /// <returns>The Control of given _type and name or null with there is no one.</returns>
    public T FindControlInChildren<T>(string controlName) where T : BitControl
    {
        return InternalFindControlInChildren<T>(controlName);
    }

    public void FindAllControls<T>(List<T> list) where T : BitControl
    {
        Transform transform1 = transform;
        for (int i = transform1.childCount; --i >= 0; )
        {
            BitControl c = transform1.GetChild(i).GetComponent<BitControl>();
            if (c != null)
            {
                if (c is T)
                    list.Add((T)c);

                if (c is BitContainer)
                    ((BitContainer)c).FindAllControls(list);
            }
        }
    }

    public void FindAllControls(ArrayList list)
    {
        Transform transform1 = transform;
        for (int i = transform1.childCount; --i >= 0; )
        {
            BitControl c = transform1.GetChild(i).GetComponent<BitControl>();
            if (c != null)
            {
                if (c is BitControl)
                    list.Add(c);

                if (c is BitContainer)
                    ((BitContainer)c).FindAllControls(list);
            }
        }
    }

    #endregion


    #region Layout

    internal override void LayoutChildren()
    {
        base.LayoutChildren();
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            BitControl c = transform.GetChild(i).GetComponent<BitControl>();
            if (c == null)
            {
                continue;
            }
            c.PerformLayoutItself();
            c.PerformLayoutChildren();
        }
    }


    //This will turn the window into a minimum bounding rectangle around the components.
    //It also involves moving the components so that the minimum x,y become 0,0
    protected override void DoAutoSize()
    {
        if (AutoSizeMode == AutoSizeModeEnum.all)
        {
            GUIStyle currStyle = Style ?? DefaultStyle;
            float minx = float.MaxValue;
            float miny = float.MaxValue;
            float maxx = float.MinValue;
            float maxy = float.MinValue;

            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();
                if (c.Visible)
                {
                    minx = Math.Min(c.Position.x, minx);
                    miny = Math.Min(c.Position.y, miny);

                    maxx = Math.Max(c.Position.x + c.Position.width, maxx);
                    maxy = Math.Max(c.Position.y + c.Position.height, maxy);


                }
            }

            //If nothing changed or no children, dont change the size of the window
            //if (minx == float.MaxValue || maxx == float.MinValue)
            //    return;

            //Move all children
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();

                //TODO: BitGroup etc should behave like BitWindow
                if (this is BitWindow)
                {
                    c.Position = new Rect(c.Position.x - minx, c.Position.y - miny, c.Position.width, c.Position.height);
                }
                else
                {
                    c.Position = new Rect(c.Position.x - minx + currStyle.padding.left, c.Position.y - miny + currStyle.padding.top, c.Position.width, c.Position.height);
                }
            }

            //Pack
            Position = new Rect(Position.x, Position.y, maxx - minx + currStyle.padding.left + currStyle.padding.right,
                                maxy - miny + currStyle.padding.top + currStyle.padding.bottom);
        }
        else if (AutoSizeMode == AutoSizeModeEnum.vertical)
        {
            GUIStyle currStyle = Style ?? DefaultStyle;
            float miny = float.MaxValue;
            float maxy = float.MinValue;

            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();
                if (c.Visible)
                {
                    miny = Math.Min(c.Position.y, miny);

                    maxy = Math.Max(c.Position.y + c.Position.height, maxy);


                }
            }

            //If nothing changed or no children, dont change the size of the window
            //if (minx == float.MaxValue || maxx == float.MinValue)
            //    return;

            //Move all children
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();

                //TODO: BitGroup etc should behave like BitWindow
                if (this is BitWindow)
                {
                    c.Position = new Rect(c.Position.x, c.Position.y - miny, c.Position.width, c.Position.height);
                }
                else
                {
                    c.Position = new Rect(c.Position.x + currStyle.padding.left, c.Position.y - miny + currStyle.padding.top, c.Position.width, c.Position.height);
                }
            }

            //Pack
            Position = new Rect(Position.x, Position.y, Position.width,
                                maxy - miny + currStyle.padding.top + currStyle.padding.bottom);
        }
        else if (AutoSizeMode == AutoSizeModeEnum.horizontal)
        {
            GUIStyle currStyle = Style ?? DefaultStyle;
            float minx = float.MaxValue;
            float maxx = float.MinValue;

            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();
                if (c.Visible)
                {
                    minx = Math.Min(c.Position.x, minx);

                    maxx = Math.Max(c.Position.x + c.Position.width, maxx);

                }
            }

            //If nothing changed or no children, dont change the size of the window
            //if (minx == float.MaxValue || maxx == float.MinValue)
            //    return;

            //Move all children
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                BitControl c = transform.GetChild(i).GetComponent<BitControl>();

                //TODO: BitGroup etc should behave like BitWindow
                if (this is BitWindow)
                {
                    c.Position = new Rect(c.Position.x - minx, c.Position.y, c.Position.width, c.Position.height);
                }
                else
                {
                    c.Position = new Rect(c.Position.x - minx + currStyle.padding.left, c.Position.y + currStyle.padding.top, c.Position.width, c.Position.height);
                }
            }

            //Pack
            Position = new Rect(Position.x, Position.y, maxx - minx + currStyle.padding.left + currStyle.padding.right,
                                Position.height);
        }
    }

    #endregion
}