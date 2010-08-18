using System;
using UnityEngine;


namespace Bitverse.Unity.Gui
{
    public delegate void InvalidatedEventHandler(object sender, InvalidatedEventArgs e);


    public class InvalidatedEventArgs
    {

    }


    public delegate void BeforeTooltipEventHandler(object sender, BeforeTooltipEventArgs e);


    public class BeforeTooltipEventArgs : EventArgs
    {
        public readonly Vector2 MousePosition;

        public BeforeTooltipEventArgs(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }


    public delegate void MouseClickEventHandler(object sender, MouseEventArgs e);


    public class MouseEventArgs : EventArgs
    {
        public readonly int MouseButton;
        public readonly Vector2 MousePosition;

        public MouseEventArgs(int mouseButton, Vector2 mousePosition)
        {
            MouseButton = mouseButton;
            MousePosition = mousePosition;
        }
    }

    public delegate void MouseDoubleClickEventHandler(object sender, MouseEventArgs e);


    public delegate void MouseDownEventHandler(object sender, MouseEventArgs e);


    public delegate void MouseUpEventHandler(object sender, MouseEventArgs e);


    public delegate void MouseHoldEventHandler(object sender, MouseEventArgs e);


    public delegate void MouseStartDragEventHandler(object sender, MouseDragEventArgs e);
    public delegate void MouseDragEventHandler(object sender, MouseDragEventArgs e);
    public delegate void ScrollEventHandler(object sender);

    public class MouseDragEventArgs : EventArgs
    {
        public readonly int MouseButton;
        public readonly Vector2 MousePosition;
        public readonly Vector2 PositionOffset;

        public MouseDragEventArgs(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
        {
            MouseButton = mouseButton;
            MousePosition = mousePosition;
            PositionOffset = positionOffset;
        }
    }

    public delegate void MouseEnterEventHandler(object sender, MouseMoveEventArgs e);


    /*
public class MouseEnterEventArgs : EventArgs
    {
    }
*/


    public delegate void MouseExitEventHandler(object sender, MouseMoveEventArgs e);


/*
        public class MouseExitEventArgs : EventArgs
        {
        }*/
    


    public delegate void MouseMoveEventHandler(object sender, MouseMoveEventArgs e);


    public class MouseMoveEventArgs : EventArgs
    {
        public readonly Vector2 MousePosition;

        public MouseMoveEventArgs(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }


    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);


    public class ValueChangedEventArgs : EventArgs
    {
        public readonly object Value;

        public ValueChangedEventArgs(object value)
        {
            Value = value;
        }
    }

    public delegate void ReturnKeyPressedHandler(object sender);


    public delegate void ControlOpenHandler(object sender);
    public delegate void ControlCloseHandler(object sender);


    public delegate void SelectionChangedEventHandler<T>(object sender, SelectionChangedEventArgs<T> e);

    public class SelectionChangedEventArgs<T> : EventArgs
    {
        public readonly T[] Selection;

        public SelectionChangedEventArgs(T[] selection)
        {
            Selection = selection;
        }

        public SelectionChangedEventArgs(T selection)
        {
            Selection = new T[1];
            Selection[0] = selection;
        }
    }

    public delegate void UnselectEventHandler<T>(object sender);

	public delegate void KeyPressedEventHandler(object sender, KeyPressedEventArgs e);


	public class KeyPressedEventArgs : EventArgs
	{
		public readonly KeyCode Code;
		public readonly char Character;
		public readonly bool Alt;
		public readonly bool Command;
		public readonly bool Control;
		public readonly bool Shift;

		public KeyPressedEventArgs(KeyCode code, char character, bool alt, bool command, bool control, bool shift)
		{
			Code = code;
			Character = character;
			Shift = shift;
			Control = control;
			Command = command;
			Alt = alt;
		}
	}


	public delegate void FocusEventHandler(object sender, FocusEventArgs e);


	public class FocusEventArgs : EventArgs
	{
	}


    public delegate void PositionCameraHandler(object sender, Camera camera);
}