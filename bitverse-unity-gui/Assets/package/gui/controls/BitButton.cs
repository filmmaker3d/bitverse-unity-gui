using UnityEngine;

public class BitButton : BitControl
{
	#region Appearance

	protected override string DefaultStyleName
	{
		get { return "button"; }
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
        GUIStyle style = Style;
        if (style != null)
        {
            GUI.Button(Position, Content, style);
        }
        else
        {
            GUI.Button(Position, Content);
        }
    }
    
	#endregion


}



