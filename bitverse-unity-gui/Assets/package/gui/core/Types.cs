using System;
using UnityEngine;

namespace Bitverse.Unity.Gui
{
    [System.Serializable]
    public enum MouseButtons
    {
        Left = 0,
        Middle = 1,
        Right = 2
    }

    [System.Serializable]
    public enum VerticalAlignment
    {
        Manual,
        Top,
        Middle,
        Botton
    }

    [System.Serializable]
    public enum HorizontalAlignment
    {
        Manual,
        Left,
        Center,
        Right
    }


    [System.Serializable]
    public enum FormModes
    {
        Modal,
        Modeless
    }

    [System.Serializable]
    public enum WindowModes
    {
        None,
        Window
    }

    [System.Serializable]
    public enum WindowStates
    {
        Normal,
        Maximized,
        Minimized
    }

    [System.Serializable]
    public enum StartPositions
    {
        Manual,
        Center
    }

    [System.Serializable]
    public enum ValueType
    {
        Integer,
        Float
    }

    [System.Serializable]
    public struct Point
    {
        private bool _isNothing;

        public float X;
        public float Y;

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
            return string.Format ("{0}, {1}", X, Y);
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

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct Size
    {
        private bool _isNotEmpty ;

        public float Width;
        public float Height;

        public static readonly Size Empty;

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

        public static bool operator < (Size v1, Size v2)
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

}
