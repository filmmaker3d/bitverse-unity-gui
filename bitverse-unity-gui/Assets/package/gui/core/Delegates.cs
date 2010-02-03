using System;


namespace Bitverse.Unity.Gui
{
	public delegate void InvalidatedEventHandler(object sender, InvalidatedEventArgs e);

	public class InvalidatedEventArgs
	{

	}


	public delegate void MouseClickEventHandler(object sender, MouseClickEventArgs e);


	public class MouseClickEventArgs : EventArgs
	{
		public readonly int MouseButton;

		public MouseClickEventArgs(int mouseButton)
		{
			MouseButton = mouseButton;
		}
	}


	//public delegate void MouseDragEventHandler(object sender, MouseDragEventArgs e);

	//public class MouseDragEventArgs : EventArgs
	//{

	//}


	//public delegate void MouseDownEventHandler(object sender, MouseDownEventArgs e);


	//public class MouseDownEventArgs : EventArgs
	//{
	//}


	//public delegate void MouseUpEventHandler(object sender, MouseUpEventArgs e);


	//public class MouseUpEventArgs : EventArgs
	//{
	//}


	public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);


	public class ValueChangedEventArgs : EventArgs
	{
		public readonly object Value;

		public ValueChangedEventArgs(object value)
		{
			Value = value;
		}
	}

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
}