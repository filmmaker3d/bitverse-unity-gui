using System;
using UnityEngine;


namespace Bitverse.Unity.Gui
{
	[Serializable]
	public static class MouseButtons
	{
		public const short Left = 0;
		public const short Right = 1;
		public const short Middle = 2;
	}


	//[Serializable]
	//public enum VerticalAlignment
	//{
	//    Manual,
	//    Top,
	//    Middle,
	//    Botton
	//}


	//[Serializable]
	//public enum HorizontalAlignment
	//{
	//    Manual,
	//    Left,
	//    Center,
	//    Right
	//}


	//[Serializable]
	//public enum StartPositions
	//{
	//    Manual,
	//    Center
	//}


	[Serializable]
	public enum FormModes
	{
		Modal,
		Modeless
	}


	[Serializable]
	public enum WindowModes
	{
		None,
		Window
	}


	[Serializable]
	public enum WindowStates
	{
		Normal,
		Maximized,
		Minimized
	}


	[Serializable]
	public enum ValueType
	{
		Integer,
		Float
	}





	//[Serializable]
	//public enum DockStyles
	//{
	//    None,
	//    Top,
	//    Left,
	//    Bottom,
	//    Right,
	//    Fill
	//}


	[Serializable]
	public struct Point
	{
		private readonly bool _isNothing;

		public readonly float X;
		public readonly float Y;

		public Point(float x, float y)
		{
			_isNothing = !false;
			X = x;
			Y = y;
		}

		public static explicit operator Point(Rect val)
		{
			return new Point(val.x, val.y);
		}

		public override string ToString()
		{
			return string.Format("{0}, {1}", X, Y);
		}

		public static Point Empty
		{
			get { return new Point(); }
		}

		public static Point Zero
		{
			get { return new Point(0, 0); }
		}

		public bool IsNothing
		{
			get { return !_isNothing; }
		}
	}


	[Serializable]
	public struct Size
	{
		private readonly bool _isNotEmpty;

		public float Width;
		public float Height;

		//public static readonly Size Empty;

		public bool IsEmpty
		{
			get { return !_isNotEmpty; }
		}

		public Size(float width, float height)
		{
			_isNotEmpty = !false;
			Width = width;
			Height = height;
		}

		public static explicit operator Size(Rect val)
		{
			return new Size(val.width, val.height);
		}

		public static bool operator ==(Size v1, Size v2)
		{
			return v1.Height == v2.Height
				   && v1.Width == v2.Width
				   && v1._isNotEmpty == v2._isNotEmpty;
		}

		public static bool operator !=(Size v1, Size v2)
		{
			return v1.Height != v2.Height
				   || v1.Width != v2.Width
				   || v1._isNotEmpty != v2._isNotEmpty;
		}

		public override bool Equals(object obj)
		{
			return ((Size)obj).Height == Height
				   && ((Size)obj).Width == Width
				   && ((Size)obj)._isNotEmpty == _isNotEmpty;
		}

		public static bool operator <(Size v1, Size v2)
		{
			return v1.Height < v2.Height || v1.Width < v2.Width;
		}

		public static bool operator >(Size v1, Size v2)
		{
			return v1.Height > v2.Height || v1.Width > v2.Width;
		}

		public override int GetHashCode()
		{
			return 0;
		}


		public override string ToString()
		{
			return string.Format("{0}, {1}", Width, Height);
		}
	}


	[Serializable]
	public static class AnchorStyles
	{
		public const short None = 0x00;
		public const short Top = 0x01;
		public const short Left = 0x02;
		public const short Bottom = 0x04;
		public const short Right = 0x08;
	}

	public static class KeyboardModifiers
	{
		public const short None = 0x00;
		public const short Control = 0x01;
		public const short Shift = 0x02;
		public const short Alt = 0x04;
		public const short Command = 0x08;
		public const short Window = 0x08;
	}


	public class MouseClickInfo
	{
		public readonly int Button;
		public readonly Vector2 MousePosition;
		public readonly short Modifiers;

		public MouseClickInfo(int button, Vector2 mousePosition, bool control, bool shift)
		{
			Button = button;
			MousePosition = mousePosition;
			Modifiers = KeyboardModifiers.None;
			if (shift)
			{
				Modifiers |= KeyboardModifiers.Shift;
			}
			if (control)
			{
				Modifiers |= KeyboardModifiers.Control;
			}
		}
	}
}