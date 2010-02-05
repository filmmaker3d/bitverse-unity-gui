using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitMenu : BitContainer
{
	public BitMenu()
	{
		MaxSize = new Size(500, float.MaxValue);
	}


	#region Draw

	private const float DefaultMenuItemHeight = 20;

	protected override void DoDraw()
	{
		GUI.BeginGroup(Position, Content, Style ?? DefaultStyle);

		DrawChildren();

		GUI.EndGroup();
	}

	protected override void DrawChildren()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl) ch.GetComponent(typeof (BitControl));
			if (c == null)
			{
				continue;
			}
			if (c.Anchor == -1)
			{
				c.Position = new Rect(0, i * DefaultMenuItemHeight, Position.width, DefaultMenuItemHeight);
				c.Anchor = AnchorStyles.Right | AnchorStyles.Left;
			}
			c.Draw();
		}
	}

	#endregion


	#region Layout

	protected override void LayoutItself()
	{
		//Debug.Log("layout itself");
		//int count = transform.childCount;
		//if (count == 0)
		//{
		//    return;
		//}

		//float width = Size.Width;

		//for (int i = 0; i < count; i++)
		//{
		//    Transform ch = transform.GetChild(i);
		//    BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
		//    if (c == null)
		//    {
		//        continue;
		//    }
		//    if (width < c.Size.Width)
		//    {
		//        width = c.Size.Width;
		//    }
		//}

		//Size = new Size(width, count * DefaultMenuItemHeight);
	}

	internal override void LayoutChildren()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl) ch.GetComponent(typeof (BitControl));
			if (c != null)
			{
				c.Location = new Point(0, i * DefaultMenuItemHeight);
				c.Size = new Size(Size.Width, DefaultMenuItemHeight);
				c.MinSize = new Size(Size.Width, DefaultMenuItemHeight);
				c.MaxSize = new Size(Size.Width, DefaultMenuItemHeight);
				c.Anchor = AnchorStyles.Right | AnchorStyles.Left;
			}
		}
	}


	public override BitControl AddControl(Type controlControlType, string controlName)
	{
		BitControl control = base.AddControl(controlControlType, controlName);
		PerformLayoutChildren();
		return control;
	}

	#endregion


	//#region Editor

	//public override void InitialSetup()
	//{
	//    base.InitialSetup();
	//    Size = new Size(100, 50);
	//}

	//#endregion
}