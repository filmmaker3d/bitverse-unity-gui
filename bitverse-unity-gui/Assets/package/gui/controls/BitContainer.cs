using System;
using UnityEngine;


public abstract class BitContainer : BitControl
{
	#region Draw

	protected virtual void DrawChildren()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl) ch.GetComponent(typeof (BitControl));
			if (c != null)
			{
				// TODO Find a better way to do this
				//if (c.Anchor == -1)
				//{
				//    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
				//}
				c.Draw();
			}
		}
	}

	#endregion


	//#region Events

	//public event MouseDownEventHandler MouseDown;
	//private void RaiseMouseDownEvent()
	//{
	//    if (MouseDown != null)
	//    {
	//		MouseDown(this, new MouseDownEventArgs());
	//    }
	//}

	//public event MouseUpEventHandler MouseUp;
	//private void RaiseMouseUp()
	//{
	//    if (MouseUp != null)
	//    {
	//        MouseUp(this, new MouseUpEventArgs());
	//    }
	//}


	//protected override bool CheckUserEvents()
	//{
	// Invert this to improve performance
	//    if (!Position.IsSelected(Event.current.mousePosition))
	//    {
	//        return false;
	//    }

	//    switch (Event.current.type)
	//    {
	//        case EventType.mouseDown:
	//            RaiseMouseDownEvent();
	//            return true;
	//        case EventType.mouseUp:
	//            RaiseMouseUp();
	//            return true;
	//    }
	//    return false;
	//}

	//#endregion


	#region Hierarchy

	/// <summary>
	/// Adds a Control to the hierarchy.
	/// </summary>
	/// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
	/// <param name="controlName">Name of the Control.</param>
	/// <returns>A new instance of the Control of given _type and name.</returns>
	public virtual BitControl AddControl(Type controlType, string controlName)
	{
		return InternalAddControl(controlType, controlName);
	}

	/// <summary>
	/// Adds a Control with default name: the class name without the "Bit" prefix.
	/// </summary>
	/// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
	/// <returns>A new instance of the Control of given _type and automatically named.</returns>
	/// <seealso cref="AddControl(System.Type,string)"/>
	public BitControl AddControl(Type controlType)
	{
		return InternalAddControl(controlType, controlType.Name.Substring("Bit".Length));
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
			return (T) c;
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
		return GetComponentInChildren<T>();
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

	/// <summary>
	/// Finds all controls of _type <see cref="T"/>.
	/// Searches in all its children.
	/// </summary>
	/// <typeparam name="T">The Control's _type to search.</typeparam>
	/// <returns>All controls of given _type or null with there is no one.</returns>
	public T[] FindAllControls<T>() where T : BitControl
	{
		return InternalFindAllControls<T>();
	}

	#endregion


	#region Layout

	internal override void LayoutChildren()
	{
		base.LayoutChildren();
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl) ch.GetComponent(typeof (BitControl));
			if (c != null)
			{
				c.PerformLayoutItself();
				c.PerformLayoutChildren();
			}
		}
	}

	#endregion
}