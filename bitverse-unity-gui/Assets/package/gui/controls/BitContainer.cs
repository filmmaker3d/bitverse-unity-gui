using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public abstract class BitContainer : BitControl
{
	#region Layout

	protected override void DoLayout()
	{
		base.DoLayout();
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
			if (c != null)
			{
				//c.ReLayout();
			}
		}
	}

	#endregion


	#region Draw

	protected virtual void DrawChildren()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
			if (c != null)
			{
				// TODO Find a better way to do this
				if (c.Anchor == -1)
				{
					c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
				}
				c.Draw();
			}
		}
	}

	#endregion

	#region Hierarchy

	/// <summary>
	/// Adds a control to the hierarchy.
	/// </summary>
	/// <param name="controlType">Control type to add. Must be a BitControl child.</param>
	/// <param name="controlName">Name of the control.</param>
	/// <returns>A new instance of the control of given type and name.</returns>
	public BitControl AddControl(Type controlType, string controlName)
	{
		if (controlType == null || !typeof(BitControl).IsAssignableFrom(controlType))
		{
			return null;
		}

		GameObject go = new GameObject();
		BitControl control = (BitControl)go.AddComponent(controlType);
		go.transform.parent = transform;
		go.name = controlName;
		//control.InitialSetup();
		return control;
	}

	/// <summary>
	/// Adds a control with default name: the class name without the "Bit" prefix.
	/// </summary>
	/// <param name="controlType">Control type to add. Must be a BitControl child.</param>
	/// <returns>A new instance of the control of given type and automatically named.</returns>
	/// <seealso cref="AddControl(System.Type,string)"/>
	public BitControl AddControl(Type controlType)
	{
		return AddControl(controlType, controlType.Name.Substring("Bit".Length));
	}

	/// <summary>
	/// Find the first control of type <see cref="T"/>.
	/// Searches in all its children.
	/// </summary>
	/// <typeparam name="T">The control's type to search.</typeparam>
	/// <returns>The control of given type or null with there is no one.</returns>
	public T FindControl<T>() where T : BitControl
	{
		return GetComponentInChildren<T>();
	}

	/// <summary>
	/// Find the first control of type <see cref="T"/> and name <see cref="controlName"/>.
	/// Searches in all its children.
	/// </summary>
	/// <typeparam name="T">The control's type to search.</typeparam>
	/// <param name="controlName">The control's name to search.</param>
	/// <returns>The control of given type and name or null with there is no one.</returns>
	public T FindControl<T>(string controlName) where T : BitControl
	{
		if (string.IsNullOrEmpty(controlName))
		{
			return null;
		}

		T[] controls = FindAllControls<T>();
		if (controls == null)
		{
			return null;
		}

		foreach (T control in controls)
		{
			if (controlName.Equals(control.name))
			{
				return control;
			}
		}
		return null;
	}

	/// <summary>
	/// Find all controls of type <see cref="T"/>.
	/// Searches in all its children.
	/// </summary>
	/// <typeparam name="T">The control's type to search.</typeparam>
	/// <returns>All controls of given type or null with there is no one.</returns>
	public T[] FindAllControls<T>() where T : BitControl
	{
		T[] children = GetComponentsInChildren<T>();

		if (children == null || children.Length == 0)
		{
			return null;
		}

		return children;
	}

	#endregion
}