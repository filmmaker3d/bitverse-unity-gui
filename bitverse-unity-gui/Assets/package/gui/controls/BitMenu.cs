using Bitverse.Unity.Gui;
using UnityEngine;


public class BitMenu : BitContainer
{
	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "box"; }
	}

	#endregion


	#region Draw

	public override void DoDraw()
	{
		GUIStyle s = Style ?? "";
		GUI.BeginGroup(Position, Content, s);

		DrawChildren();

		GUI.EndGroup();
	}

	//private static readonly float DefaultMenuItemHeight = 20;

	protected override void DrawChildren()
	{
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			Transform ch = transform.GetChild(i);
			BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
			if (c == null || !(c is BitMenuItem))
			{
				continue;
			}
			//if (c.Anchor == -1)
			//{
			//	c.Anchor = AnchorStyles.Right | AnchorStyles.Left;
			//	c.Position = new Rect(0, i * DefaultMenuItemHeight, Position.width, DefaultMenuItemHeight);
			//	//Size = new Size(Size.Width, count * DefaultMenuItemHeight);
			//}
			c.Draw();
		}
	}

	#endregion
}