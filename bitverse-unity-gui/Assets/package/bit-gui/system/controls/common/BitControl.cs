using System;
using System.Collections.Generic;
using bitgui;
using Bitverse.Unity.Gui;
using UnityEngine;

public enum TextKind
{
    NONE,
    STATIC_TEXT,
    DINAMIC_TEXT,
    IGNORE,
    FLOAT,
    INT
}

/// <summary>
/// Base class for all controls.
/// </summary>
public abstract partial class BitControl : MonoBehaviour
{

    // TODO Use this for Focus Control in future (implementation in BitWindow)
    [HideInInspector]
    public int TabIndex;
    [HideInInspector]
    public bool TabStop;

    private static int unid;
    [HideInInspector]
    public int UniqueControlID = unid++;

    private bool _forceFocus;
    private int _forceFocusCount;

    public void ForceFocus()
    {
        _forceFocusCount = 2;
        _forceFocus = true;
    }

    protected static EventType OriginalEvent;

    private void GainFocusIfForced()
    {
        // When I first click in a component which is read in MouseDown Event
        // Then we set this component as Focused for the next two Events Layout and Repaing.
        if (_forceFocus)
        {
            if (_forceFocusCount > 0) _forceFocus = false;
            _forceFocusCount--;
            if (FocusType.Keyboard == GetFocusType())
            {
                GUIUtility.keyboardControl = ControlID;
            }
        }
    }

    #region Accessibility

    private Guid _id;

    public string _idToString = Guid.NewGuid().ToString();

    /// <summary>
    /// Control ID.
    /// </summary>
    public Guid ID
    {
        get
        {
            if (_id == Guid.Empty)
            {
                try
                {
                    _id = new Guid(_idToString);
                }
                catch
                {
                    _id = Guid.NewGuid();
                    _idToString = _id.ToString();
                }
            }

            return _id;
        }

        set
        {
            _id = value;
            _idToString = _id.ToString();
        }
    }

    private int _controlID;

    public int ControlID
    {
        get { return _controlID; }
        set { _controlID = value; }
    }

    #endregion


    #region Animation

    #region Animation Parameters

    // http://www.cis.sojo-u.ac.jp/~izumi/Unity_Documentation_jp/Documentation/Components/animeditor-AnimationCurves.html
    //The following types of properties are supported in the animation system: 
    //Float 
    //Color 
    //Vector2 
    //Vector3 
    //Vector4 
    //Quaternion
    // note that Arrays are not supported and neither are structs or objects other than the ones listed above. 

    [HideInInspector]
    [SerializeField]
    public Rect AnimationPosition;

    [HideInInspector]
    [SerializeField]
    public Color AnimationColor;

    [HideInInspector]
    [SerializeField]
    public Vector2 AnimationScale;

    [HideInInspector]
    [SerializeField]
    public Vector2 AnimationScalePivot;

    [HideInInspector]
    [SerializeField]
    public float AnimationRotationAngle;

    [HideInInspector]
    [SerializeField]
    public Vector2 AnimationRotationPivot;

    [HideInInspector]
    [SerializeField]
    public bool Animating;

    private bool IsAnimating
    {
        get { return Animating; } // && EditMode; }
        set { Animating = value; }
    }

    private bool _isPlaying;


    private struct BackupAnimation
    {
        public Rect Position;
        public Color Color;

        public Vector2 Scale;
        public Vector2 ScalePivot;

        public float RotationAngle;
        public Vector2 RotationPivot;
    }

    private void Backup()
    {
        _backup.Position = Position;
        _backup.Color = Color;

        _backup.Scale = Scale;
        _backup.ScalePivot = ScalePivot;

        _backup.RotationAngle = RotationAngle;
        _backup.RotationPivot = RotationPivot;
    }

    private void RecoverFromBackup()
    {
        Position = _backup.Position;
        Color = _backup.Color;

        Scale = _backup.Scale;
        ScalePivot = _backup.ScalePivot;

        RotationAngle = _backup.RotationAngle;
        RotationPivot = _backup.RotationPivot;
    }

    private void ResetAnimation()
    {
        AnimationPosition = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
        AnimationColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        AnimationScale = new Vector2(0.0f, 0.0f);
        AnimationScalePivot = new Vector2(0.0f, 0.0f);

        AnimationRotationAngle = 0.0f;
        AnimationRotationPivot = new Vector2(0.0f, 0.0f);
    }

    private BackupAnimation _backup;


    #endregion


    [SerializeField]
    private BitAnimationEvent[] _animationEvents = { };

    //Runs all the animations registered for given trigger
    protected void RunBitAnimations(BitAnimationTrigger trigger)
    {
        if (controlAnimation != null && controlAnimation.isPlaying)
            return;
        if (_animationEvents == null || _animationEvents.Length == 0)
            return;

        foreach (BitAnimationEvent be in _animationEvents)
        {
            if (be.Trigger == trigger)
            {
                if (controlAnimation == null)
                    gameObject.AddComponent(new Animation().GetType());

                if (controlAnimation.GetClip(be.ClipName) == null)
                {
                    controlAnimation.AddClip(be.Animation.GetClip(be.ClipName), be.ClipName);
                }

                controlAnimation.Play(be.ClipName);
                _isPlaying = true;
            }
        }
    }

    #endregion


    #region Appearance

    private static GUIStyle _emptyStyle;

    public static GUIStyle EmptyStyle
    {
        get
        {
            if (_emptyStyle == null)
            {
                _emptyStyle = GUIStyle.none;
                _emptyStyle.name = "Empty Style";
            }
            return _emptyStyle;
        }
    }

    [SerializeField]
    private Color _color = Color.white;

    public virtual Color Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public void SetColor(Color c)
    {
        _color = c;
    }

    [SerializeField]
    private GUISkin _skin;

    /// <summary>
    /// Skin associated with the Control. If there is no Skin associated, returns Unity default skin.
    /// When set, a re-layout is executed.
    /// </summary>
    public GUISkin Skin
    {
        get { return _skin ?? GUI.skin; }
        set
        {
            _skin = value;
            _style = null;
            PerformLayoutItself();
        }
    }

    [SerializeField]
    private string _styleName;

    public string StyleName
    {
        get { return _styleName; }
        set
        {
            _styleName = value;
            // clear the cached style
            _style = null;
            _styleSkin = null;
        }
    }

    private GUIStyle _style;
    private GUISkin _styleSkin;

    public GUIStyle Style
    {
        get
        {
            GUISkin s = Skin;
            if (s == null)
                return _style = null;
            if (_styleSkin == s)
                return _style;
            _styleSkin = s;
            if (!string.IsNullOrEmpty(_styleName))
                return _style = s.FindStyle(_styleName);
            return _style = null;
        }
        set { _style = value; }
    }

    //TODO Test this!!!
    public virtual GUIStyle DefaultStyle
    {
        get { return EmptyStyle; }
    }

    #endregion


    #region Behaviour

    // Parent disabled
    private bool _dirtyList;

    [HideInInspector]
    [SerializeField]
    private bool _disabled;

    /// <summary>
    /// Whether the component is disabled. This is set to propagate the state to its children. If the parent is disabled, all its
    /// children are disabled too. If the parent is not disabled, then its children can be <see cref="Enabled"/>.
    /// </summary>
    public bool Disabled
    {
        get { return _disabled; }
        set { _disabled = value; }
    }

    [SerializeField]
    private bool _enabled = true;

    /// <summary>
    /// Whether the component is enabled. This property is not propagated to its children.
    /// </summary>
    public bool Enabled
    {
        get
        {
            bool val = false;
            if (Parent == null)
            {
                val = !_disabled ? _enabled : false;
            }
            else if (Parent.Enabled || Parent.KeepEnabled)
            {
                val = !_disabled ? _enabled : false;
            }
            return val || KeepEnabled;
        }

        set { _enabled = value; }
    }

    [SerializeField]
    private bool _mouseEnabled = true;
    /// <summary>
    /// Checks if mouse can consume events.
    /// </summary>
    public virtual bool MouseEnabled
    {
        get { return _mouseEnabled; }
        set
        {
            _mouseEnabled = value;
//            foreach (KeyValuePair<string, object> o in UserProperties)
            {

            }
        }
    }

    [HideInInspector]
    public bool KeepEnabled;

    private bool _invalidated = true;

    /// <summary>
    /// [Read-only] Whether the Control is invalidated.
    /// </summary>
    protected bool IsInvalidated
    {
        get { return _invalidated; }
    }

    private BitControl _parent;

    private bool _isFather;

    /// <summary>
    /// Control's parent.
    /// </summary>
    public BitControl Parent
    {
        get
        {
            try
            {
                if (_parent == null && !_isFather)
                {
                    _parent = (transform.parent != null ? transform.parent.GetComponent<BitControl>() : null);
                    if (_parent == null)
                        _isFather = true;
                }
                return _parent;
            }
            catch (Exception) { BitStage.LogError("no transform for: " + this.GetType().Name); return null; }
        }
        set
        {
            _parent = value;
            if (value != null)
            {
                transform.parent = value.transform;
            }
        }
    }

    private BitWindow _topWindow;

    /// <summary>
    /// The top-most parent.
    /// </summary>
    public BitWindow TopWindow
    {
        get
        {
            if (Application.isPlaying && _topWindow != null)
                return _topWindow;
            // THIS IS SPARTAAA!!!
            if (this is BitWindow)
            {
                _topWindow = (BitWindow)this;
            }
            else if (Parent != null)
            {
                _topWindow = Parent.TopWindow;
            }
            else
            {
                _topWindow = null;
            }

            return _topWindow;
        }
    }

    private BitStage _stage;

    public BitStage Stage
    {
        get
        {
            if (Application.isPlaying && _stage != null)
                return _stage;

            if (TopWindow == null)
            {
                // When a BitStage without a parent Window asks for his BitStage it returns itself. correct?
                if (this is BitStage)
                {
                    return (BitStage)this;
                }
                if (BitStage.Current != null)
                    return BitStage.Current;
                BitStage.LogError(GetType().Name);
                throw new Exception("cant find Stage without a window");
            }
            return _stage = TopWindow.transform.parent.GetComponent<BitStage>();
        }
    }

    private TooltipManager _tooltipManager;

    public TooltipManager TooltipManager
    {
        get
        {
            if (_tooltipManager == null)
            {
                _tooltipManager = (TooltipManager)FindObjectOfType(typeof(TooltipManager));
            }

            return _tooltipManager;
        }
    }

    [SerializeField]
    private bool _visible = true;

    // TODO See if this is necessary
    //private StartPositions _startPosition = StartPositions.Manual;

    /// <summary>
    /// Whether the Control is visible.
    /// </summary>
    public virtual bool Visible
    {
        get { return _visible; }
        set
        {
            bool changed = _visible != value;
            _visible = value;

            if (changed)
            {
                if (_visible)
                {
                    //RaiseControlOpen();
                    RunBitAnimations(BitAnimationTrigger.OnOpen);
                }
                else
                {
                    RunBitAnimations(BitAnimationTrigger.OnClose);
                }
            }
        }
    }

    private bool _supressNextDraw;

    public bool SupressNextDraw
    {
        get
        {
            if (_supressNextDraw)
            {
                _supressNextDraw = false;
                return true;
            }

            return false;
        }
        set { _supressNextDraw = value; }
    }

    private bool _isOpen;
    public bool IsOpen
    {
        get { return _isOpen; }
        set { _isOpen = value; }
    }

    public bool IsVisible()
    {
        if (this is BitWindow)
            return Visible;
        if (!Visible)
            return false;
        return Parent.IsVisible();
    }

    public override bool Equals(object o)
    {
        if (!(o is BitControl))
        {
            return false;
        }
        return ((BitControl)o).ID == _id;
    }


    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    public string TooltipProviderName;


    private bool _showTooltip;

    public bool ShowTooltip
    {
        set { _showTooltip = value; }
    }


    private object _tooltipData;

    public object TooltipData
    {
        set { _tooltipData = value; }
    }


    private bool _controlHover;

    public bool IsControlHover
    {
        get { return _controlHover; }
        set { _controlHover = value; }
    }

    #endregion


    #region Data

    [SerializeField]
    private GUIContent _content = new GUIContent();

    /// <summary>
    /// [Read-only] Gets the Control GUIContent.
    /// </summary>
    public GUIContent Content
    {
        get { return _content; }

        set { _content = value; }
    }

    [SerializeField]
    public TextKind textKind = TextKind.NONE;

    public virtual string Text
    {
        get { return _content.text; }
        set
        {
            if (value == null)
            {
                //Log.Error("Invalid value for text: null.");
                _content.text = "[NULL]";
                return;
            }
            _content.text = value;
        }
    }

    public virtual string TextVisibleIfNotNullOrEmpty
    {
        get { return Text; }
        set
        {
            Text = value;
            Visible = (!String.IsNullOrEmpty(value));
        }
    }

    public virtual Texture Image
    {
        get { return _content.image; }
        set { _content.image = value; }
    }

    //TODO The right thing here is make a IPopup field, but if I make this, Unity don't show this field.
    [SerializeField]
    private AbstractBitPopup _contextMenu;

    /// <remarks>
    /// DO NOT USE THIS. THIS CAUSES THE MENU TO BE SHOWN AGAIN WITHOUT BEING REFRESHED.
    /// </remarks>
    [Obsolete]
    public AbstractBitPopup ContextMenu
    {
        get { return _contextMenu; }
        set { _contextMenu = value; }
    }

    [HideInInspector]
    [SerializeField]
    private bool _canShowContextMenu = true;

    public bool CanShowContextMenu
    {
        get { return _canShowContextMenu; }
        set { _canShowContextMenu = value; }
    }

    [SerializeField]
    private Dictionary<string, object> _userProperties;

    public Dictionary<string, object> UserProperties
    {
        get
        {
            if (_userProperties == null)
            {
                _userProperties = new Dictionary<string, object>();
            }

            return _userProperties;
        }
        set
        {
            // Use this carefully so no old values are lost.
            _userProperties = value;
        }
    }

    #endregion


    #region Draw

    private static bool _isHover;

    /// <summary>
    /// Whether mouse pointer is over any Control on the GUI (note that this is static).
    /// </summary>
    public static bool IsHover
    {
        get { return _isHover; }
        internal set { _isHover = value; }
    }

    private bool _lastFrameWasHover;
    public bool LastFrameWasHover
    {
        get { return _lastFrameWasHover; }
    }

    private bool _forceSelectedState;

    public bool ForceOnState
    {
        get { return _forceSelectedState; }
        set { _forceSelectedState = value; }
    }

    private static bool _isOn;

    /// <summary>
    /// Whether the control is on (selected).
    /// </summary>
    public static bool IsOn
    {
        get { return _isOn; }
        internal set { _isOn = value; }
    }

    private static bool _isActive;

    /// <summary>
    /// Whether the mouse is down over the Control.
    /// </summary>
    public static bool IsActive
    {
        get { return _isActive; }
        internal set { _isActive = value; }
    }

    [HideInInspector]
    public bool LeftButton = true;
    [HideInInspector]
    public bool MiddleButton = true;
    [HideInInspector]
    public bool RightButton = true;

    [SerializeField]
    private bool _autoSize;

    public bool AutoSize
    {
        get { return _autoSize; }
        set { _autoSize = value; }
    }

    public virtual FocusType FocusType
    {
        get { return FocusType.Passive; }
    }

    public virtual FocusType GetFocusType()
    {
        return FocusType;
    }

    // HACK
    private Point _tooltipPosition;
    private bool _repositionTooltip;

    public void ShowAsTooltip(Point position)
    {
        _tooltipPosition = position;
        _repositionTooltip = true;
    }

    private void ShowAsTooltipIfShould()
    {
        if (_repositionTooltip)
        {
            RecursiveAutoSize();
            bool bottomPass = Position.height + _tooltipPosition.Y > Screen.height;
            bool rightPass = Position.width + _tooltipPosition.X > Screen.width;

            // Make sure the tooltip does not show out of the screen.
            float x = _tooltipPosition.X;
            float y = _tooltipPosition.Y;

            if (bottomPass && rightPass)
            {
                x = x - Position.width - 2;
                y = y - Position.height - 2;
            }
            else if (bottomPass)
            {
                y = Screen.height - Position.height;
            }
            else if (rightPass)
            {
                x = Screen.width - Position.width;
            }

            Location = new Point(x, y);

            _repositionTooltip = false;
        }
    }



    /// <summary>
    /// Draws the Control.
    /// </summary>
    internal void Draw()
    {
        Backup();

        if (IsAnimating || (controlAnimation != null && controlAnimation.isPlaying))
        {
            Position = new Rect(Position.x + AnimationPosition.x,
                                Position.y + AnimationPosition.y,
                                Position.width + AnimationPosition.width,
                                Position.height + AnimationPosition.height);
            Color += AnimationColor;

            Scale += AnimationScale;
            ScalePivot += AnimationScalePivot;

            RotationAngle += AnimationRotationAngle;
            RotationPivot += AnimationRotationPivot;
        }
        else if (_isPlaying)
        {
            _isPlaying = false;

            // Reset animation parameters because que animation.Play sometimes does not play with last frame
            // which can cause some flicker with posterior animation play call
            ResetAnimation();
        }

        if (!Visible && !(controlAnimation != null && controlAnimation.isPlaying))
        {
            if (IsOpen) RaiseControlClose();
            return;
        }

        if (Visible && !IsOpen && !(controlAnimation != null && controlAnimation.isPlaying))
        {
            RaiseControlOpen();
        }

        if (_position.width <= 0 || _position.height <= 0)
        {
            DoAutoSize();
            DoLayout();
            return;
        }

        GUISkin oldSkin = GUI.skin;
        GUI.skin = Skin;
        GUI.color = _color;
        GUI.SetNextControlName(_idToString);
        GUI.enabled = Enabled;

        ChangeModalColor(); //recent change, probably will conflict with animation color (currently deprecated)

        // HACKs
        ShowAsTooltipIfShould();

        // Rotates Matrix
        Matrix4x4 backupMatrix = GUI.matrix;
        // note: for some reason when one of the scale factors is zero it does not work
        // TODO: verify this error, matrix becomes not invertible
        if ((Scale.x != 0 && Scale.y != 0) && (Scale.x != 1.0f || Scale.y != 1.0f))
            GUIUtility.ScaleAroundPivot(Scale, RenderPivot(ScalePivot));
        if (RotationAngle != 0)
            GUIUtility.RotateAroundPivot(RotationAngle, RenderPivot(RotationPivot));

        SecureAutoSize();

        //Stop layout while there is an animation running
        if (controlAnimation == null || !controlAnimation.isPlaying)
        {
            DoLayout();
        }

        DoCentralizeIfPossible();

        if (!SupressNextDraw)
        {
            GainFocusIfForced();
            InternalUserEventsBeforeDraw();
            DoDraw();
            InternalUserEventsAfterDraw();
        }

        InternalUserEvents();

        // Restore Matrix
        GUI.matrix = backupMatrix;

        GUI.skin = oldSkin;

        if (IsAnimating || (controlAnimation != null && controlAnimation.isPlaying))
        {
            RecoverFromBackup();
        }

        if ((!_showTooltip) || (!_lastFrameWasHover) || ((_tooltipData != null) && (_tooltipData != BitGuiContext.Current.Data)))
        {
            return;
        }

        if ((!string.IsNullOrEmpty(TooltipProviderName)) || (!string.IsNullOrEmpty(Content.tooltip)))
        {
            if (TooltipManager != null)
            {
                // Raise before tooltip to set up user properties if needed.
                RaiseBeforeTooltip(Event.current.mousePosition);

                // Try to get the current user properties from BitGuiContext.
                ITooltipData tooltipData = BitGuiContext.Current.Data as ITooltipData;

                if ((tooltipData != null) && (tooltipData.TooltipProperties != null))
                {
                    UserProperties = tooltipData.TooltipProperties;
                }

                TooltipManager.ShowTooltip(this, new Point(Input.mousePosition.x + 2, Screen.height - Input.mousePosition.y + 2));
            }
        }

        _showTooltip = false;
    }

    private void ChangeModalColor()
    {
        if (!Enabled)
        {
            GUI.color = Stage.ModalColor;
        }
        else
        {
            GUI.color = Color;
        }
    }

    private void InternalUserEventsAfterDraw()
    {
        if (Event.current.type == EventType.Used || !MouseEnabled)
        {
            return;
        }
        bool consume = false;
        MouseStatus ms = GetMouseStatus();

        switch (Event.current.type)
        {
            case EventType.MouseDown:
                {
                    MouseButtonStatus mbs;
                    GetMouseButtonStatus(ms, out mbs);

                    if (!mbs.IsDown && ms.IsHover)
                    {
                        mbs.IsDown = true;
                        mbs.MouseDownPosition = Event.current.mousePosition;
                        mbs.IsDragging = false;

                        SetMouseButtonStatus(ref ms, mbs);
                        SaveMouseStatus(ms);

                        RaiseMouseDown(Event.current.button, Event.current.mousePosition);

                        if (Event.current.clickCount >= 2)
                        {
                            RaiseMouseDoubleClick(Event.current.button, Event.current.mousePosition);
                        }
                        consume = true;
                    }
                }
                break;
            case EventType.MouseUp:
            case EventType.Ignore:  // When calling GUIClip.Push the MouseUP events becomes Ignore if mouse position is outside clip area
                // But we still want to treat those events to release our mouse control internal structure
                {
                    if (Event.current.type == EventType.Ignore && OriginalEvent != EventType.MouseUp)
                        break;
                    MouseButtonStatus mbs;
                    GetMouseButtonStatus(ms, out mbs);

                    if (mbs.IsDown)
                    {
                        mbs.IsDown = false;

                        SetMouseButtonStatus(ref ms, mbs);
                        SaveMouseStatus(ms);

                        RaiseMouseUp(Event.current.button, Event.current.mousePosition);

                        if (ms.IsHover)
                        {
                            RaiseMouseClick(Event.current.button, Event.current.mousePosition);
                        }
                    }

                }
                break;
            case EventType.MouseDrag:
                {
                }
                break;

        }

        if (TopWindow.Enabled && (consume && ConsumeEvent(Event.current.type)))
        {
            Event.current.Use();
        }
    }


    /// <summary>
    /// Called at the end of <see cref="Draw"/> method.
    /// </summary>
    protected abstract void DoDraw();


    //TODO i'm not sure that this is necessary
    //protected void DrawTiles(GUIStyle Style, bool relative)
    //{
    //    if ((Event.current._type != EventType.Repaint) || (Style == null))
    //    {
    //        return;
    //    }

    //    Rect position = Position;

    //    float xOffset = relative ? 0 : position.x;
    //    float yOffset = relative ? 0 : position.y;

    //    RectOffset border = Style.border;

    //    if (_centerTile != null)
    //    {
    //        DrawTiles(_centerTile, new Rect(xOffset + border.left, yOffset + border.top, position.width - border.left - border.right, position.height - border.top - border.bottom));
    //    }

    //    if (_topTile != null)
    //    {
    //        DrawTiles(_topTile, new Rect(xOffset + border.left, yOffset, position.width - border.left - border.right, border.top));
    //    }

    //    if (_bottomTile != null)
    //    {
    //        DrawTiles(_bottomTile, new Rect(xOffset + border.left, yOffset + position.height - border.bottom, position.width - border.left - border.right, border.bottom));
    //    }

    //    if (_leftTile != null)
    //    {
    //        DrawTiles(_leftTile, new Rect(xOffset, yOffset + border.top, border.left, position.height - border.top - border.bottom));
    //    }

    //    if (_rightTile != null)
    //    {
    //        DrawTiles(_rightTile, new Rect(xOffset + position.width - border.right, yOffset + border.top, border.right, position.height - border.top - border.bottom));
    //    }
    //}


    ////TODO i'm not sure that this is necessary
    //private static void DrawTiles(Texture texture, Rect screenRect)
    //{
    //    Rect sourceRect = new Rect(0, 0, screenRect.width / texture.width, screenRect.height / texture.height);
    //    Graphics.DrawTexture(screenRect, texture, sourceRect, 0, 0, 0, 0, GUI.color);
    //}

    #endregion


    #region Editor

    //TODO remove this field and put it on editor
    [HideInInspector]
    private bool _unselectable;

    public bool Unselectable
    {
        get
        {
            if (Parent == null)
            {
                return _unselectable;
            }

            return Parent._unselectable;
        }
        set { _unselectable = value; }
    }

    public HideFlags HideFlags
    {
        get { return hideFlags; }
        set
        {
            gameObject.hideFlags = value;
            hideFlags = value;
        }
    }

    #endregion


    #region Events

    private struct TreeKey : IEquatable<TreeKey>
    {
        private int _pathHash;
        private Guid _componentGuid;

        public TreeKey(int pathHash, Guid componentGuid)
        {
            _pathHash = pathHash;
            _componentGuid = componentGuid;
        }

        public bool Equals(TreeKey other)
        {
            return other._pathHash == _pathHash && other._componentGuid.Equals(_componentGuid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(TreeKey))
                return false;
            return Equals((TreeKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_pathHash * 397) ^ _componentGuid.GetHashCode();
            }
        }

        public static bool operator ==(TreeKey left, TreeKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TreeKey left, TreeKey right)
        {
            return !left.Equals(right);
        }
    }


    private static Dictionary<TreeKey, MouseStatus> _mouseStatus = new Dictionary<TreeKey, MouseStatus>();

    /*
     * TODO re-implement all InternalUserEventsBeforeDraw to use this context and treat mouse events by button separately ...
     *      ... note that last mouse position cannot be set statically because it changes with hierarchy sweeping ...
     *      ... thus, we must think about another way to implement mouse move. Maybe how it is today when it always calls mouse move even if it is stopped.
    **/


    public struct MouseButtonStatus
    {
        public bool IsDown;
        public Vector2 MouseDownPosition;

        public bool IsDragging;
        public Vector2 LastDragPosition;
    }


    public struct MouseStatus
    {
        public MouseButtonStatus LeftButton;
        public MouseButtonStatus MiddleButton;
        public MouseButtonStatus RightButton;
        public bool IsHover;
        public Vector2 LastHoverPosition;

        public bool Reserved;
    }


    protected MouseStatus GetMouseStatus()
    {
        MouseStatus ms;
        TreeKey key = new TreeKey(BitGuiContext.Current.PathHash, _id);
        if (_mouseStatus.TryGetValue(key, out ms))
        {
            ms.Reserved = true;
        }

        return ms;
    }

    private static bool IsMouseButtonStatusActive(MouseButtonStatus mbs)
    {
        if (mbs.IsDragging || mbs.IsDown)
            return true;
        return false;
    }

    private static bool IsMouseStatusActive(MouseStatus ms)
    {
        if (IsMouseButtonStatusActive(ms.LeftButton) ||
            IsMouseButtonStatusActive(ms.MiddleButton) ||
            IsMouseButtonStatusActive(ms.RightButton) ||
            ms.IsHover)
            return true;
        return false;
    }

    private static bool IsAnyButtonDown(MouseStatus ms)
    {
        if (ms.LeftButton.IsDown || ms.MiddleButton.IsDown || ms.RightButton.IsDown)
            return true;
        return false;
    }

    protected void SaveMouseStatus(MouseStatus ms)
    {
        TreeKey key = new TreeKey(BitGuiContext.Current.PathHash, _id);
        if (IsMouseStatusActive(ms))
            _mouseStatus[key] = ms;
        else if (ms.Reserved)
            _mouseStatus.Remove(key);
    }

    protected void GetMouseButtonStatus(MouseStatus ms, out MouseButtonStatus mbs)
    {
        switch (Event.current.button)
        {
            case MouseButtons.Left:
                mbs = ms.LeftButton;
                break;
            case MouseButtons.Middle:
                mbs = ms.MiddleButton;
                break;
            case MouseButtons.Right:
                mbs = ms.RightButton;
                break;
            default:
                mbs = new MouseButtonStatus();
                break;
        }
    }

    protected void SetMouseButtonStatus(ref MouseStatus ms, MouseButtonStatus mbs)
    {
        switch (Event.current.button)
        {
            case MouseButtons.Left:
                ms.LeftButton = LeftButton ? mbs : ms.LeftButton;
                break;
            case MouseButtons.Middle:
                ms.MiddleButton = MiddleButton ? mbs : ms.MiddleButton;
                break;
            case MouseButtons.Right:
                ms.RightButton = RightButton ? mbs : ms.RightButton;
                break;
        }
    }

    public event MouseDownEventHandler MouseDown;

    protected virtual void RaiseMouseDown(int mouseButton, Vector2 mousePosition)
    {
        //Debug.Log("Mouse Down Hit On component " + name + " with content \'" + Text + "\'");
        TopWindow.BringToFront();
        ForceFocus();

        if (MouseDown != null)
        {
            MouseEventArgs args;
            args.MouseButton = mouseButton;
            args.MousePosition = mousePosition;
            MouseDown(this, args);
        }
    }

    public event MouseUpEventHandler MouseUp;

    protected virtual void RaiseMouseUp(int mouseButton, Vector2 mousePosition)
    {
        MakePopupMenu(mouseButton, mousePosition);

        if (MouseUp != null)
        {
            MouseEventArgs args;
            args.MouseButton = mouseButton;
            args.MousePosition = mousePosition;
            MouseUp(this, args);
        }
    }

    public event MouseClickEventHandler MouseClick;

    protected virtual void RaiseMouseClick(int mouseButton, Vector2 mousePosition)
    {
        RunBitAnimations(BitAnimationTrigger.OnMouseClick);
        if (MouseClick != null)
        {
            MouseEventArgs args;
            args.MouseButton = mouseButton;
            args.MousePosition = mousePosition;

            MouseClick(this, args);
        }
    }

    public event MouseDoubleClickEventHandler MouseDoubleClick;
    private const float _startDragThreshold = 2.0f;

    protected virtual void RaiseMouseDoubleClick(int mouseButton, Vector2 mousePosition)
    {
        RunBitAnimations(BitAnimationTrigger.OnMouseDoubleClick);
        if (MouseDoubleClick != null)
        {
            MouseEventArgs args;
            args.MouseButton = mouseButton;
            args.MousePosition = mousePosition;
            MouseDoubleClick(this, args);
        }
    }

    public event MouseStartDragEventHandler MouseStartDrag;

    protected virtual void RaiseMouseStartDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        if (MouseStartDrag != null)
        {
            MouseStartDrag(this, new MouseDragEventArgs(mouseButton, mousePosition, positionOffset));
        }
    }

    public event MouseDragEventHandler MouseDrag;

    protected virtual void RaiseMouseDrag(int mouseButton, Vector2 mousePosition, Vector2 positionOffset)
    {
        if (MouseDrag != null)
        {
            MouseDrag(this, new MouseDragEventArgs(mouseButton, mousePosition, positionOffset));
        }
    }

    public event MouseMoveEventHandler MouseMove;

    protected virtual void RaiseMouseMove(Vector2 mousePos)
    {
        if (MouseMove != null)
        {
            MouseMove(this, mousePos);
        }
    }

    public event MouseEnterEventHandler MouseEnter;

    protected virtual void RaiseMouseEnter(Vector2 mousePos)
    {
        if ((!string.IsNullOrEmpty(TooltipProviderName)) || (!string.IsNullOrEmpty(Content.tooltip)))
        {
            // Mouse enter.
            BitStage stage = Stage;

            if ((stage != null) && (stage.TooltipManager != null))
            {
                stage.TooltipManager.BeginHover(this);
            }
        }

        if (MouseEnter != null)
        {
            MouseEnter(this, mousePos);
        }
    }

    public event MouseExitEventHandler MouseExit;

    protected virtual void RaiseMouseExit(Vector2 mousePos)
    {
        if ((!string.IsNullOrEmpty(TooltipProviderName)) || (!string.IsNullOrEmpty(Content.tooltip)))
        {
            // Mouse exit.
            BitStage stage = Stage;

            if ((stage != null) && (stage.TooltipManager != null))
            {
                stage.TooltipManager.EndHover(this);
            }
        }

        if (MouseExit != null)
        {
            MouseExit(this, mousePos);
        }
    }

    public event FocusEventHandler FocusGain;

    protected virtual void RaiseFocusGainEvent()
    {
        if (FocusGain != null)
        {
            FocusGain(this, new FocusEventArgs());
        }
    }

    public event FocusEventHandler FocusLost;

    protected virtual void RaiseFocusLostEvent()
    {
        if (FocusLost != null)
        {
            FocusLost(this, new FocusEventArgs());
        }
    }

    /// <summary>
    /// Event called when the Control is invalidated.
    /// </summary>
    public event InvalidatedEventHandler Invalidated;

    /// <summary>
    /// Raises the <see cref="Invalidated"/> event.
    /// </summary>
    protected void RaiseInvalidated()
    {
        if (Invalidated != null)
        {
            Invalidated(this, new InvalidatedEventArgs());
        }
    }

    public event BeforeTooltipEventHandler BeforeTooltip;

    protected void RaiseBeforeTooltip(Vector2 mousePosition)
    {
        if (BeforeTooltip != null)
        {
            BeforeTooltip(this, mousePosition);
        }
    }

    public event ControlOpenHandler ControlOpen;

    protected virtual void RaiseControlOpen()
    {
        IsOpen = true;
        if (ControlOpen != null)
        {
            ControlOpen(this);
        }
    }

    public event ControlCloseHandler ControlClose;

    private void RaiseControlClose()
    {
        IsOpen = false;

        if (ControlClose != null)
        {
            ControlClose(this);
        }
    }

    public event ScrollEventHandler Scroll;

    protected virtual void RaiseScroll()
    {
        if (Scroll != null)
        {
            Scroll(this);
        }
    }

    public event KeyPressedEventHandler KeyPressed;

    protected void RaiseKeyPressedEvent(KeyCode code, char character, bool alt, bool command, bool control, bool shift)
    {
        if (KeyPressed != null)
        {
            KeyPressed(this, new KeyPressedEventArgs(code, character, alt, command, control, shift));
        }
    }

    public event KeyPressedEventHandler KeyFunctionPressed;

    protected void RaiseFunctionKeyPressedEvent(KeyCode code, char character, bool alt, bool command, bool control, bool shift)
    {
        if (KeyFunctionPressed != null)
        {
            KeyFunctionPressed(this, new KeyPressedEventArgs(code, character, alt, command, control, shift));
        }
    }

    public bool UseTextureAreaHit;
    public Rect TextureAreaHit;
    public float MinAlphaHit;

    private Texture2D GetStyleBackground(GUIStyle style)
    {
        GUIStyleState styleState = style.normal;
        if (!IsOn)
        {
            if (IsActive)
            {
                styleState = style.active;
            }
            else if (IsHover)
            {

                styleState = style.hover;
            }
            else if (_focus)
            {
                styleState = style.focused;
            }
        }
        else
        {
            if (IsActive)
            {
                styleState = style.onActive;
            }
            else if (IsHover)
            {

                styleState = style.onHover;
            }
            else if (_focus)
            {
                styleState = style.onFocused;
            }

            if (styleState.background == null)
                styleState = style.onNormal;
        }


        if (styleState.background == null)
            styleState = style.normal;

        return styleState.background;

    }

    private static float GetTextureCoordinate(float controlCoord, float controlExtension, float textureExtension, Vector2 border, bool stretch)
    {
        int after = (int)(controlExtension - controlCoord);

        if (stretch)
        {
            if (after < border.y)
            {
                controlCoord = textureExtension - after;
            }
            else if (controlCoord > border.x)
            {
                float borderExtension = border.x + border.y;
                controlCoord = border.x + ((controlCoord - border.x) * (textureExtension - borderExtension) / (controlExtension - borderExtension));
            }
        }
        else if (controlCoord > textureExtension)
        {
            return -1.0f;
        }

        return controlCoord;
    }

    public bool HoverTest(Vector2 mousePosition)
    {
        if (MinAlphaHit > 1.0f || !Position.Contains(Event.current.mousePosition)) return false;
        if (MinAlphaHit <= 0.0f && !UseTextureAreaHit) return true;

        GUIStyle style = (Style ?? DefaultStyle);
        Texture2D background = GetStyleBackground(style);
        if (background == null) return true;

        float x = GetTextureCoordinate(mousePosition.x - Position.x, Position.width, background.width,
                                       new Vector2(style.border.left, style.border.right), style.stretchWidth);
        float y = GetTextureCoordinate(mousePosition.y - Position.y, Position.height, background.height,
                                       new Vector2(style.border.top, style.border.bottom), style.stretchHeight);

        if (UseTextureAreaHit)
        {
            if (!TextureAreaHit.Contains(new Vector2(x, y)))
                return false;

            if (MinAlphaHit <= 0.0f)
                return true;
        }

        x = x / background.width;
        y = y / background.height;

        if (x < 0.0f || x > 1.0f || y < 0.0f || y > 1.0f) return false;

        Color c = background.GetPixelBilinear(x, 1.0f - y);
        if (c.a < MinAlphaHit) return false;

        return true;
    }

    protected void InternalUserEventsBeforeDraw()
    {
        if (!MouseEnabled)
            return;

        bool canConsume = UserEventsBeforeDraw();

        bool consume = false;
        bool controlHover = HoverTest(Event.current.mousePosition);

        _controlHover = controlHover;

        if (controlHover && Event.current.type == EventType.Layout && Stage.HoverWindow == null)
        {
            Stage.HoverWindow = TopWindow;
        }

        if (Event.current.type != EventType.Repaint)
        {
            return;
        }

        if (Stage.HoverWindow != TopWindow)
        {
            controlHover = false;
        }

        MouseStatus ms = GetMouseStatus();

        // Draw Control attributes
        _lastFrameWasHover = IsHover = controlHover;
        IsActive = controlHover && IsAnyButtonDown(ms);

        if (!ms.IsHover && controlHover)
        {
            ms.IsHover = true;
            ms.LastHoverPosition = Event.current.mousePosition;
            SaveMouseStatus(ms);
            RaiseMouseEnter(Event.current.mousePosition);
            RaiseMouseMove(Event.current.mousePosition);
        }
        else if (ms.IsHover && !controlHover)
        {
            ms.IsHover = false;
            //ms.LastHoverPosition = Event.current.mousePosition;
            SaveMouseStatus(ms);
            RaiseMouseExit(Event.current.mousePosition);
        }
        else if (ms.IsHover && Vector2.Distance(ms.LastHoverPosition, Event.current.mousePosition) > _startDragThreshold)
        {
            ms.LastHoverPosition = Event.current.mousePosition;
            SaveMouseStatus(ms);
            RaiseMouseMove(Event.current.mousePosition);
        }

        if (CheckFocusEvent())
        {
            FocusEvent();
        }

        // Drag
        if (Enabled)
        {
            MouseButtonStatus mbs;
            GetMouseButtonStatus(ms, out mbs);

            if (mbs.IsDown)
            {
                if (!mbs.IsDragging && mbs.MouseDownPosition != Event.current.mousePosition)
                {
                    mbs.IsDragging = true;
                    mbs.LastDragPosition = mbs.MouseDownPosition;
                    Vector2 positionOffset = Event.current.mousePosition - mbs.LastDragPosition;

                    SetMouseButtonStatus(ref ms, mbs);
                    SaveMouseStatus(ms);

                    RaiseMouseStartDrag(Event.current.button, Event.current.mousePosition, positionOffset);
                }

                if (mbs.IsDragging && mbs.LastDragPosition != Event.current.mousePosition)
                {
                    Vector2 positionOffset = Event.current.mousePosition - mbs.LastDragPosition;
                    mbs.LastDragPosition = Event.current.mousePosition;

                    SetMouseButtonStatus(ref ms, mbs);
                    SaveMouseStatus(ms);

                    RaiseMouseDrag(Event.current.button, Event.current.mousePosition, positionOffset);
                }
            }
        }

        if (TopWindow.Enabled && ((consume && ConsumeEvent(Event.current.type)) || canConsume))
        {
            Event.current.Use();
        }
    }


    protected virtual bool CheckFocusEvent()
    {
        return GetFocusType() != FocusType.Passive;
    }


    public void FocusEvent()
    {
        int focusedControl = BitStage.FocusedComponentId;

        if (_focus)
        {
            if (focusedControl != ControlID)
            {
                RaiseFocusLostEvent();
                _focus = false;
            }
        }
        else if (focusedControl == ControlID)
        {
            RaiseFocusGainEvent();
            _focus = true;
        }
    }


    protected virtual void MakePopupMenu(int mouseButton, Vector2 mousePosition)
    {
        // Defines Where ContextMenu will Spawn
        if (_contextMenu != null && Position.Contains(mousePosition))
        {
            if ((_contextMenu.UseRightClick && mouseButton == MouseButtons.Right)
            || (!_contextMenu.UseRightClick && mouseButton == MouseButtons.Left))
            {
                mousePosition = new Vector2(mousePosition.x - Position.x, mousePosition.y - Position.y);
                _contextMenu.IsOpenLeft = false;
                _contextMenu.Show(new Point(mousePosition.x + AbsolutePosition.x, mousePosition.y + AbsolutePosition.y), Skin);
            }
        }
        //--
    }

    //should this control consume events?
    //override this to change behaviour
    protected virtual bool ConsumeEvent(EventType type)
    {
        return false;
    }


    /// <summary>
    /// Checks for specific events inside controls.
    /// </summary>
    /// <returns>True if the event must be consumed and not propagated to other controls.</returns>
    protected void InternalUserEvents()
    {
        try
        {
            //TODO check default events
            if (TopWindow.Enabled && (UserEventsAfterDraw() && Event.current.type != EventType.Repaint) && MouseEnabled)
            {
                Event.current.Use();
            }
        }
        catch (Exception e)
        {
            BitStage.LogError("An event exception occurred: " + e.Message);
            Event.current.Use();
        }
    }

    protected virtual bool UserEventsAfterDraw()
    {
        return false;
    }

    protected virtual bool UserEventsBeforeDraw()
    {
        return false;
    }

    #endregion


    #region Focus

    private bool _focus;

    // TODO Why focus is internal/protected?
    /// <summary>
    /// Gets or sets the focus to the Control.
    /// </summary>
    internal bool Focus
    {
        get
        {
            return _focus;
        }
        set
        {
            bool oldFocus = _focus;
            _focus = value;

            if ((_focus != oldFocus) && (_focus))
            {
                ForceFocus();
            }
        }
    }

    #endregion


    #region Hierarchy

    public static T Create<T>(string controlName) where T : BitControl
    {
        GameObject go = new GameObject();
        go.name = controlName;
        return go.AddComponent<T>();
    }

    public static BitControl Create(Type controlType, string controlName)
    {
        GameObject go = new GameObject();
        go.name = controlName;
        return (BitControl)go.AddComponent(controlType);
    }

    public static BitControl Clone(BitControl control)
    {
        return control != null ? (BitControl)BitStage.InstantiateAsset(control) : null;
    }

    public BitControl Clone()
    {
        return Clone(this);
    }


    /// <summary>
    /// Gets the number of controls in this control.
    /// </summary>
    /// <returns>Countrol count.</returns>
    protected int InternalControlCount
    {
        get { return transform.childCount; }
    }

    /// <summary>
    /// Gets the control at <see cref="index"/>, child of this control.
    /// </summary>
    /// <param name="index">Control index to get.</param>
    /// <returns>The BitControl at <see cref="index"/> or null if the index is invalid.</returns>
    protected BitControl InternalGetControlAt(int index)
    {
        return index >= 0 && index < transform.childCount ? InternalGetControlWithoutIndexCheck(index) : null;
    }

    /// <summary>
    /// Gets the control at <see cref="index"/>, child of this control.
    /// Makes no index bounds verification. Ideal for loops inside <see cref="InternalControlCount"/>, where the index is always inside bounds.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected BitControl InternalGetControlWithoutIndexCheck(int index)
    {
        return transform.GetChild(index).GetComponent<BitControl>();
    }

    /// <summary>
    /// Gets the <see cref="control"/> index inside this component.
    /// </summary>
    /// <param name="control">The <see cref="BitControl"/> to find out its index.</param>
    /// <returns>The index of the <see cref="control"/> or -1 if there is no such Control inside this Control.</returns>
    protected int InternalGetControlIndex(BitControl control)
    {
        if (control != null)
        {
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                if (control.Equals(InternalGetControlWithoutIndexCheck(i)))
                {
                    return i;
                }
            }
        }
        return -1;
    }

    /// <summary>
    /// Adds a Control to hierarchy.
    /// </summary>
    /// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
    /// <param name="controlName">Name of the Control.</param>
    /// <returns>A new instance of the Control of given _type and name.</returns>
    protected virtual BitControl InternalAddControl(Type controlType, string controlName)
    {
        if (controlType == null || !typeof(BitControl).IsAssignableFrom(controlType))
        {
            return null;
        }

        BitControl control = Create(controlType, controlName);
        control.Parent = this;

        if (EditMode)
            control.Awake();
        control.Index = -1;
        return control;
    }

    /// <summary>
    /// Adds a Control to hierarchy.
    /// </summary>
    /// <param name="controlName">Name of the Control.</param>
    /// <returns>A new instance of the Control of given _type and name.</returns>
    protected virtual T InternalAddControl<T>(string controlName) where T : BitControl
    {
        BitControl control = Create<T>(controlName);
        control.Parent = this;
        if (EditMode)
            control.Awake();
        control.Index = -1;
        return (T)control;
    }

    /// <summary>
    /// Adds an instantiated Control to hierarchy.
    /// </summary>
    /// <param name="control">Control to add.</param>
    protected virtual void InternalAddControl(BitControl control)
    {
        if (control != null)
        {
            control.transform.parent = transform;
            control.Index = -1;
        }
    }

    /// <summary>
    /// Removes the given Control from the hierarchy.
    /// </summary>
    /// <param name="control">Control to remove.</param>
    protected virtual void InternalRemoveControl(BitControl control)
    {
        if (control == null)
        {
            return;
        }
        if (EditMode)
        {
            DestroyImmediate(control.gameObject);
        }
        else
        {
            BitStage.DestroyAsset(control.gameObject);
        }
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> and name <see cref="controlName"/> in this container.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <param name="controlName">The Control's name to search.</param>
    /// <returns>The Control of given _type or null with there is no one.</returns>
    protected T InternalFindControl<T>(string controlName) where T : BitControl
    {
        if (string.IsNullOrEmpty(controlName))
        {
            BitStage.LogError("BitGUI Error: Control name is empty.");
            return null;
        }
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            Component c = transform.GetChild(i).GetComponent<T>();
            if (c == null || !controlName.Equals(c.name))
            {
                continue;
            }
            return (T)c;
        }
        BitStage.LogWarning("BitGUI Warning: control [" + typeof(T).Name + "] " + controlName + " not found inside " + name + ".");
        return null;
    }

    /// <summary>
    /// Finds the first Control children of _type <see cref="T"/> and name <see cref="controlName"/> in this container and in all its children.
    /// </summary>
    /// <typeparam name="T">The Control's _type to search.</typeparam>
    /// <param name="controlName">The Control's name to search.</param>
    /// <returns>The Control of given _type and name or null with there is no one.</returns>
    protected T InternalFindControlInChildren<T>(string controlName) where T : BitControl
    {
        if (string.IsNullOrEmpty(controlName))
        {
            BitStage.LogWarning("BitGUI Error: Control name is empty.");
            return null;
        }
        if (this is T && controlName.Equals(name))
            return (T)this;

        Transform transform1 = transform;
        for (int i = transform1.childCount; --i >= 0; )
        {
            BitControl c = transform1.GetChild(i).GetComponent<BitControl>();
            if (c != null)
            {
                c = c.InternalFindControlInChildren<T>(controlName);
                if (c != null)
                {
                    return (T)c;
                }
            }
        }

        return null;
    }

    #endregion


    #region Layout

    [HideInInspector]
    [SerializeField]
    private Rect _maxSize = new Rect(0, 0, float.MaxValue, float.MaxValue);

    /// <summary>
    /// Gets or sets the maximum size of the Control. Default is float.MinValue.
    /// </summary>
    public Size MaxSize
    {
        get { return new Size(_maxSize.width, _maxSize.height); }
        set
        {
            _maxSize = new Rect(0, 0, value.Width, value.Height);
            if ((Size.Width > MinSize.Width) || (Size.Height > MinSize.Height))
            {
                Size = MaxSize;
            }
        }
    }

    [HideInInspector]
    [SerializeField]
    private Rect _minSize;

    /// <summary>
    /// Gets or sets the minimum size of the Control. Default is 0.
    /// </summary>
    public Size MinSize
    {
        get { return new Size(_minSize.width, _minSize.height); }
        set
        {
            _minSize = new Rect(0, 0, value.Width, value.Height);
            if ((Size.Width < MinSize.Width) || (Size.Height < MinSize.Height))
            {
                Size = MinSize;
            }
        }
    }

    [HideInInspector]
    [SerializeField]
    private Rect _position;

    /// <summary>
    /// Component position is the Rect that contains exactly the Control.
    /// </summary>
    public virtual Rect Position
    {
        get { return _position; }
        internal set
        {
            //if (this is BitWindow)
            //    Debug.Log("Position " + Position + " control " + name);
            //if (_position.x != value.x && _position.y != value.y)
            //{
            Location = new Point(value.x, value.y);
            //}
            //if (_position.width != value.width && _position.height != value.height)
            //{
            Size = new Size(value.width, value.height);
            //}
        }
    }

    [SerializeField]
    private float _depth;

    public float Depth
    {
        get { return _depth; }
        set { _depth = value; }
    }


    [SerializeField]
    private float _rotationAngle;

    /// <summary>
    /// Controls Object Rotation
    /// </summary>
    public float RotationAngle
    {
        get { return _rotationAngle; }
        set { _rotationAngle = value; }
    }

    [SerializeField]
    private Vector2 _rotationPivot;

    /// <summary>
    /// Controls Animation Pivot.
    /// </summary>
    public Vector2 RotationPivot
    {
        get { return _rotationPivot; }
        set { _rotationPivot = value; }
    }

    [SerializeField]
    private Vector2 _scale;

    /// <summary>
    /// Controls Object Rotation
    /// </summary>
    public Vector2 Scale
    {
        get { return _scale; }
        set { _scale = value; }
    }

    [SerializeField]
    private Vector2 _scalePivot;

    /// <summary>
    /// Controls Animation Pivot.
    /// </summary>
    public Vector2 ScalePivot
    {
        get { return _scalePivot; }
        set { _scalePivot = value; }
    }

    private Vector2 RenderPivot(Vector2 pivot)
    {
        //GUIStyle s = Style ?? DefaultStyle;
        float x = pivot.x + AbsolutePosition.x;
        float y = pivot.y + AbsolutePosition.y;
        return new Vector2(x, y);
    }

    // TODO Improve the performance here (caching absolutePosition, for example)
    /// <summary>
    /// Control's absolute position is the Control's position at screen.
    /// </summary>
    public virtual Rect AbsolutePosition
    {
        get
        {
            BitControl p = Parent;
            if (p != null)
            {
                GUIStyle parentStyle = p.Style ?? p.DefaultStyle;
                Rect ab = p.AbsolutePosition;
                return new Rect(
                    ab.x + _position.x + parentStyle.padding.left,
                    ab.y + _position.y + parentStyle.padding.top,
                    _position.width,
                    _position.height);
                //return new Rect(ab.x + _position.x, ab.y + _position.y, _position.width, _position.height);
            }
            return _position;
        }
        set
        {
            BitControl p = Parent;
            //Rect newPosition;
            if (p != null)
            {
                Rect ab = p.AbsolutePosition;
                GUIStyle parentStyle = p.Style ?? p.DefaultStyle;
                Position = new Rect(
                    value.x - ab.x - parentStyle.padding.left,
                    value.y - ab.y - parentStyle.padding.top,
                    value.width,
                    value.height);
            }
            else
            {
                Position = value;
            }


            //Position = newPosition;

            //if (newPosition.x != _position.x || newPosition.y != _position.y)
            //{
            //    //_position = newPosition;
            //    Location = new Point(newPosition.x, newPosition.y);
            //    //CalculateAnchors();
            //    //PerformLayoutItself();
            //}
            //if (newPosition.width != _position.width || newPosition.height != _position.height)
            //{
            //    Size = new Size(newPosition.width, newPosition.height);
            //    //_position = newPosition;
            //    //CalculateAnchors();
            //    //PerformLayoutChildren();
            //}
        }
    }

    /// <summary>
    /// Location is where the Control's top-left point (x,y) is located.
    /// </summary>
    public Point Location
    {
        get { return new Point(_position.x, _position.y); }
        set
        {
            if (value.X == _position.x && value.Y == _position.y)
            {
                return;
            }
            UnsafeChangePosition((int)value.X, (int)value.Y);
            CalculateAnchors();
            PerformLayoutItself();
        }
    }

    private void UnsafeChangePosition(int x, int y)
    {
        _position.x = x;
        _position.y = y;
    }

    /// <summary>
    /// Gets or sets the size of the Control.
    /// </summary>
    public Size Size
    {
        get { return new Size(_position.width, _position.height); }
        set
        {
            if (value.Width == _position.width && value.Height == _position.height)
            {
                return;
            }
            SecureChangeSize(value);
            CalculateAnchors();
            PerformLayoutChildren();
            //SecureAutoSize();
        }
    }

    [HideInInspector]
    [SerializeField]
    private AnchorStyles _anchor = AnchorStyles.Invalid;

    [HideInInspector]
    [SerializeField]
    private float _bottomAbsoluteAnchor;

    [HideInInspector]
    [SerializeField]
    private float _leftRelativeAnchor;

    [HideInInspector]
    [SerializeField]
    private float _rightAbsoluteAnchor;

    [HideInInspector]
    [SerializeField]
    private float _topRelativeAnchor;


    /// <summary>
    /// A bitwise property that anchors the Control at one or more sides.
    /// All possible options are in <see cref="AnchorStyles"/>.
    /// </summary>
    public AnchorStyles Anchor
    {
        get { return _anchor; }
        set
        {
            _anchor = value;
            CalculateAnchors();
        }
    }

    [ContextMenu("Do Something")]
    private void DoSomething()
    {
       // if (Log.IsDebugEnabled)
       //     Log.Debug("Perform operation");
    }

    //public DockStyles Dock
    //{
    //    get { return _dock; }
    //    set { _dock = value; }
    //}

    protected void SecureChangeSize(Size value)
    {
        _position.height = Mathf.Clamp(value.Height, _minSize.height, _maxSize.height);
        _position.width = Mathf.Clamp(value.Width, _minSize.width, _maxSize.width);
    }

    /// <summary>
    /// Schedules a layout itelf.
    /// </summary>
    public void PerformLayoutItself()
    {
        _invalidated = true;
    }

    /// <summary>
    /// Schedules a layout on its children.
    /// </summary>
    public void PerformLayoutChildren()
    {
        _dirtyList = true;
    }

    /// <summary>
    /// Performs a layout if necessary.
    /// </summary>
    private void DoLayout()
    {
        if (_invalidated)
        {
            LayoutItself();
            RaiseInvalidated();
            _invalidated = false;
        }
        if (_dirtyList)
        {
            LayoutChildren();
            _dirtyList = false;
        }
    }

    private void DoCentralizeIfPossible()
    {
        float x = Position.x, y = Position.y;


        if (CenterHorizontal && (Anchor & AnchorStyles.Left) != AnchorStyles.Left && (Anchor & AnchorStyles.Right) != AnchorStyles.Right)
        {
            if (this is BitWindow)
            {
                x = (int)Screen.width / 2 - (Position.width / 2);
            }
            else
            {
                x = (int)(Parent.Position.width / 2) - (Position.width / 2);
            }
        }
        if (CenterVertical && (Anchor & AnchorStyles.Top) != AnchorStyles.Top && (Anchor & AnchorStyles.Bottom) != AnchorStyles.Bottom)
        {
            if (this is BitWindow)
            {
                y = (int)Screen.height / 2 - (Position.height / 2);
            }
            else
            {
                y = (int)(Parent.Position.height / 2) - (Position.height / 2);
            }
        }
        UnsafeChangePosition((int)x, (int)y);
    }


    /// <summary>
    /// Performs a layout in the Control (itself).
    /// </summary>
    protected virtual void LayoutItself()
    {
        BitControl parent = Parent;
        if (parent == null)
        {
            return;
        }

        if ((!MinSize.IsEmpty && Size < MinSize) || (!MaxSize.IsEmpty && Size > MaxSize))
        {
            SecureChangeSize(Size);
        }


        Rect parentPosition = parent.Position;
        AnchorLayout(parentPosition);
    }

    /// <summary>
    /// Performs a layout in its children.
    /// </summary>
    internal virtual void LayoutChildren()
    {
    }

    //private bool DockLayout(Rect parentPosition)
    //{
    //    switch (_dock)
    //    {
    //        case DockStyles.None:
    //            return false;
    //        case DockStyles.Top:
    //            _position = new Rect(0, 0, parentPosition.width, _position.height);
    //            break;
    //        case DockStyles.Left:
    //            _position = new Rect(0, 0, _position.width, parentPosition.height);
    //            break;
    //        case DockStyles.Bottom:
    //            _position = new Rect(0, parentPosition.height - _position.height, parentPosition.width, _position.height);
    //            break;
    //        case DockStyles.Right:
    //            _position = new Rect(parentPosition.width - _position.width, 0, _position.width, parentPosition.height);
    //            break;
    //        case DockStyles.Fill:
    //            _position = new Rect(0, 0, parentPosition.width, parentPosition.height);
    //            break;
    //    }
    //    return true;
    //}

    /// <summary>
    /// Performs the anchor layout.
    /// </summary>
    /// <param name="parentPosition">Parent position.</param>
    private void AnchorLayout(Rect parentPosition)
    {
        if (!Parent.AutoSize)
        {
            if (_anchor == AnchorStyles.None)
            {
                _position.x = parentPosition.width * _leftRelativeAnchor;
                _position.y = parentPosition.height * _topRelativeAnchor;
                return;
            }

            Rect position = _position;

            if ((_anchor & AnchorStyles.Top) != AnchorStyles.Top)
            {
                _position.y = parentPosition.height * _topRelativeAnchor;
            }
            if ((_anchor & AnchorStyles.Left) != AnchorStyles.Left)
            {
                _position.x = parentPosition.width * _leftRelativeAnchor;
            }

            if ((_anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
            {
                if ((_anchor & AnchorStyles.Top) == AnchorStyles.Top)
                {
                    position.yMax = parentPosition.height - _bottomAbsoluteAnchor;
                }
                else
                {
                    _position.y = parentPosition.height - _bottomAbsoluteAnchor - _position.height;
                }
            }

            if ((_anchor & AnchorStyles.Right) == AnchorStyles.Right)
            {
                if ((_anchor & AnchorStyles.Left) == AnchorStyles.Left)
                {
                    position.xMax = parentPosition.width - _rightAbsoluteAnchor;
                }
                else
                {
                    _position.x = parentPosition.width - _rightAbsoluteAnchor - _position.width;
                }
            }

            SecureChangeSize(new Size(position.width, position.height));
            //CalculateAnchors();
        }
    }

    /// <summary>
    /// Calculate anchor for layout.
    /// </summary>
    private void CalculateAnchors()
    {
        BitControl parent = Parent;
        if (parent == null)
        {
            return;
        }
        Rect parentPosition = parent.Position;
        Rect rect = Position;
        if (_anchor == AnchorStyles.None)
        {
            _leftRelativeAnchor = rect.x / parentPosition.width;
            _topRelativeAnchor = rect.y / parentPosition.height;
            return;
        }

        if ((_anchor & AnchorStyles.Top) != AnchorStyles.Top)
        {
            _topRelativeAnchor = rect.y / parentPosition.height;
        }
        if ((_anchor & AnchorStyles.Left) != AnchorStyles.Left)
        {
            _leftRelativeAnchor = rect.x / parentPosition.width;
        }

        _rightAbsoluteAnchor = parentPosition.width - rect.xMax;
        _bottomAbsoluteAnchor = parentPosition.height - rect.yMax;
    }


    #region AutoSize

    private bool ParentAutoSize()
    {
        if (Parent != null && Parent.AutoSize)
            return Parent.SecureAutoSize();

        return false;
    }

    // Only to compute final size for this control
    public bool RecursiveAutoSize()
    {
        bool ret = false;
        bool anyAutoSize = false;
        if (AutoSize)
        {
            GUISkin oldSkin = GUI.skin;
            GUI.skin = Skin;
            for (int i = 0, count = InternalControlCount; i < count; i++)
            {
                BitControl c = InternalGetControlWithoutIndexCheck(i);
                if (c != null && c.RecursiveAutoSize())
                {
                    anyAutoSize = true;
                }
            }
            if (SecureAutoSizeMe() || anyAutoSize)
                ret = true;
            GUI.skin = oldSkin;
        }
        return ret;
    }

    //TODO Optimize the atuto size calling (it must be called only when the content changes
    protected virtual void DoAutoSize()
    {
        GUIStyle style = Style ?? DefaultStyle;

        float width, height;
        if (!style.wordWrap)
        {
            Vector2 size = style.CalcSize(Content);
            width = size.x;
            height = size.y;
        }
        else
        {
            width = _position.width;
            height = style.CalcHeight(Content, Position.width);
        }

        if (_position.height == height && _position.width == width)
        {
            return;
        }

        Location = new Point(
            ((_anchor & AnchorStyles.Right) == AnchorStyles.Right) ? _position.x - (width - _position.width) : _position.x,
            ((_anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom) ? _position.y - (height - _position.height) : _position.y);
        Size = new Size(width, height);
    }

    public bool SecureAutoSize()
    {
        return SecureAutoSizeMe();
    }

    protected virtual bool SecureAutoSizeMe()
    {
        if (AutoSize) DoAutoSize();
        return AutoSize;
    }

    #endregion


    #endregion


    #region MonoBehaviour

    //TODO Find a better way to identify editor mode
    private static bool _editMode = true;

    protected static bool EditMode
    {
        get { return _editMode; }
    }

    public virtual void Start()
    {
        _editMode = false;
    }

    public void Stop()
    {
        _editMode = true;
    }

    public virtual void Awake()
    {
        try
        {
            _id = new Guid(_idToString);
        }
        catch
        {
            _id = Guid.NewGuid();
            _idToString = _id.ToString();
        }

        if (Anchor == AnchorStyles.Invalid)
        {
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
        }
        // TODO fix this in a way that Main partial class BitControl doesn't know about this partial class
        AudioManagerAwake();

        //This piece is to make sure the onopen animation is run
        if (_visible)
            Visible = true;

        transform.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable;
    }

    [HideInInspector]
    public bool SelectedInEditor = false;
    public virtual void OnDrawGizmos()
    {
        OnDrawGizmos(SelectedInEditor ? Color.yellow : Color.gray);
    }

    public virtual void OnDrawGizmos(Color color)
    {
        Rect abs = AbsolutePosition;
        if (Unselectable)
        {
            DrawRect(new Color(1f, 0f, 0f, 0.1f), abs);
            return;
        }
        DrawRect(color, abs);
        GUIStyle s = Style ?? DefaultStyle;
        DrawRect(new Color(1f, 1f, 1f, 0.1f), new Rect(abs.x + s.padding.left, abs.y + s.padding.top, abs.width - s.padding.left - s.padding.right - 2,
                                                       abs.height - s.padding.top - s.padding.bottom - 2));
        // Draws helps Gizmos
        Vector2 RenderRotationPivot = RenderPivot(RotationPivot);
        DrawRect(Color.yellow, new Rect(RenderRotationPivot.x, RenderRotationPivot.y, 4, 4));
    }

    protected static void DrawRect(Color color, Rect rect)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(new Vector3(rect.x, 0, rect.y), new Vector3(rect.xMax, 0, rect.y));
        Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.y), new Vector3(rect.xMax, 0, rect.y + rect.height));
        Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMax), new Vector3(rect.x, 0, rect.yMax));
        Gizmos.DrawLine(new Vector3(rect.x, 0, rect.yMax), new Vector3(rect.x, 0, rect.y));
    }

    #endregion


    // useful when this is a child of AbstractBitLayoutGroup


    #region Grouping

    [HideInInspector]
    [SerializeField]
    private int _index = -1;

    public int Index
    {
        get { return _index; }
        set
        {
            _index = value;
            if (Parent is AbstractBitLayoutGroup)
            {
                ((AbstractBitLayoutGroup)Parent).SortChildren();
            }
        }
    }

    [HideInInspector]
    [SerializeField]
    private bool _fixedWidth = true;

    public bool FixedWidth
    {
        get { return _fixedWidth; }
        set { _fixedWidth = value; }
    }


    [HideInInspector]
    [SerializeField]
    private bool _fixedHeight = true;

    public bool FixedHeight
    {
        get { return _fixedHeight; }
        set { _fixedHeight = value; }
    }

    [HideInInspector]
    [SerializeField]
    private GrouppingAligments _alignment = GrouppingAligments.Center;

    [SerializeField]
    private bool _centerHorizontal;

    public bool CenterHorizontal
    {
        get { return _centerHorizontal; }
        set { _centerHorizontal = value; }
    }

    [SerializeField]
    private bool _centerVertical;

    protected Animation controlAnimation;

    public bool CenterVertical
    {
        get { return _centerVertical; }
        set { _centerVertical = value; }
    }


    public GrouppingAligments Alignment
    {
        get { return _alignment; }
        set
        {
            _alignment = value;
            //BitControl parent = Parent;
            //if (parent is AbstractBitLayoutGroup)
            //{
            //    ((AbstractBitLayoutGroup) parent).FitContent();
            //}
        }
    }


    public virtual void BringToFront()
    {
        if (Parent != null)
        {
            float max = 0;
            foreach (BitControl b in Parent.transform.GetComponentsInChildren<BitControl>())
                if (b != this)
                    max = Mathf.Max(b.Depth, max);
            Depth = Mathf.Max(max + 1, Depth);
        }
    }

    public virtual void SendToBack()
    {
        if (Parent != null)
        {
            float min = 0;
            foreach (BitControl b in Parent.transform.GetComponentsInChildren<BitControl>())
                if (b != this)
                    min = Mathf.Min(b.Depth, min);
            Depth = Mathf.Min(min - 1, Depth);
        }
    }

    #endregion

    #region "API Change Workaround"
    protected static void GUIClipPush(Rect position)
    {
        GUI.BeginGroup(position, GUIContent.none, GUIStyle.none);
    }

    protected static void GUIClipPop()
    {
        GUI.EndGroup();
    }

    protected static void GUIDoTextField(Rect position, int id, GUIContent content, bool multiline, int maxLength, GUIStyle style)
    {
        // ignoring id
        //if (Event.current.type == EventType.repaint)
        {
            if (multiline)
                content.text = GUI.TextArea(position, content.text, maxLength, style);
            else
                content.text = GUI.TextField(position, content.text, maxLength, style);
        }
    }
    #endregion
}