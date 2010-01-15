using UnityEngine;
using Bitverse.Unity.Gui;


    public class BitHorizontalScrollbar : BitControl
    {
        public event ValueChangedEventHandler ValueChanged;

        [SerializeField]
        private float _value = 0;
        [SerializeField]
        private ValueType _valueType = ValueType.Float;
        [SerializeField]
        private float _visibleSize = 0;
        [SerializeField]
        private float _right = 100;
        [SerializeField]
        private float _left = 0;

        public float Value
        {
            get { return _value; }
            set 
            { 
                _value = value;
                if (ValueChanged != null) ValueChanged(this, new ValueChangedEventArgs(_value));
            }
        }

        public ValueType ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        public float VisibleSize
        {
            get { return _visibleSize; }
            set { _visibleSize = value; }
        }

        public float RightValue
        {
            get { return _right; }
            set { _right = value; }
        }

        public float LeftValue
        {
            get { return _left; }
            set { _left = value; }
        }

        public override void DoDraw()
        {
            float val;
            if (ValueType == ValueType.Float)
            {
                if (Style != null)
                {
                    val = UnityEngine.GUI.HorizontalScrollbar(Position, Value, VisibleSize, LeftValue, RightValue, Style);
                }
                else
                {
                    val = UnityEngine.GUI.HorizontalScrollbar(Position, Value, VisibleSize, LeftValue, RightValue);
                }
            }
            else
            {
                if (Style != null)
                {
                    val = UnityEngine.GUI.HorizontalScrollbar(Position, (int)Value, VisibleSize, LeftValue, RightValue, Style);
                }
                else
                {
                    val = UnityEngine.GUI.HorizontalScrollbar(Position, (int)Value, VisibleSize, LeftValue, RightValue);
                }
            }

            if (val != Value)
            {
                Value = val;
            }
        }

        
    }
