using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public partial class BitScrollView : BitContainer
{
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

	private Vector2 _scrollPosition = Vector2.zero;


	public Vector2 ScrollPosition
	{
		get { return _scrollPosition; }
		set
		{
            
            if(_scrollPosition != value)
            {
                RaiseScroll();
            }
                
		    _scrollPosition = value;
		}
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
		//control.MinSize = Size;
		control.Location = new Point(0, 0);
	}

	#endregion
}