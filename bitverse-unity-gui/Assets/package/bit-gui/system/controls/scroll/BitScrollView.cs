using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public class BitScrollView : BitContainer
{
    private  Vector2 _scrollValue;

	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.scrollView; }
	}

	#endregion

	private BitControl _scrollRenderer;

	protected BitControl ScrollRenderer
	{
		get
		{
			if (_scrollRenderer == null)
			{
				_scrollRenderer = FindControl<BitControl>();
			}
			return _scrollRenderer;
		}
	}


	#region Behaviour

	[SerializeField]
	private Vector2 _scrollPosition;


	public Vector2 ScrollPosition
	{
		get { return _scrollPosition; }
		set { _scrollPosition = value; }
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		if (ScrollRenderer == null)
		{
			return;
		}

		ScrollPosition = GUI.BeginScrollView(
			Position,
			ScrollPosition,
			ScrollRenderer.Position);
        if(ScrollPosition != _scrollValue)
        {
            //UFMAudioSource.Play(AudioConstants.ScrollChanged);
            RaiseScroll();
            _scrollValue = ScrollPosition;
        }

		DrawChildren();

		GUI.EndScrollView();
	}

	protected override T InternalAddControl<T>(string controlName)
	{
		if (_scrollRenderer != null)
		{
			InternalRemoveControl(_scrollRenderer);
		}
		_scrollRenderer = base.InternalAddControl<T>(controlName);
		SetupRenderer(_scrollRenderer);
		return (T) _scrollRenderer;
	}

	protected override BitControl InternalAddControl(Type controlType, string controlName)
	{
		if (_scrollRenderer != null)
		{
			InternalRemoveControl(_scrollRenderer);
		}
		_scrollRenderer = base.InternalAddControl(controlType, controlName);
		SetupRenderer(_scrollRenderer);
		return _scrollRenderer;
	}

	private void SetupRenderer(BitControl control)
	{
		control.MinSize = Size;
		control.Location = new Point(0, 0);
	}

	#endregion
}