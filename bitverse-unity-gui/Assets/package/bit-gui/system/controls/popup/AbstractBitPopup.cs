using Bitverse.Unity.Gui;
using UnityEngine;


public class AbstractBitPopup : BitWindow, IPopup
{
    #region Appearance

    public override GUIStyle DefaultStyle
    {
        get { return GUI.skin.box; }
    }

    #endregion


    #region Behaviour

    [SerializeField]
    private bool _useRightClick = true;
    public bool UseRightClick
    {
        set { _useRightClick = value; }
        get { return _useRightClick; }
    }

    private bool _openLeft;

    public bool IsOpenLeft
    {
        get { return _openLeft; }
        set { _openLeft = value; }
    }

    private bool _visibility;

    private bool _invisibility;

    public override bool Visible
    {
        set
        {
            base.Visible = value;
            if (value)
            {
                _visibility = true;
                _invisibility = false;
            }
            else
            {
                _visibility = false;
                _invisibility = true;
            }
        }
    }

    [SerializeField]
    private bool _alwaysInsideScreen;

    public bool AlwaysInsideScreen
    {
        get { return _alwaysInsideScreen; }
        set { _alwaysInsideScreen = value; }
    }

    public void Show(Point position, GUISkin skin)
    {
        if (Skin == null)
            Skin = skin;
        Style = Skin.FindStyle(StyleName) ?? DefaultStyle;
        Show(position);
    }

    public void Show(Vector2 position, GUISkin skin)
    {
        Show(new Point(position.x, position.y), skin);
    }

    public virtual void Show(Point position)
    {
        Rect p = new Rect(position.X, position.Y, Position.width, Position.height);

        if (p.x + p.width > Screen.width)
        {
            p.x = p.x - p.width;
            IsOpenLeft = true;
        }

        if (p.y + p.height > Screen.height)
            p.y = p.y - p.height;

        Location = new Point(p.x, p.y);
        Visible = true;
        FormMode = FormModes.Popup;
    }

    public void Show(Vector2 position)
    {
        Show(new Point(position.x, position.y));
    }

    public void Hide()
    {
        Visible = false;
    }

    #endregion


    #region Draw

    protected override void DoDraw()
    {
        base.DoDraw();
        if (_visibility)
        {
            _visibility = false;
            BitStage.BeforeOnGUI += TestFocusLost;
        }

        if (_invisibility)
        {
            _invisibility = false;
            BitStage.BeforeOnGUI -= TestFocusLost;
        }
    }

    #endregion

    #region Events

    private void TestFocusLost()
    {
        if (Input.GetMouseButtonDown(MouseButtons.Left) || Input.GetMouseButtonDown(MouseButtons.Right) || Input.GetMouseButtonDown(MouseButtons.Middle))
        {
            Vector2 mp = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            if (!AbsolutePosition.Contains(mp))
            {
                Visible = false;
            }
        }
    }

    #endregion


    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();
        CanShowContextMenu = false;
        Draggable = false;
        Hide();
        FormMode = FormModes.Popup;
    }

    #endregion

    private BitContextMenuItem _parentMenuItem;

    public BitContextMenuItem ParentMenuItem
    {
        get { return _parentMenuItem; }
        set { _parentMenuItem = value; }
    }

    public void Show(Point position, BitContextMenuItem parentItem)
    {
        _parentMenuItem = parentItem;
        Skin = _parentMenuItem.ParentContextMenu.Skin;
        Style = Skin.FindStyle(StyleName);

        Show(position);
    }

    public void PreDelete()
    {
        if (_invisibility)
        {
            _invisibility = false;
            BitStage.BeforeOnGUI -= TestFocusLost;
        }
    }
}