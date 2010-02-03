using UnityEngine;


public class BitScrollView : BitContainer
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.scrollView; }
	}

	#endregion


	[SerializeField]
	private GUIStyle _horizontalScrollStyle;

	public GUIStyle HorizontalScrollStyle
	{
		get { return _horizontalScrollStyle; }
		set { _horizontalScrollStyle = value; }
	}

	[SerializeField]
	private GUIStyle _verticalScrollStyle;

	public GUIStyle VerticalScrollStyle
	{
		get { return _verticalScrollStyle; }
		set { _verticalScrollStyle = value; }
	}


	#region Behaviour

	[SerializeField]
	private Vector2 _scrollPosition;

	[SerializeField]
	private Rect _viewRect;

	public Vector2 ScrollPosition
	{
		get { return _scrollPosition; }
		set { _scrollPosition = value; }
	}

	public Rect ViewRect
	{
		get { return _viewRect; }
		set { _viewRect = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		ScrollPosition = GUI.BeginScrollView(
			Position,
			ScrollPosition,
			_viewRect,
			_horizontalScrollStyle ?? GUI.skin.horizontalScrollbar,
			_verticalScrollStyle ?? GUI.skin.verticalScrollbar);

		DrawChildren();

		GUI.EndScrollView();
	}

	#endregion
}