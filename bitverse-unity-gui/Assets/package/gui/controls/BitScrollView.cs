using Bitverse.Unity.Gui;
using UnityEngine;


public class BitScrollView : BitContainer
{
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

    public override void DoDraw()
    {

        if (Style != null)
        {
			ScrollPosition = GUI.BeginScrollView(Position, ScrollPosition, _viewRect, Style, Style);
        }
        else
        {
			ScrollPosition = GUI.BeginScrollView(Position, ScrollPosition, _viewRect);
        }

        DrawChildren();

		GUI.EndScrollView();
	}

	#endregion
}
