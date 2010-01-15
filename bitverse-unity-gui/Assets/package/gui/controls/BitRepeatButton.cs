using UnityEngine;
using Bitverse.Unity.Gui;


    public class BitRepeatButton : BitControl
    {
        public event MouseClickEventHandler ButtonClick;

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
            bool button = false;
            int currentButton = -1;
            //bool doubleClick = false;

            if (Style == null)
            {
                button = UnityEngine.GUI.RepeatButton(Position, Content);
            }
            else
            {
                button = UnityEngine.GUI.RepeatButton(Position, Content, Style);
            }

            //doubleClick = Event.current.isMouse && Event.current.clickCount == 2;
            currentButton = Event.current.button;

            if (button)
            {
                if (ButtonClick != null)
                {
                    ButtonClick(this, new MouseClickEventArgs((MouseButtons)currentButton));
                }
            }
        }

      
    }

