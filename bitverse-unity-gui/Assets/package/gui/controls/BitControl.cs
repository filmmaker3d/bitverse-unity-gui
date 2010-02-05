using System;
using Bitverse.Unity.Gui;
using UnityEngine;


/// <summary>
/// Base class for all controls.
/// </summary>
public abstract class BitControl : MonoBehaviour
{
	#region Accessibility

	[SerializeField]
	private readonly Guid _id = Guid.NewGuid();


	/// <summary>
	/// Control ID.
	/// </summary>
	public Guid ID
	{
		get { return _id; }
	}

	private int _guiID;

	public int GuiID
	{
		get
		{
			if (_guiID < 0)
			{
				_guiID = GUIUtility.GetControlID(Content, FocusType.Native);
			}
			return _guiID;
		}
	}

	#endregion


	#region Appearance

	protected static readonly GUIStyle EmptyStyle = GUIStyle.none;

	[SerializeField]
	private Color _color = Color.white;

	[SerializeField]
	private GUISkin _skin;

	[SerializeField]
	private string _styleName;

	public string StyleName
	{
		get { return _styleName; }
		set { _styleName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private GUIStyle _style;


	public GUIStyle Style
	{
		get
		{
			if (_style != null && !string.IsNullOrEmpty(_style.name))
			{
				return _style;
			}

			if (!string.IsNullOrEmpty(_styleName))
			{
				GUISkin s = Skin;
				if (s != null)
				{
					return s.FindStyle(_styleName);
				}
			}
			return null;
		}
		set { _style = value; }
	}

	//TODO Test this!!!
	public virtual GUIStyle DefaultStyle
	{
		get { return EmptyStyle; }
	}


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
			PerformLayoutItself();
		}
	}

	// TODO See if this is necessary
	public Color Color
	{
		get { return _color; }
		set { _color = value; }
	}

	#endregion


	#region Behaviour

	// Parent disabled
	private bool _dirty;

	[HideInInspector]
	[SerializeField]
	private bool _disabled;

	[SerializeField]
	private bool _enabled = true;

	private bool _invalidated = true;
	private BitControl _parent;
	private BitWindow _parentForm;

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
				// si no tiene un contenedor retornamos el estado del Control
				return !_disabled ? _enabled : false;
			}

			if (Parent.Enabled)
			{
				// en caso de que tiene un contenedor y está habilitado
				// retornamos el estado del Control
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

	// TODO See if this is necessary
	//public StartPositions StartPosition
	//{
	//    get { return _startPosition; }
	//    set { _startPosition = value; }
	//}

	/// <summary>
	/// [Read-only] Whether the Control is invalidated.
	/// </summary>
	protected bool IsInvalidated
	{
		get { return _invalidated; }
	}


	/// <summary>
	/// Control's parent.
	/// </summary>
	public BitControl Parent
	{
		get { return transform.parent != null ? transform.parent.GetComponent<BitControl>() : _parent; }
		protected set { _parent = value; }
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

	#endregion


	#region Data

	[SerializeField]
	private GUIContent _content = new GUIContent();

	/// <summary>
	/// [Read-only] Gets the Control GUIContent.
	/// </summary>
	public GUIContent Content
	{
		get
		{
			//TODO WTF?
			//_content.tooltip = _id.ToString();
			return _content;
		}
	}

	private object _tag;

	/// <summary>
	/// Tag is a object associated to Control but has no effect to it. Use it to set special data that must be stored but with no side effects.
	/// </summary>
	public object Tag
	{
		get { return _tag; }
		set { _tag = value; }
	}

	private string _tooltip;

	/// <summary>
	/// Control's tooltip (text).
	/// </summary>
	public string ToolTip
	{
		get { return _tooltip; }
		set { _tooltip = value; }
	}

	[SerializeField]
	private BitPopup _popupMenu;

	public BitPopup PopupMenu
	{
		get { return _popupMenu; }
		set { _popupMenu = value; }
	}

	#endregion


	#region Draw

	/// <summary>
	/// Draws the Control.
	/// </summary>
	internal void Draw()
	{
		if (!Visible)
		{
			return;
		}

		if (_position.width <= 0 || _position.height <= 0)
		{
			DoLayout();
			return;
		}

		GUI.skin = Skin;
		GUI.SetNextControlName(ID.ToString());
		GUI.enabled = Enabled;

		DoLayout();

		InternalUserEventsBeforeDraw();
		GUI.color = _color;
		DoDraw();
		InternalUserEvents();
	}


	/// <summary>
	/// Called at the end of <see cref="Draw"/> method.
	/// </summary>
	protected abstract void DoDraw();


	//TODO i'm not sure that this is necessary
	//protected void DrawTiles(GUIStyle style, bool relative)
	//{
	//    if ((Event.current._type != EventType.Repaint) || (style == null))
	//    {
	//        return;
	//    }

	//    Rect position = Position;

	//    float xOffset = relative ? 0 : position.x;
	//    float yOffset = relative ? 0 : position.y;

	//    RectOffset border = style.border;

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
	public bool Unselectable;

	#endregion


	#region Events

	/// <summary>
	/// Event called when the mouse button is over the Control's <see cref="Position"/>.
	/// </summary>
	//public event EventHandler MouseOver;
	/// <summary>
	/// Raises the <see cref="MouseOver"/> event.
	/// </summary>
	//internal void RaiseMouseOver()
	//{
	//    RaiseEvent(MouseOver, new EventArgs());
	//}
	/// <summary>
	/// Event called when the mouse button left the Control's <see cref="Position"/>.
	/// </summary>
	//public event EventHandler MouseExit;
	/// <summary>
	/// Raises the <see cref="MouseExit"/> event.
	/// </summary>
	//internal void RaiseMouseExit()
	//{
	//    RaiseEvent(MouseExit, new EventArgs());
	//}
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


	/// <summary>
	/// Raises every _type of event.
	/// </summary>
	/// <param name="eventHandler">The event handle to raise.</param>
	/// <param name="args"></param>
	//protected void RaiseEvent(EventHandler eventHandler, EventArgs args)
	//{
	//    if (eventHandler != null)
	//    {
	//		eventHandler(this, args);
	//    }
	//}
	private void InternalUserEventsBeforeDraw()
	{
		try
		{
			// POPUP
			if (Event.current.type == EventType.MouseUp && Event.current.button == MouseButtons.Right)
			{
				if (_popupMenu != null && Position.Contains(Event.current.mousePosition))
				{
					_popupMenu.Show(new Point(Input.mousePosition.x, Screen.height - Input.mousePosition.y), Skin);
				}
			}

			if (UserEventsBeforeDraw())
			{
				Event.current.Use();
			}
		}
		catch (Exception e)
		{
			Debug.Log("An event exception ocurred: " + e.Message);
			Event.current.Use();
		}
	}

	protected virtual bool UserEventsBeforeDraw()
	{
		return false;
	}


	/// <summary>
	/// Checks for specific events inside controls.
	/// </summary>
	/// <returns>True if the event must be consumed and not propagated to other controls.</returns>
	private void InternalUserEvents()
	{
		try
		{
			//TODO check default events
			if (UserEventsAfterDraw())
			{
				Event.current.Use();
			}
		}
		catch (Exception e)
		{
			Debug.Log("An event exception ocurred: " + e.Message);
			Event.current.Use();
		}
	}

	protected virtual bool UserEventsAfterDraw()
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
			bool f = _focus;
			_focus = false;
			return f;
		}

		set { _focus = value; }
	}

	#endregion


	#region Hierarchy

	/// <summary>
	/// Adds a Control to the hierarchy.
	/// </summary>
	/// <param name="controlType">Control _type to add. Must be a BitControl child.</param>
	/// <param name="controlName">Name of the Control.</param>
	/// <returns>A new instance of the Control of given _type and name.</returns>
	protected BitControl InternalAddControl(Type controlType, string controlName)
	{
		if (controlType == null || !typeof(BitControl).IsAssignableFrom(controlType))
		{
			return null;
		}

		GameObject go = new GameObject();
		BitControl control = (BitControl)go.AddComponent(controlType);
		go.transform.parent = transform;
		go.name = controlName;
		control.Awake();
		//control.InitialSetup();
		return control;
	}

	/// <summary>
	/// Removes the given Control from the hierarchy.
	/// </summary>
	/// <param name="control">Control to remove.</param>
	protected void InternalRemoveControl(BitControl control)
	{
		if (control == null)
		{
			return;
		}
		Destroy(control.gameObject);
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
			return null;
		}

		T[] controls = InternalFindAllControls<T>();
		if (controls == null)
		{
			return null;
		}

		foreach (T control in controls)
		{
			if (controlName.Equals(control.name))
			{
				return control;
			}
		}
		return null;
	}

	/// <summary>
	/// Finds all controls of _type <see cref="T"/>.
	/// Searches in all its children.
	/// </summary>
	/// <typeparam name="T">The Control's _type to search.</typeparam>
	/// <returns>All controls of given _type or null with there is no one.</returns>
	protected T[] InternalFindAllControls<T>() where T : BitControl
	{
		T[] children = GetComponentsInChildren<T>();

		if (children == null || children.Length == 0)
		{
			return null;
		}

		return children;
	}

	#endregion


	#region Layout

	[HideInInspector]
	[SerializeField]
	private int _anchor = -1;

	[HideInInspector]
	[SerializeField]
	private float _bottomAbsoluteAnchor;

	//private DockStyles _dock = DockStyles.None;

	[HideInInspector]
	[SerializeField]
	private float _leftRelativeAnchor;

	[SerializeField]
	private Vector2 _maxSize = new Vector2(float.MaxValue, float.MaxValue);

	[SerializeField]
	private Vector2 _minSize;

	[SerializeField]
	private Rect _position;

	[HideInInspector]
	[SerializeField]
	private float _rightAbsoluteAnchor;

	[HideInInspector]
	[SerializeField]
	private float _topRelativeAnchor;

	/// <summary>
	/// Component position is the Rect that contains exactly the Control.
	/// </summary>
	public Rect Position
	{
		get { return _position; }
		internal set
		{
			if (_position.x != value.x && _position.y != value.y)
			{
				//_position = value;
				Location = new Point(value.x, value.y);
				//CalculateAnchors();
				//PerformLayoutItself();
			}
			if (_position.width != value.width && _position.height != value.height)
			{
				//_position = value;
				Size = new Size(value.width, value.height);
				//CalculateAnchors();
				//PerformLayoutChildren();
			}
		}
	}

	// TODO Improve the performance here (caching absolutePosition, for example)
	/// <summary>
	/// Control's absolute position is the Control's position at screen.
	/// </summary>
	public Rect AbsolutePosition
	{
		get
		{
			BitControl p = Parent;
			if (p != null)
			{
				GUIStyle parentStyle = p.Style ?? p.DefaultStyle;
				Rect ab = p.AbsolutePosition;
				return new Rect(ab.x + _position.x + parentStyle.padding.left, ab.y + _position.y + parentStyle.padding.top, _position.width, _position.height);
				//return new Rect(ab.x + _position.x, ab.y + _position.y, _position.width, _position.height);
			}
			return _position;
		}
		set
		{
			BitControl p = Parent;
			Rect newPosition;
			if (p != null)
			{
				Rect ab = p.AbsolutePosition;
				GUIStyle parentStyle = p.Style ?? p.DefaultStyle;
				newPosition = new Rect(value.x - ab.x - parentStyle.padding.left, value.y - ab.y - parentStyle.padding.top, value.width, value.height);
			}
			else
			{
				newPosition = value;
			}

			if (newPosition.x != _position.x || newPosition.y != _position.y)
			{
				//_position = newPosition;
				Location = new Point(newPosition.x, newPosition.y);
				//CalculateAnchors();
				//PerformLayoutItself();
			}
			if (newPosition.width != _position.width || newPosition.height != _position.height)
			{
				Size = new Size(newPosition.width, newPosition.height);
				//_position = newPosition;
				//CalculateAnchors();
				//PerformLayoutChildren();
			}
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
			_position.x = value.X;
			_position.y = value.Y;
			CalculateAnchors();
			PerformLayoutItself();
		}
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
		}
	}

	/// <summary>
	/// Gets or sets the maximum size of the Control. Default is float.MinValue.
	/// </summary>
	public Size MaxSize
	{
		get { return new Size(_maxSize.x, _maxSize.y); }
		set { _maxSize = new Vector2(value.Width, value.Height); }
	}

	/// <summary>
	/// Gets or sets the minimum size of the Control. Default is 0.
	/// </summary>
	public Size MinSize
	{
		get { return new Size(_minSize.x, _minSize.y); }
		set { _minSize = new Vector2(value.Width, value.Height); }
	}


	/// <summary>
	/// A bitwise property that anchors the Control at one or more sides.
	/// All possible options are in <see cref="AnchorStyles"/>.
	/// </summary>
	public virtual int Anchor
	{
		get { return _anchor; }
		set
		{
			_anchor = value;
			CalculateAnchors();
		}
	}

	//public DockStyles Dock
	//{
	//    get { return _dock; }
	//    set { _dock = value; }
	//}

	private void SecureChangeSize(Size value)
	{
		_position.height = Math.Max(Math.Min(value.Height, _maxSize.y), _minSize.y);
		_position.width = Math.Max(Math.Min(value.Width, _maxSize.x), _minSize.x);
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
		_dirty = true;
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
		if (_dirty)
		{
			LayoutChildren();
			_dirty = false;
		}
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
		CalculateAnchors();
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

	#endregion


	#region MonoBehaviour

	public virtual void Awake()
	{
		//Debug.Log("&&& awakening " + name);
		if (Anchor == -1)
		{
			Anchor = AnchorStyles.Top | AnchorStyles.Left;
		}
	}

	public virtual void OnDrawGizmos()
	{
		if (!Unselectable)
			DrawRect(Color.gray, AbsolutePosition);
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
}