using UnityEngine;


public class BitLabel : BitControl
{
	#region Appearance

    public override string DefaultStyleName
	{
		get { return "label"; }
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
            GUI.Label(Position, Content, Style);
        }
        else
        {
            GUI.Label(Position, Content);
        }
    }

	#endregion
}



