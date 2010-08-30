using Bitverse.Unity.Gui;
using UnityEngine;


public class BitContextMenu : AbstractBitPopup
{
    #region Appearance

    [SerializeField]
    private string _defaultMenuItemStyleName = "Context Menu Item";

    public string DefaultMenuItemStyleName
    {
        get { return _defaultMenuItemStyleName; }
        set { _defaultMenuItemStyleName = value; }
    }

    private GUIStyle _defaultMenuItemStyle;

    public GUIStyle DefaultMenuItemStyle
    {
        get
        {
            if (_defaultMenuItemStyle != null && !string.IsNullOrEmpty(_defaultMenuItemStyle.name))
            {
                return _defaultMenuItemStyle;
            }

            if (!string.IsNullOrEmpty(_defaultMenuItemStyleName))
            {
                GUISkin s = Skin;
                if (s != null)
                {
                    return s.FindStyle(_defaultMenuItemStyleName);
                }
            }
            return null;
        }
        set { _defaultMenuItemStyle = value; }
    }

    [SerializeField]
    private string _submenuIndicatorStyleName = "Context Submenu Indicator";

    public string SubmenuIndicatorStyleName
    {
        get { return _submenuIndicatorStyleName; }
        set { _submenuIndicatorStyleName = value; }
    }

    private GUIStyle _submenuIndicatorStyle;

    public GUIStyle SubmenuIndicatorStyle
    {
        get
        {
            if (_submenuIndicatorStyle != null && !string.IsNullOrEmpty(_submenuIndicatorStyle.name))
            {
                return _submenuIndicatorStyle;
            }

            if (!string.IsNullOrEmpty(_submenuIndicatorStyleName))
            {
                GUISkin s = Skin;
                if (s != null)
                {
                    return s.FindStyle(_submenuIndicatorStyleName);
                }
            }
            return null;
        }
        set { _submenuIndicatorStyle = value; }
    }

    #endregion


    #region Data

    private BitContextMenuOptions _options;

    private BitContextMenuItem _activeSubmenu;

    public BitContextMenuItem ActiveSubmenu
    {
        get { return _activeSubmenu; }
        set { _activeSubmenu = value; }
    }

    protected BitContextMenuOptions Options
    {
        get
        {
            if (_options == null)
            {
                _options = FindControl<BitContextMenuOptions>("Options");
                if (_options == null)
                {
                    _options = base.InternalAddControl<BitContextMenuOptions>("Options");
                    _options.Awake();
                }
            }
            return _options;
        }
    }

    #endregion


    #region Monobehaviour

    public override void Awake()
    {
        base.Awake();
        _options = Options;
        ActiveSubmenu = null;
    }

    #endregion


    #region Hierarchy

    protected override T InternalAddControl<T>(string controlName)
    {
        Debug.Log(">>>>>>>");
        T control = Options.AddControl<T>(controlName);
        control.Location = new Point(0, 0);
        control.Size = new Size(Options.Position.width, control.Position.height);
        return control;
    }

    protected override void InternalAddControl(BitControl control)
    {
        Debug.Log(">>>>>>>");
        control.Location = new Point(0, 0);
        control.Size = new Size(Options.Position.width, control.Position.height);
        _options.AddControl(control);
    }

    protected override BitControl InternalAddControl(System.Type controlType, string controlName)
    {
        Debug.Log(">>>>>>>");
        BitControl control = Options.AddControl(controlType, controlName);
        control.Location = new Point(0, 0);
        control.Size = new Size(Options.Position.width, control.Position.height);
        return control;
    }

    protected override void InternalRemoveControl(BitControl control)
    {
        _options.RemoveControl(control);
    }

    #endregion


    protected override void DoAutoSize()
    {
	    //Size before = Size;
	    //Size options = _options.Size;
        //_options.AutoSize = true;
        base.DoAutoSize();

        // Debug.Log("Before " + before + " options " + options + " | After " + Size + " options " + _options.Size);

        //GUIStyle style = Style ?? DefaultStyle;
        //Debug.Log("Style " + Style);
        //if (style != null && _options != null)
        //{
        //    Size = new Size(_options.Position.width + style.padding.horizontal,
        //                    _options.Position.height + style.padding.vertical);
        //}
    }

    private float _lastHoverTime;
    private const float NotHoverTimeToClose = 2.0f;

    public override bool Visible
    {
        set
        {
            _lastHoverTime = Time.time;

            base.Visible = value;
        }
    }

    protected override void DoDraw()
    {
        bool timeOut = false;
        if (Visible)
        {
            Stage.TooltipManager.HideTooltip();

            if (IsHover)
            {
                _lastHoverTime = Time.time;
            }
            else if ((Time.time - _lastHoverTime) >= NotHoverTimeToClose)
            {
                timeOut = true;
            }
        }

        if (timeOut || (ParentMenuItem != null && (!ParentMenuItem.ShowingSubmenu || (ParentMenuItem.ParentContextMenu != null && !ParentMenuItem.ParentContextMenu.Visible))))
        {
            if (Visible)
            {
                Visible = false;
                ActiveSubmenu = null;
            }
            return;
        }

        base.DoDraw();
    }


    public override void Show(Point position)
    {
        if (Options.gameObject.transform.childCount == 0)
        {
            Visible = false;
            return;
        }

        base.Show(position);
    }
}

