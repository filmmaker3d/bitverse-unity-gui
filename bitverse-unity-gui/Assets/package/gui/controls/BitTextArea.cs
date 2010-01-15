using UnityEngine;
using Bitverse.Unity.Gui;


    public class BitTextArea : BitControl
    {
        public event ValueChangedEventHandler TextChanged;

        [SerializeField]
        private int _maxLenght = -1;

        public string Text
        {
            get { return Content.text; }
            set 
            {
                Content.text = value;
                if (TextChanged != null) TextChanged(this, new ValueChangedEventArgs(Content.text));
            }
        }

        public int MaxLenght
        {
            get { return _maxLenght; }
            set { _maxLenght = value; }
        }


        public override void DoDraw()
        {
            string t;

            if (Style != null)
            {
                if (MaxLenght < 0)
                {
                    t = GUI.TextArea(Position, Text, Style);
                }
                else
                {
                    t = GUI.TextArea(Position, Text, MaxLenght, Style);
                }
            }
            else
            {
                if (MaxLenght < 0)
                {
                    t =GUI.TextArea(Position, Text);
                }
                else
                {
                    t = GUI.TextArea(Position, Text, MaxLenght);
                }
            }

            if (Text != t)
            {
                Text = t;
            }
        }

    }
