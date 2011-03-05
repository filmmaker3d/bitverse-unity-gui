using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitTabbedPane : BitContainer
{
    public event SelectionChangedEventHandler<BitTab> TabChanged;

    public void RaiseTabChanged (BitTab[] tabs)
    {
        if (TabChanged != null)
        {
            TabChanged(this, new SelectionChangedEventArgs<BitTab>(tabs));
        }
    }

	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.FindStyle("tabbed pane") ?? GUI.skin.box; }
	}

	public GUIStyle TabsAreaDefaultStyle
	{
		get { return GUI.skin.FindStyle("tabbed pane tab area") ?? EmptyStyle; }
	}

	[SerializeField]
	private string _tabAreaStyleName;

	public string TabAreaStyleName
	{
		get { return _tabAreaStyleName; }
		set { _tabAreaStyleName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private GUIStyle _tabsesAreaStyle;

	public GUIStyle TabsAreaStyle
	{
		get
		{
			if (_tabsesAreaStyle != null && !string.IsNullOrEmpty(_tabsesAreaStyle.name))
			{
				return _tabsesAreaStyle;
			}

			if (!string.IsNullOrEmpty(_tabAreaStyleName))
			{
				GUISkin s = Skin;
				if (s != null)
				{
					return s.FindStyle(_tabAreaStyleName);
				}
			}
			return null;
		}
		set { _tabsesAreaStyle = value; }
	}

	public GUIStyle TabDefaultStyle
	{
		get { return GUI.skin.FindStyle("tab") ?? GUI.skin.button; }
	}

	[SerializeField]
	private string _tabStyleName;

	public string TabStyleName
	{
		get { return _tabStyleName; }
		set { _tabStyleName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private GUIStyle _tabStyle;

	public GUIStyle TabStyle
	{
		get
		{
			if (_tabStyle != null && !string.IsNullOrEmpty(_tabStyle.name))
			{
				return _tabStyle;
			}

			if (!string.IsNullOrEmpty(_tabStyleName))
			{
				GUISkin s = Skin;
				if (s != null)
				{
					return s.FindStyle(_tabStyleName);
				}
			}
			return null;
		}
		set { _tabStyle = value; }
	}

	//[SerializeField]
	//private float _tabAreaDefaultHeight = 30;

	//public float TabAreaDefaultHeight
	//{
	//    get { return _tabAreaDefaultHeight; }
	//    set { _tabAreaDefaultHeight = value; }
	//}

	#endregion


	#region Data

	[SerializeField]
	private int _selectedTabIndex = -1;

	private int _lastSelectedTabIndex = -2;

	public int SelectedTabIndex
	{
		get { return _selectedTabIndex; }
		set
		{
            if (InternalControlCount <= 0)
            {
                _selectedTabIndex = -1;
                _lastSelectedTabIndex = -1;
                return;
            }

		    if (value < 0 || value >= InternalControlCount || _selectedTabIndex == value)
		        return;
		    //BitTab tab = SelectedTab;
			//if (tab != null)
			//{
			//    tab.Visible = false;
			//}

            BitControl c = InternalGetControlAt(_selectedTabIndex);
            if (!(c is BitTab))
                return;

            BitTab[] tabs = { (BitTab)c };
            _lastSelectedTabIndex = _selectedTabIndex;
            _selectedTabIndex = value;

            RaiseTabChanged(tabs);


			//tab = SelectedTab;
			//if (tab != null)
			//{
			//    tab.Visible = true;
			//}
			
		}
	}

	public BitTab SelectedTab
	{
		get
		{
			BitControl c = InternalGetControlAt(_selectedTabIndex);
			return c is BitTab ? (BitTab)c : null;
		}
		set
		{
			if (value == null)
			{
				SelectedTabIndex = -1;
				return;
			}
			if (value.Equals(SelectedTab))
			{
				return;
			}
			SelectedTabIndex = InternalGetControlIndex(value);
		}
	}

	#endregion


	#region Draw

	private Rect _tabsAreaPosition;

	public Rect TabsAreaPosition
	{
		get { return _tabsAreaPosition; }
	}

	//private float _tabAreaHeight;
	private const float DefaultTabAreaHeight = 30;

	//private float _tabWidth;
	private const float DefaultTabWidth = 60;

	private void UpdateTabsAreaPosition(GUIStyle style, GUIStyle tabsAreaStyle)
	{
		_tabsAreaPosition = new Rect(
			style.padding.left + tabsAreaStyle.margin.left,
			style.padding.top + tabsAreaStyle.margin.top,
			Position.width - style.padding.horizontal - tabsAreaStyle.margin.horizontal,
			(tabsAreaStyle.fixedHeight > 0 ? tabsAreaStyle.fixedHeight : DefaultTabAreaHeight) - style.padding.top - tabsAreaStyle.margin.vertical);
	}

	private static float GetTabWidth(GUIStyle tabStyle)
	{
		return tabStyle.fixedWidth > 0 ? tabStyle.fixedWidth : DefaultTabWidth;
	}

	protected override void DoDraw()
	{
		GUIStyle style = Style ?? DefaultStyle;
		GUIStyle tabsAreaStyle = TabsAreaStyle ?? TabsAreaDefaultStyle;
		GUIStyle tabStyle = TabStyle ?? TabDefaultStyle;

		UpdateTabsAreaPosition(style, tabsAreaStyle);

		GUI.BeginGroup(Position, Content, style);
		DrawSelectedTab(style);
		DrawTabs(tabsAreaStyle, tabStyle);
		GUI.EndGroup();
	}

	private void DrawSelectedTab(GUIStyle style)
	{
		BitTab tab = SelectedTab;
		if (tab == null)
		{
			return;
		}
		tab.Position = GetTabRect(style, tab.Style ?? tab.DefaultStyle);
		tab.Draw();
	}

	private Rect GetTabRect(GUIStyle parentStyle, GUIStyle tabStyle)
	{
		Rect tabsAreaPosition = TabsAreaPosition;
		return new Rect(
			parentStyle.padding.left + tabStyle.margin.left,
			tabsAreaPosition.height + tabStyle.margin.top,
			tabsAreaPosition.width - tabStyle.margin.horizontal - parentStyle.padding.horizontal,
			Position.height - tabsAreaPosition.height - tabStyle.margin.vertical);
	}


	private void DrawTabs(GUIStyle tabAreaStyle, GUIStyle tabStyle)
	{
		GUI.BeginGroup(_tabsAreaPosition, tabAreaStyle);

		Rect pos = new Rect(
			tabAreaStyle.padding.left + tabStyle.margin.left,
			tabAreaStyle.padding.top + tabStyle.margin.top,
			GetTabWidth(tabStyle),
			_tabsAreaPosition.height - tabAreaStyle.padding.vertical - tabStyle.margin.vertical
			);

		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			BitTab tab = (BitTab)InternalGetControlWithoutIndexCheck(i);
			if (tab == null)
			{
				continue;
			}

			bool currentSelected = i == _selectedTabIndex;
			bool selected = GUI.Toggle(pos, currentSelected, tab.Content, tabStyle);

			if (selected && !currentSelected)
			{
				SelectedTabIndex = i;
			}

			//TODO I don't like this
			tab.Visible = currentSelected;

			pos.x += pos.width + tabStyle.margin.horizontal;
		}

		GUI.EndGroup();
	}

	protected override void DrawChildren()
	{
	}

	#endregion


	#region Hierarchy

	/// <summary>
	/// Adds a <see cref="BitControl"/> to hierarchy. This method does not accept none but <see cref="BitTab"/>.
	/// Do not call this directly, instead, call <see cref="AddTab()"/> method or one of its overloads.
	/// </summary>
	/// <typeparam name="T">Control type. Must be a <see cref="BitTab"/>.</typeparam>
	/// <param name="controlName">Control name.</param>
	/// <returns>A new <see cref="BitTab"/> if <see cref="T"/> is one.</returns>
	protected override T InternalAddControl<T>(string controlName)
	{
		if (!typeof(BitTab).IsAssignableFrom(typeof(T)))
		{
			Debug.LogError("BitTabbedPane must have only BitTabs as children.");
			return null;
		}
		T tab = base.InternalAddControl<T>(controlName);
		//tab.Visible = false;
		//if (_selectedTabIndex < 0)
		//{
		//    SelectedTabIndex = 0;
		//}
		//else
		//{
		//    SelectedTabIndex++;
		//}

		tab.Position = new Rect();
		tab.Anchor = AnchorStyles.All;
		//tab.Position = new Rect();//new Point(_tabsAreaPosition.x, _tabsAreaPosition.y);
		//tab.Size = new Size(_tabsAreaPosition.width, _tabsAreaPosition.height);

		return tab;
	}

	/// <summary>
	/// Adds a <see cref="BitControl"/> to hierarchy. This method does not accept none but <see cref="BitTab"/>.
	/// Do not call this directly, instead, call <see cref="AddTab()"/> method or one of its overloads.
	/// </summary>
	/// <param name="controlType">Control type. Must be a <see cref="BitTab"/>.</typeparam>
	/// <param name="controlName">Control name.</param>
	/// <returns>A new <see cref="BitTab"/> if <see cref="controlType"/> is one.</returns>
	protected override BitControl InternalAddControl(Type controlType, string controlName)
	{
		if (!(typeof(BitTab).IsAssignableFrom(controlType)))
		{
			Debug.LogError("Cannot add a control to a BitTabbedPane but a BitTab.");
			return null;
		}
		BitControl tab = base.InternalAddControl(controlType, controlName);
		tab.Visible = false;
		if (_selectedTabIndex < 0)
		{
			SelectedTabIndex = 0;
		}
		else
		{
			SelectedTabIndex++;
		}
		return tab;
	}

	protected override void InternalRemoveControl(BitControl control)
	{
		if (control == null)
		{
			return;
		}
		if (control.Equals(SelectedTab))
		{
			int i = InternalGetControlIndex(control);
			if (i < _lastSelectedTabIndex)
			{
				_lastSelectedTabIndex--;
			}
			SelectedTabIndex = _lastSelectedTabIndex;
		}
		base.InternalRemoveControl(control);
	}

	public BitTab AddTab(GUIContent title)
	{
		if (title == null)
		{
			title = new GUIContent("tab" + ControlCount);
		}
		BitTab tab = AddControl<BitTab>(title.text);
		tab.Content = title;
		return tab;
	}

    public int TabCount
    {
        get
        {
            return transform.childCount;
        }
    }

    public BitTab Tab(int index)
    {
        return (BitTab)InternalGetControlAt(index);
    }

    public BitTab CloneTab(int index)
    {
        BitTab source=(BitTab)InternalGetControlAt(index);
        return (BitTab)source.Clone();
    }

	public BitTab AddTab()
	{
		return AddTab(null);
	}

	public void RemoveTab(BitTab tab)
	{
		InternalRemoveControl(tab);
	}

	public void RemoveTabAt(int index)
	{
		if (index >= 0 && index < InternalControlCount)
		{
			InternalRemoveControl(InternalGetControlAt(index));
		}
	}

	public override void Awake()
	{
		base.Awake();

		if (_selectedTabIndex < 0 && InternalControlCount > 0)
		{
			SelectedTabIndex = 0;
		}
	}

	#endregion


	#region MonoBehaviour

	public override void OnDrawGizmos()
	{
		Rect abs = AbsolutePosition;
		Rect pos = new Rect(abs.x + _tabsAreaPosition.x, abs.y + _tabsAreaPosition.y, GetTabWidth(TabStyle ?? TabDefaultStyle), _tabsAreaPosition.height);
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			BitTab tab = transform.GetChild(i).GetComponent<BitTab>();
			if (tab == null)
			{
				continue;
			}
			DrawRect(Color.red, pos);
			pos.x += pos.width;
		}
		base.OnDrawGizmos();
	}

	#endregion
}