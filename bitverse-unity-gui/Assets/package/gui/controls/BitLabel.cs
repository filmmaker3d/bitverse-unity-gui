using System;
using UnityEngine;
using System.Collections;
using Bitverse.Unity.Gui;

public class BitLabel : BitControl
{

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
        if (Style != null)
        {
            GUI.Label(Position, Content, Style);
        }
        else
        {
            GUI.Label(Position, Content);
        }
    }

    public void SetText(string ntext)
    {
        Text = ntext;
    }


}



