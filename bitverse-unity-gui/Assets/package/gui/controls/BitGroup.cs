using Bitverse.Unity.Gui;
using UnityEngine;


public class BitGroup : BitContainer
{
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
            GUI.BeginGroup(Position, Content, Style);
        }
        else
        {
            GUI.BeginGroup(Position, Content);
        }

        DrawChildren();

        GUI.EndGroup();
    }
	#endregion
}

