using Bitverse.Unity.Gui;
using UnityEngine;


public class BitMenuItem : BitButton
{
	public BitMenuItem()
	{
		Anchor = 0;
		MouseClick += BitMenuItem_MouseClick;
	}

	void BitMenuItem_MouseClick(object sender, System.EventArgs e)
	{
		Debug.Log(">>");
	}


	public override int Anchor
	{
		get { return base.Anchor; }
		set { base.Anchor = AnchorStyles.Left | AnchorStyles.Right; }
	}
}