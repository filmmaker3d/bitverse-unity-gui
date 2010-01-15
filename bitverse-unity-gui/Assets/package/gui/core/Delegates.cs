using System;


namespace Bitverse.Unity.Gui
{
    public delegate void MouseClickEventHandler(object sender, MouseClickEventArgs e);
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public class MouseClickEventArgs : EventArgs
    {
        private MouseButtons _mouseButton;

        public MouseButtons MouseButton
        {
            get { return _mouseButton; }
        }

        public MouseClickEventArgs(MouseButtons mouseButton)
        {
            _mouseButton = mouseButton;
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        private object _value;

        public object Value
        {
            get { return _value; }
        }

        public ValueChangedEventArgs(object value)
        {
            _value = value;
        }
    }

}
