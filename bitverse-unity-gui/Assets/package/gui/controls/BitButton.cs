using System;
using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class BitButton : BitControl {

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

    public void SetText(string ntext)
    {
        Text = ntext;
    }


}



