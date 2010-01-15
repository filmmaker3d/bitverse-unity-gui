using UnityEngine;
using Bitverse.Unity.Gui;


    public class BitPasswordField : BitControl
    {
        public event ValueChangedEventHandler TextChanged;

        //private string _text = "";
        [SerializeField]
        private int _maxLenght = -1;
        [SerializeField]
        private char _maskChar = '*';

        public string Text
        {
            get { return Content.text; }
            set 
            {
                Content.text = value;
                if (TextChanged != null) TextChanged(this, new ValueChangedEventArgs(Text));
            }
        }

        public int MaxLenght
        {
            get { return _maxLenght; }
            set { _maxLenght = value; }
        }

        public char MaskChar
        {
            get { return _maskChar; }
            set { _maskChar = value; }
        }

        public override void DoDraw()
        {
            string t;

            if (Style != null)
            {
                if (MaxLenght == -1)
                {
                    t = GUI.PasswordField(Position, Text, MaskChar, Style);
                }
                else
                {
                    t = GUI.PasswordField(Position, Text, MaskChar, MaxLenght, Style);
                }
            }
            else
            {
                if (MaxLenght == -1)
                {
                    t = GUI.PasswordField(Position, Text, MaskChar);
                }
                else
                {
                    t = GUI.PasswordField(Position, Text, MaskChar, MaxLenght);
                }
            }

            if (Text != t)
            {
                Text = t;
            }
        }

       
    }
