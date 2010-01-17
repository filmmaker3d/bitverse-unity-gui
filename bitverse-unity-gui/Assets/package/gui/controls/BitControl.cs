using System;
using UnityEngine;
using Bitverse.Unity.Gui;

/// <summary>
/// Base class for all controls.
/// </summary>
public abstract class BitControl : MonoBehaviour
{
    #region Accessibility

	[SerializeField]
	private Guid _id = Guid.NewGuid();

	/// <summary>
	/// Control ID.
	/// </summary>
	public Guid ID
    {
        get { return _id; }
    }
    
    #endregion


	#region Appearance


	[SerializeField]
	private string _styleName;

    [SerializeField]
    private GUIStyle _style;

    [SerializeField]
    private GUISkin _skin;

    private GUISkin _defaultSkin;

    [SerializeField]
    private Color _color = Color.white;

	/// <summary>
	/// Style associated with the control.
	/// </summary>
	public GUIStyle Style
	{
		get
		{
			GUISkin s = Skin;
			if (s == null)
			{
				return null;
			}

			if (string.IsNullOrEmpty(_styleName))
			{
				if (string.IsNullOrEmpty(DefaultStyleName))
				{
					return null;
				}

				return (s.styles == null) ? null : (GUIStyle)s.styles[DefaultStyleName];
			}
			return (GUIStyle)s.styles[_styleName];
		}
	}

	/// <summary>
	/// Default style name.
	/// </summary>
	public virtual string DefaultStyleName
	{
		get { return null; }
	}
	
	
	/// <summary>
	/// Skin associated with the control. If there is no Skin associated, returns the <see cref="DefaultSkin"/>.
	/// When set, a re-layout is executed.
	/// </summary>
    public GUISkin Skin
    {
        get { return _skin ?? DefaultSkin; }
        set
        {
            _skin = value;
            PerformLayout();
        }
    }
    
	/// <summary>
	/// Default skin. Never null. When there is no default skin, returns Unity GUI.skin.
	/// </summary>
    public GUISkin DefaultSkin
    {
        get
        {
			// Default skin could not return null
			return _defaultSkin ?? (Parent != null ? Parent.Skin : GUI.skin);
        }
        set
        {
            _defaultSkin = value;
            PerformLayout();
        }
    }
    
    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }
	
	#endregion


	#region Behaviour
	
    [SerializeField]
    private bool _visible = true;

    [SerializeField]
    private bool _enabled = true;

    [SerializeField]
    private bool _disabled;

    private bool _invalidated = true;

	/// <summary>
	/// Whether the control is visible.
	/// </summary>
    public bool Visible
    {
        get { return _visible; }
        set { _visible = value; }
    }
    
	/// <summary>
	/// Whether the component is enabled. This property is not propagated to its children.
	/// </summary>
    public bool Enabled
    {
        get
        {
            if (Parent == null)
            {
                // si no tiene un contenedor retornamos el estado del control
                return !_disabled ? _enabled : false;
            }

            if (Parent.Enabled)
            {
                // en caso de que tiene un contenedor y está habilitado
                // retornamos el estado del control
                return !_disabled ? _enabled : false;
            }
			// retornamos no habilitado
			return false;
        }

        set { _enabled = value; }
    }
 
 	/// <summary>
	/// Whether the component is disabled. This is set to propagate the state to its children. If the parent is disabled, all its
	/// children are disabled too. If the parent is not disabled, then its children can be <see cref="Enabled"/>.
	/// </summary>
	public bool Disabled
    {
        get { return _disabled; }
        set { _disabled = value; }
    }
 
	/// <summary>
	/// [Read-only] Whether the control is invalidated.
	/// </summary>
    protected bool IsInvalidated
    {
        get { return _invalidated; }
    }
        
	#endregion


	#region Control

    private BitControl _parent = null;

    private BitWindow _parentForm = null;
	
	
	/// <summary>
	/// Control's parent.
	/// </summary>
    public BitControl Parent
    {
        get
        {
            if (transform.parent != null)
            {
                return transform.parent.GetComponent<BitControl>();
            }
            return _parent;
        }
		set
		{
			_parent = value;
			transform.parent = value.transform;
		}
	}

	/// <summary>
	/// The top-most parent.
	/// </summary>
    public BitWindow ParentForm
    {
        get
        {
            if (_parentForm == null)
            {
                if (this is BitWindow)
                {
                    _parentForm = (BitWindow)this;
                }
                else if (_parent != null)
                {
                    _parentForm = _parent.ParentForm;
                }
                else
                {
                    _parentForm = null;
                }
            }

            return _parentForm;
        }
    }
	
	#endregion

	#region Data
    private object _tag;

    private string _tooltip;

    [SerializeField]
    private GUIContent _content = new GUIContent();

	/// <summary>
	/// Tag is a object associated to control but has no effect to it. Use it to set special data that must be stored but with no side effects.
	/// </summary>
    public object Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

	/// <summary>
	/// Control's tooltip (text).
	/// </summary>
    public string ToolTip
    {
        get { return _tooltip; }
        set { _tooltip = value; }
    }

	/// <summary>
	/// [Read-only] Gets the control GUIContent.
	/// </summary>
    public GUIContent Content
    {
        get
        {
            return _content;
        }
    }


	#endregion


	#region Draw
	
	/// <summary>
	/// Draws the control.
	/// </summary>
    internal void Draw()
    {
		if (!Visible || _position.width <= 0 || _position.height <= 0)
		{
			return;
		}

		GUI.skin = Skin;
		GUI.SetNextControlName(ID.ToString());
		GUI.enabled = Enabled;
		
		Layout();
		DoDraw();
    }
    
	/// <summary>
	/// Called at the end of <see cref="Draw"/> method.
	/// </summary>
    public abstract void DoDraw();
    
    #endregion


    #region Events

	/// <summary>
	/// Event called when the mouse button is pressed inside the control's <see cref="Position"/>.
	/// </summary>
	public event EventHandler MouseClick;

	/// <summary>
	/// Event called when the mouse button is over the control's <see cref="Position"/>.
	/// </summary>
	public event EventHandler MouseOver;

	/// <summary>
	/// Event called when the mouse button left the control's <see cref="Position"/>.
	/// </summary>
	public event EventHandler MouseExit;

	/// <summary>
	/// Event called when the control is invalidated.
	/// </summary>
	public event EventHandler Invalidated;


    /// <summary>
    /// 
    /// </summary>
    internal void RaiseEventMouseOut()
    {
        if (MouseExit != null)
        {
            MouseExit(this, new EventArgs());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal void RaiseEventMouseOver()
    {
        if (MouseOver != null)
        {
            MouseOver(this, new EventArgs());
        }
    }
    
    #endregion


	#region Focus
    
	private bool _focus;

	// TODO Why focus is internal/protected?
	/// <summary>
	/// Gets or sets the focus to the control.
	/// </summary>
	internal bool Focus
    {
        get
        {
            bool f = _focus;
            _focus = false;
            return f;
        }

        set { _focus = value; }
    }

    #endregion

	#region Layout

	[SerializeField]
	private Rect _position;

	[SerializeField]
	private int _anchor = -1;
	
	/// <summary>
	/// A bitwise property that anchors the control at one or more sides.
	/// All possible options are in <see cref="AnchorStyles"/>.
	/// </summary>
	public virtual int Anchor
	{
		get { return _anchor; }
		set
		{
			_anchor = value;
			//CalculateAnchors();
		}
	}
	

	/// <summary>
	/// Component position is the Rect that contains exactly the control.
	/// </summary>
	public Rect Position
	{
		get { return _position; }
		internal set
		{
			_position = value;
			//CalculateAnchors();
		}
	}

	// TODO Improve the performance here (caching absolutePosition, for example)
	/// <summary>
	/// Control's absolute position is the control's position at screen.
	/// </summary>
    public Rect AbsolutePosition
    {
        get
        {
            BitControl p = Parent;
            if (p != null)
            {
                Rect ab = p.AbsolutePosition;
                return new Rect(ab.x + _position.x, ab.y + _position.y, _position.width, _position.height);
            }
            return _position;
        }
        set
        {
            BitControl p = Parent;
            if (p != null)
            {
                Rect ab = p.AbsolutePosition;
                _position = new Rect(value.x - ab.x, value.y - ab.y, value.width, value.height);
            }
            else
            {
                _position = value;
            }
			//CalculateAnchors();
        }
    }


	/// <summary>
	/// Gets or sets the size of the control.
	/// </summary>
	public Size Size
	{
		get { return new Size(_position.width, _position.height); }
		set
		{
			_position.height = value.Height;
			_position.width = value.Width;
			//CalculateAnchors();
			PerformLayout();
		}
	}


	/// <summary>
	/// Location is where the control's top-left point (x,y) is located.
	/// </summary>
    public Point Location
    {
        get { return new Point(_position.x, _position.y); }
        set
        {
            _position.x = value.X;
            _position.y = value.Y;
			//CalculateAnchors();
        }
    }
      
	/// <summary>
	/// Schedules a re-layout.
	/// </summary>
	public void PerformLayout()
    {
        _invalidated = true;
    }
    
	/// <summary>
	/// Calls the layout and the event associated.
	/// </summary>
	internal void Layout()
    {
		if (!_invalidated)
		{
			return;
		}
		
		DoLayout();

		if (Invalidated != null)
		{
			Invalidated(this, new EventArgs());
		}

		_invalidated = false;
    }
       

	/// <summary>
	/// Actualy do the layout. Cares about the minimum and maximum size.
	/// </summary>
	protected virtual void DoLayout()
    {
    }

	#endregion


	#region MonoBehaviour
    public virtual void OnDrawGizmos()
    {
		DrawRect(Color.white, AbsolutePosition);

		GUIStyle style = Style;
		if (style == null)
		{
			return;
		}
    }

	private static void DrawRect(Color color, Rect rect)
	{
		Gizmos.color = color;
		Gizmos.DrawLine(new Vector3(rect.x, 0, rect.y), new Vector3(rect.xMax, 0, rect.y));
		Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.y), new Vector3(rect.xMax, 0, rect.y + rect.height));
		Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMax), new Vector3(rect.x, 0, rect.yMax));
		Gizmos.DrawLine(new Vector3(rect.x, 0, rect.yMax), new Vector3(rect.x, 0, rect.y));
	}
    #endregion

}
