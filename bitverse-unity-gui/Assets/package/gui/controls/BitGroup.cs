using System;
using UnityEngine;
using System.Collections.Generic;


    public class BitGroup: BitContainerControl
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
                GUI.BeginGroup(Position, Content, Style);
            }
            else
            {
                GUI.BeginGroup(Position, Content);
            }

            DrawChildren();

            GUI.EndGroup();
        }

    }

