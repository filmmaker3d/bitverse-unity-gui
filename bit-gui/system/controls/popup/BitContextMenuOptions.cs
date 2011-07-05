using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitContextMenuOptions : BitVerticalGroup
{

	#region MonoBehaviour
	#endregion

	#region Hierarchy

	protected override T InternalAddControl<T>(string controlName)
	{
		if (!typeof (IBitContextMenuItem).IsAssignableFrom(typeof (T)))
		{
			throw new ArgumentException("BitContextMenu only accepts BitContextMenuItem");
		}
		BitControl control = base.InternalAddControl<T>(controlName);
		SetupNewMenuItem((BitContextMenuItem) control);
		return (T) control;
	}

	private void SetupNewMenuItem(BitContextMenuItem control)
	{
		BitContextMenu parent = (BitContextMenu)Parent;
		GUIStyle parentStyle = parent.Style ?? parent.DefaultStyle;
		control.Size = new Size(parent.Position.width - parentStyle.padding.horizontal, control.Position.height);
		control.ParentContextMenu = parent;
	}

	protected override void InternalAddControl(BitControl control)
	{
		if (control == null)
		{
			return;
		}
		if (!typeof (IBitContextMenuItem).IsAssignableFrom(control.GetType()))
		{
			throw new ArgumentException("BitContextMenu only accepts BitContextMenuItem");
		}
		SetupNewMenuItem((BitContextMenuItem) control);
		base.InternalAddControl(control);
	}

	protected override BitControl InternalAddControl(Type controlType, string controlName)
	{
		if (controlType == null)
		{
			return null;
		}
		if (!typeof (IBitContextMenuItem).IsAssignableFrom(controlType))
		{
			throw new ArgumentException("BitContextMenu only accepts BitContextMenuItem");
		}
		BitControl control = base.InternalAddControl(controlType, controlName);
		SetupNewMenuItem((BitContextMenuItem) control);
		return control;
	}

	#endregion

	public override void Awake()
	{
		base.Awake();
		//Style = EmptyStyle;
		AutoSize = true;
		Location = new Point(0, 0);
		Anchor = AnchorStyles.All;
		HideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
		Unselectable = true;
		CanShowContextMenu = false; 
	}
}