using System;
using System.Collections.Generic;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitWindow : BitContainer
{
    [SerializeField]
    public BitControl[] linkSource = new BitControl[0];
    [SerializeField]
    public BitControl[] linkTarget = new BitControl[0];
    [SerializeField]
    public BitScrollView[] linkScrollView = new BitScrollView[0];
    [SerializeField]
    public Color[] LinkColor = new Color[0];

    private bool _destroyMe;
    public Action<int> _destroyCallback;

    public delegate void GainedFocusHandler();
    public event GainedFocusHandler WindowGainedFocus;

    public bool DestroyMe
    {
        get { return _destroyMe; }
    }

    /// <summary>
    /// Whether the window is visible.
    /// </summary>
    public override bool Visible
    {
        get { return base.Visible; }
        set
        {
            bool changed = base.Visible != value;
            base.Visible = value;

            if (!changed)
            {
                return;
            }

            if (value)
            {
                BringToFront();
            }
            else if (TooltipManager != null)
            {
                TooltipManager.HideTooltip();
            }
        }
    }
    public bool IsTextFieldFocused;

    public void QueueDestroy(Action<int> callback)
    {
        _destroyMe = true;
        _destroyCallback = callback;
        Stage.FocusedWindowChanged -= RaiseWindowGainedFocus;
        RunBitAnimations(BitAnimationTrigger.OnClose);
    }

    #region Accessibility

    private int _windowID = ++WindowsCount;

    public int WindowID
    {
        get { return _windowID; }
    }

    #endregion


    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.window; }
    }

    #endregion


    #region Behaviour

    private static int WindowsCount;

    private Rect _viewPosition;

    public bool IsFocused
    {
        get
        {
            return Stage.FocusedWindow == this;
        }
    }

    private AbstractBitTextField _startFocusedField;

    public AbstractBitTextField StartFocusedField
    {
        get { return _startFocusedField; }
        set { _startFocusedField = value; }
    }

    [SerializeField]
    private FormModes _formMode = FormModes.Normal;

    public FormModes FormMode
    {
        get { return _formMode; }
        set { _formMode = value; }
    }

    [SerializeField]
    private WindowModes _windowMode = WindowModes.Window;

    public WindowModes WindowMode
    {
        get { return _windowMode; }
        set { _windowMode = value; }
    }

    [SerializeField]
    private bool _draggable = true;

    public bool Draggable
    {
        get { return _draggable; }
        set { _draggable = value; }
    }

    public Size ViewSize
    {
        get
        {
            if (IsInvalidated)
                LayoutItself();

            return new Size(_viewPosition.width, _viewPosition.height);
        }
    }

    public override void BringToFront()
    {
        Stage.BringWindowToFront(this);
    }

    public override void SendToBack()
    {
        Stage.SendWindowToBack(this);
    }

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        if (Event.current.type == EventType.Repaint && WindowMode == WindowModes.Window)
        {
            GUIStyle s = (Style ?? DefaultStyle);
            if (s != null)
                if (Event.current.type == EventType.Repaint)
                    s.Draw(Position, Content, IsHover, IsActive, IsFocused, Focus);
        }

        Matrix4x4 m = GUI.matrix;
        Matrix4x4 t = Matrix4x4.TRS(new Vector3(Position.x, Position.y, 0), Quaternion.identity,
                                    Vector3.one);
        GUI.matrix = t * m;
        try
        {
            GUI.color = Color;
            DoWindow();
            DrawLinks();
        }
        finally
        {
            GUI.matrix = m;
        }
    }

    private void DrawLinks()
    {
        if ((linkSource != null) && (linkSource.Length > 0))
        {
            for (int t = 0; t < linkSource.Length; t++)
            {
                BitScrollView parentScrollView = linkScrollView[t];
                if (parentScrollView == null)
                    continue;
                Rect abs = parentScrollView.AbsolutePosition;
                Rect target = linkTarget[t].AbsolutePosition;
                Rect source = linkSource[t].AbsolutePosition;
                GUIStyle sourceStyle = linkSource[t].Style ?? DefaultStyle;
                GUIStyle targetStyle = linkTarget[t].Style ?? DefaultStyle;
                Rect abs2 = new Rect(abs.x - Position.x , abs.y - Position.y , abs.width, abs.height);

                source = new Rect(source.x - sourceStyle.margin.left - abs.x - parentScrollView.ScrollPosition.x, source.y - sourceStyle.margin.top - abs.y - parentScrollView.ScrollPosition.y, source.width, source.height);
                target = new Rect(target.x - targetStyle.margin.left - abs.x - parentScrollView.ScrollPosition.x, target.y - targetStyle.margin.top - abs.y - parentScrollView.ScrollPosition.y, target.width, target.height);

                GUIHelper.BeginGroup(abs2);
                //TODO OPTIMIZE...
                Vector2 sp = new Vector2(source.x + source.width / 2, source.y + source.height / 2);
                Vector2 tp = new Vector2(target.x + target.width / 2, target.y + target.height / 2);

                GUIHelper.IntersectStruct responseS = GUIHelper.IntersectRectangle(sp, tp, source);
                GUIHelper.IntersectStruct responseT = GUIHelper.IntersectRectangle(sp, tp, target);

                GUIHelper.DrawLine(responseS.Point, responseT.Point, LinkColor[t]);
                //GUIHelper.DrawRect(source, UnityEngine.Color.green);
                //GUIHelper.DrawRect(target, UnityEngine.Color.magenta);
                GUIHelper.EndGroup();
            }
        }
    }

    private void DoWindow()
    {
        if (Event.current.type != EventType.Repaint && IsFocused)// && Event.current.type != EventType.Repaint)
        {
            if (_textFields == null)
            {
                _textFields = new List<AbstractBitTextField>();
            }
            
            _textFields.Clear();
            FindAllControls(_textFields);
       
            // TODO Only use TabIndex but for now keep position check when TabIndex are equal
            if(_comparison == null)
            {
                _comparison = new Comparison<AbstractBitTextField>(CompDelegate);
            }
            _textFields.Sort(_comparison);
            foreach (AbstractBitTextField textField in _textFields)
            {
                textField.ControlID = GUIUtility.GetControlID(textField.UniqueControlID, textField.GetFocusType());
                IsTextFieldFocused = textField.Focus;
                if (StartFocusedField == textField)
                {
                    textField.ForceFocus();
                    IsTextFieldFocused = true;

                    StartFocusedField = null;
                }
                //Debug.Log("TextField " + textField.name + " ControlID " + textField.ControlID + " with event " + Event.current.type);
            }
        }

        GUIClipPush(_viewPosition);
        DrawChildren();
        GUIClipPop();
    }

    private static int CompDelegate(AbstractBitTextField a, AbstractBitTextField b)
    {
        int r = a.TabIndex - b.TabIndex;

        if (r == 0)
        {
            bool beforeA = (a.AbsolutePosition.y + a.AbsolutePosition.height < b.AbsolutePosition.y);
            bool beforeB = (b.AbsolutePosition.y + b.AbsolutePosition.height < a.AbsolutePosition.y);
            return beforeA ? -1 : (beforeB ? 1 : (int)(a.AbsolutePosition.x - b.AbsolutePosition.x));
        }
        return r;
    }

    #endregion


    #region Draggable

    protected override void RaiseMouseDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        OnDrag(mouseButton, mousePosition, positionOffset);
        base.RaiseMouseDrag(mouseButton, mousePosition, positionOffset);
    }

    protected override void RaiseMouseStartDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        OnDrag(mouseButton, mousePosition, positionOffset);
        base.RaiseMouseStartDrag(mouseButton, mousePosition, positionOffset);
    }

    [SerializeField]
    private bool _keepInScreen = true;
    public bool KeepInScreen
    {
        get { return _keepInScreen; }
        set { _keepInScreen = value; }
    }

    [SerializeField]
    private float _hideTolerance = 0.3f;

    private List<AbstractBitTextField> _textFields;
    private static Comparison<AbstractBitTextField> _comparison;

    public float HideTolerance
    {
        get { return Mathf.Clamp01(_hideTolerance); }
        set { _hideTolerance = Mathf.Clamp01(value); }
    }


    public void OnDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        if (Draggable)
        {
            CenterVertical = false;
            CenterHorizontal = false;
        }

        if (Draggable && mouseButton == MouseButtons.Left)
        {
            Vector2 newPos = new Vector2(Position.x, Position.y) + positionOffset;

            if (KeepInScreen)
            {
                // Horizontal
                if (Position.width < Parent.Position.width)
                {
                    if ((newPos.x + (Position.width * (1.0f - HideTolerance))) > Parent.Position.width)
                    {
                        newPos.x = Parent.Position.width - (Position.width * (1.0f - HideTolerance));
                    }
                    else if (newPos.x < -(Position.width * HideTolerance))
                    {
                        newPos.x = -(Position.width * HideTolerance);
                    }
                }

                // Vertical
                if (Position.height < Parent.Position.height)
                {
                    if ((newPos.y + (Position.height * (1.0f - HideTolerance))) > Parent.Position.height)
                    {
                        newPos.y = Parent.Position.height - (Position.height * (1.0f - HideTolerance));
                    }
                    else if (newPos.y < -(Position.height * HideTolerance))
                    {
                        newPos.y = -(Position.height * HideTolerance);
                    }
                }
            }

            Position = new Rect(newPos.x, newPos.y, Position.width, Position.height);
        }
    }

    /*
    private bool isDragging;
        private Vector2 dragOffset;
	
        private void CheckStopDrag(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }
	
        private void CheckStartDrag(object sender, MouseEventArgs e)
        {
            MouseStatus ms = GetMouseStatus();
            if (ms.LeftButton.IsDown && _draggable && !Stage.DragManager.IsDragging && Event.current.button == MouseButtons.Left)
            //if (ms.MouseIsDown && _draggable && !Stage.DragManager.IsDragging && Event.current.button == MouseButtons.Left)
            {
                dragOffset = new Vector2(Event.current.mousePosition.x - Position.x, Event.current.mousePosition.y - Position.y);
				
                isDragging = true;
            }
        }
	
        private void MeuMouseMove(object sender, MouseMoveEventArgs e)
        {
            if (isDragging && !Stage.DragManager.IsDragging)
            {
                Position = new Rect(Event.current.mousePosition.x - dragOffset.x, Event.current.mousePosition.y - dragOffset.y, Position.width, Position.height);
            }
        }*/

    #endregion


    #region Layout

    protected override void LayoutItself()
    {
        base.LayoutItself();
        CalculateViewRect();
    }

    internal override void LayoutChildren()
    {
        base.LayoutChildren();
        CalculateViewRect();
    }

    private void CalculateViewRect()
    {
        GUIStyle s = Style ?? DefaultStyle;

        _viewPosition = new Rect
            (
            s.padding.left,
            s.padding.top,
            Size.Width - s.padding.horizontal,
            Size.Height - s.padding.vertical
            );
    }

    #endregion


    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();
        _windowID = ++WindowsCount;
    }

    public override void Start()
    {
        base.Start();
        Stage.FocusedWindowChanged += RaiseWindowGainedFocus;
    }

    private void RaiseWindowGainedFocus(int controlID)
    {
        if(controlID != ControlID)
            return;
        if(WindowGainedFocus != null)
            WindowGainedFocus();        
    }

    #endregion
}
