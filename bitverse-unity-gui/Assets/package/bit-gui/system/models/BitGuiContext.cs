using System;
using System.Collections.Generic;
using UnityEngine;

namespace bitgui
{
    public class BitGuiContext : IDisposable 
    {
        [ThreadStatic]
        private static List<BitGuiContext> _contexts;
        [ThreadStatic]
        private static int _top;


        private BitControl _component;
        private BitControl _renderer;
        private object _data;
        private int _index;
        private bool _selected;
        private int _topPosition;
        private int _pathHash;


        public bool Selected
        {
            get { return _selected; }
        }

        public int Index
        {
            get { return _index; }
        }

        public object Data
        {
            get { return _data; }
        }

        public BitControl Renderer
        {
            get { return _renderer; }
        }

        public BitControl Component
        {
            get { return _component; }
        }

        public int PathHash
        {
            get { return _pathHash; }
        }

        public static BitGuiContext Current
        {
            get
            {
                InitCheck();
                try
                {
                    return _contexts[_top];
                } catch(Exception)
                {
                    throw new Exception("top="+_top);
                }
            }
        }

        private static void InitCheck()
        {
            if(_contexts == null)
            {
                _contexts = new List<BitGuiContext>();
            }
            while (_top >= _contexts.Count)
            {
                BitGuiContext context = new BitGuiContext();
                context._topPosition = _contexts.Count;
                _contexts.Add(context);
            }
        }


        public static BitGuiContext Push(BitControl list, BitControl renderer, object data, int i, bool selected)
        {
            InitCheck();
            _top = _top + 1;
            BitGuiContext context = Current;
            context._component = list;
            context._renderer = renderer;
            context._data = data;
            context._index = i;
            context._selected = selected;
            context._pathHash = _contexts[_top - 1].PathHash * 397 + i;
            return context;
        }

        public void Dispose()
        {
            // GC friendly
            _component = null;
            _renderer = null;
            _data = null;
            _index = -1;
            _selected = false;
            // not removing from the _contexts list to minimize object creation next time.
            if (_top == _topPosition)
                _top = _topPosition - 1;
        }
    }
}
