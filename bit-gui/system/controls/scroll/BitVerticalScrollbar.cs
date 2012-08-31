using Bitverse.Unity.Gui;
using UnityEngine;
using ValueType = Bitverse.Unity.Gui.ValueType;

/// <summary>
/// Button Up
/// VerticalScrollbar:
///     - Position
///     - Value
///     - Size
///     - Top
///     - Bottom
///     - Style
/// Button Down
/// </summary>
public class BitVerticalScrollbar : BitControl
{
    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.verticalScrollbar; }
    }

    #endregion


    #region Behaviour

    [SerializeField]
    private ValueType _valueType = ValueType.Float;

    [SerializeField]
    private float _visibleSize = 1;

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

    #endregion


    #region Data

    [SerializeField]
    private float _botton;

    [SerializeField]
    private float _top = 200;

    [SerializeField]
    private float _value;

    public float Value
    {
        get { return _value; }
        set
        {
            //sanity
            if (!InvertScrollNavigation())
            {
                if (value < Bottom || value >= Top)
                    return;
            }
            else
            {
                if (value < Top || value >= Bottom)
                    return;
            }

            if (_value != value)
            {
                _value = value;
                RaiseValueChangedEvent(value);
                RaiseScroll();
                //TKDAudioSource.Play(AudioConstants.ScrollChanged);
            }
        }
    }

    public float Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public float Bottom
    {
        get { return _botton; }
        set { _botton = value; }
    }

    private const int ButtonSide = 16;

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        var value = Value;

        if (Parent != null && Event.current.type == EventType.ScrollWheel)
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
            if (Parent.AbsolutePosition.Contains(mousePos))
            {
                if (!InvertScrollNavigation())
                    value += (Event.current.delta.y < 0) ? VisibleSize : -VisibleSize;
                else
                    value += (Event.current.delta.y < 0) ? -VisibleSize : VisibleSize;
            }
        }

        var position = new Rect(Position.x, Position.y, Position.width, Position.height);
        FixSkinThumb(position);

        //unity scroll up button has wrong behaviour
        HideSkinButtonUp();
        var buttonUpClicked = GUI.Button(GetButtonUpPosition(position), "", GetButtonUpStyle());
        if (buttonUpClicked)
            value = (!InvertScrollNavigation()) ? value + VisibleSize : value - VisibleSize;

        value = GUI.VerticalScrollbar(
            position,
            ValueType == ValueType.Float ? value : (int)value,
            VisibleSize,
            Top,
            Bottom,
            Style ?? DefaultStyle);

        //unity scroll down button has wrong behaviour
        HideSkinButtonDown();
        var buttonDownClicked = GUI.Button(GetButtonDownPosition(position), "", GetButtonDownStyle());
        if (buttonDownClicked)
            value = (!InvertScrollNavigation()) ? value - VisibleSize : value + VisibleSize;

        if (value != Value)
            Value = value;
    }

    private static GUIStyle GetButtonUpStyle()
    {
        return (GUI.skin == null) ? GUIStyle.none : GUI.skin.verticalScrollbarUpButton.name;
    }

    private static GUIStyle GetButtonDownStyle()
    {
        return (GUI.skin == null) ? GUIStyle.none : GUI.skin.verticalScrollbarDownButton.name;
    }

    private static void FixSkinThumb(Rect position)
    {
        if (GUI.skin == null || GUI.skin.verticalScrollbarThumb == null)
            return;

        //unity scroll thumb should not have fixed positions
        GUI.skin.verticalScrollbar.fixedWidth = 0;
        GUI.skin.verticalScrollbar.fixedHeight = 0;
        GUI.skin.verticalScrollbarThumb.fixedWidth = position.width-2;
        GUI.skin.verticalScrollbarThumb.fixedHeight = 0;
    }

    private static void HideSkinButtonUp()
    {
        if (GUI.skin == null || GUI.skin.verticalScrollbarUpButton == null)
            return;

        GUI.skin.verticalScrollbarUpButton.fixedWidth = 0;
        GUI.skin.verticalScrollbarUpButton.fixedHeight = 0;
    }

    private static void HideSkinButtonDown()
    {
        if (GUI.skin == null || GUI.skin.verticalScrollbarDownButton == null)
            return;

        GUI.skin.verticalScrollbarDownButton.fixedWidth = 0;
        GUI.skin.verticalScrollbarDownButton.fixedHeight = 0;
    }

    private static Rect GetButtonUpPosition(Rect position)
    {
        return new Rect(position.xMin, position.yMin - ButtonSide, position.width, ButtonSide);
    }

    private static Rect GetButtonDownPosition(Rect position)
    {
        return new Rect(position.xMin, position.yMin + position.height, position.width, ButtonSide);
    }

    private bool InvertScrollNavigation()
    {
        return Bottom > Top;
    }

    #endregion


    #region Events

    public event ValueChangedEventHandler ValueChanged;

    private void RaiseValueChangedEvent(float value)
    {
        if (ValueChanged == null)
        {
            return;
        }
        ValueChanged(this, new ValueChangedEventArgs(value));
    }

    #endregion
}