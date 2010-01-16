using System;
using UnityEngine;
using Bitverse.Unity.Gui;

public abstract class BitControl : MonoBehaviour
{
    #region Private Variables

    /// <summary>
    /// ID del control
    /// </summary>
    private Guid _ID = Guid.NewGuid();

    /// <summary>
    /// Tag
    /// </summary>
    private object _tag;

    /// <summary>
    /// Posición (x, y)
    /// </summary>
    [SerializeField]
    private Rect _position;


    /// <summary>
    /// Estilo
    /// </summary>
    private GUIStyle _style;

    /// <summary>
    /// Skin
    /// </summary>
    [SerializeField]
    private GUISkin _skin;

    /// <summary>
    /// Skin por defecto
    /// </summary>
    private GUISkin _defaultSkin;

    /// <summary>
    /// Visible o no visible
    /// </summary>
    [SerializeField]
    private bool _visible = true;

    /// <summary>
    /// Habilitado o no habilitado
    /// </summary>
    [SerializeField]
    private bool _enabled = true;

    /// <summary>
    /// Mensaje tooltip
    /// </summary>
    private string _tooltip;

    /// <summary>
    /// Contenido (texto, imagen, tooltip)
    /// </summary>
    [SerializeField]
    private GUIContent _content = new GUIContent();

    [SerializeField]
    private Color _color = Color.white;

    /// <summary>
    /// Contenedor 
    /// </summary>
    private BitControl _parent = null;

    /// <summary>
    /// Formulario contenedor
    /// </summary>
    private BitWindow _parentForm = null;

    /// <summary>
    /// 
    /// </summary>
    private bool _focus = false;

    private StartPositions _startPosition = StartPositions.Manual;
    [SerializeField]
    private bool _disabled = false;

    private bool _invalidated = true;

    #endregion

    #region Public Events

    public event EventHandler MouseOver;
    public event EventHandler MouseOut;
    public event EventHandler Invalidated;

    #endregion

    #region Public Properties

    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public bool Disabled
    {
        get { return _disabled; }
        set { _disabled = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Guid ID
    {
        get { return _ID; }
    }

    /// <summary>
    /// 
    /// </summary>
    public string ToolTip
    {
        get { return _tooltip; }
        set { _tooltip = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public object Tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Size Size
    {
        get { return new Size(_position.width, _position.height); }
        set
        {
            _position.height = value.Height;
            _position.width = value.Width;
            PerformLayout();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Point Location
    {
        get { return new Point(_position.x, _position.y); }
        set
        {
            _position.x = value.X;
            _position.y = value.Y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public GUIStyle Style
    {
        get { return _style; }
        set { _style = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public GUISkin Skin
    {
        get
        {
            if (_skin == null)
                return DefaultSkin;

            return _skin;
        }
        set
        {
            _skin = value;
            PerformLayout();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public GUISkin DefaultSkin
    {
        get
        {
            if (_defaultSkin != null)
            {
                return _defaultSkin;
            }
            else if (Parent != null)
            {
                return Parent.Skin;
            }

            return null;
        }
        set
        {
            _defaultSkin = value;
            PerformLayout();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public BitControl Parent
    {
        get
        {
            //return _parent; 
            if (transform.parent != null)
            {
                return transform.parent.GetComponent<BitControl>();
            }
            return _parent;
        }
        protected internal set { _parent = value; }
    }

    /// <summary>
    /// 
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

    /// <summary>
    /// 
    /// </summary>
    public bool Visible
    {
        get { return _visible; }
        set { _visible = value; }
    }

    /// <summary>
    /// 
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
            else
            {
                // retornamos no habilitado
                return false;
            }
        }

        set { _enabled = value; }
    }

    public StartPositions StartPosition
    {
        get { return _startPosition; }
        set { _startPosition = value; }
    }

    #endregion



    #region Protected Properties

    protected bool Focus
    {
        get
        {
            bool f = _focus;
            _focus = false;
            return f;
        }

        set { _focus = value; }
    }

    protected bool IsInvalidated
    {
        get { return _invalidated; }
    }

    /// <summary>
    /// 
    /// </summary>
    public GUIContent Content
    {
        get
        {
            _content.tooltip = _ID.ToString();
            return _content;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Rect Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    /// <summary>
    /// 
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
        }
    }

    #endregion

    #region Internal Methods

    /// <summary>
    /// 
    /// </summary>
    public void Draw()
    {
        if (Visible)
        {

            GUI.skin = Skin;

            GUI.SetNextControlName(ID.ToString());

            GUI.enabled = Enabled;

            Layout();

            DoDraw();
        }
    }

    protected void Layout()
    {
        if (_invalidated)
        {
            DoLayaout();

            if (Invalidated != null)
            {
                Invalidated(this, new EventArgs());
            }

            _invalidated = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal void RaiseEventMouseOut()
    {
        if (MouseOut != null)
        {
            MouseOut(this, new EventArgs());
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

    internal void SetFocus()
    {
        Focus = true;
    }



    #endregion

    #region Abstract Methods

    /// <summary>
    /// 
    /// </summary>
    public abstract void DoDraw();

    protected virtual void DoLayaout()
    {
    }

    #endregion

    #region Public Methods

    public void PerformLayout()
    {
        _invalidated = true;
    }

    public virtual void OnDrawGizmos()
    {
        Rect rect = AbsolutePosition;
        Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMin), new Vector3(rect.xMax, 0, rect.yMin));
        Gizmos.DrawLine(new Vector3(rect.x + rect.width, 0, rect.y), new Vector3(rect.x + rect.width, 0, rect.y + rect.height));
        Gizmos.DrawLine(new Vector3(rect.x + rect.width, 0, rect.y + rect.height), new Vector3(rect.x, 0, rect.y + rect.height));
        Gizmos.DrawLine(new Vector3(rect.x, 0, rect.y + rect.height), new Vector3(rect.x, 0, rect.y));

    }

    #endregion

}
