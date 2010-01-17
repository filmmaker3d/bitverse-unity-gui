using Bitverse.Unity.Gui;
using UnityEngine;

public class BitBox : BitControl
{
	#region Appearance

    public override string DefaultStyleName
	{
		get { return "box"; }
	}

	#endregion
	
    #region Data

    public string Text
    {
        get { return Content.text; }
        set { Content.text = value; }
    }

    public Texture Image
    {
        get { return Content.image; }
        set { Content.image = value; }
    }

    #endregion

	#region Draw

    public override void DoDraw()
    {
        if (Style != null)
        {
            UnityEngine.GUI.Box(Position, Content, Style);
        }
        else
        {
            UnityEngine.GUI.Box(Position, Content);
        }
    }

    #endregion

}
