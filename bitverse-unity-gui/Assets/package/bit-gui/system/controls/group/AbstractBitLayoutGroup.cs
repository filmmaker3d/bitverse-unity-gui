using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractBitLayoutGroup : BitContainer
{
	#region MonoBehaviour

	public override void Awake()
	{
		base.Awake();
		SortChildren();
	}

	#endregion


	#region IndexManager

	[HideInInspector]
	public readonly List<BitControl> IndexMap = new List<BitControl>();


	private class IndexComparer : IComparer<BitControl>
	{
		public int Compare(BitControl x, BitControl y)
		{
			return x.Index - y.Index;
		}
	}


	public void SortChildren()
	{
		IndexMap.Clear();
		for (int i = 0; i < ControlCount; i++)
		{
			IndexMap.Add(InternalGetControlAt(i));
		}

		IndexMap.Sort(new IndexComparer());
	}

	private int GetNextIndex()
	{
		return (IndexMap.Count > 0) ? IndexMap[IndexMap.Count - 1].Index + 1 : 0;
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		(Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn, false);
		GUIClip.Push(Position);
		SecureAutoSizeMe();
		DrawChildren();
		GUIClip.Pop();
	}

	public abstract void FitContent();

	protected override bool SecureAutoSizeMe()
	{
		FixAutoSizeInChildren();
		FitContent();
		return base.SecureAutoSizeMe();
	}

	private void FixAutoSizeInChildren()
	{
		for (int i = 0; i < IndexMap.Count; i++)
		{
			BitControl c = IndexMap[i];

			if (c == null || !c.AutoSize)
			{
				continue;
			}
			c.FixedWidth = true;
			c.FixedHeight = true;

		    c.SecureAutoSize();
		}
	}

	#endregion


	#region Hierarchy

	protected override T InternalAddControl<T>(string controlName)
	{
		T control = base.InternalAddControl<T>(controlName);
		control.Index = GetNextIndex();
		return control;
	}

	protected override void InternalAddControl(BitControl control)
	{
		control.Index = GetNextIndex();
		base.InternalAddControl(control);
	}

	protected override BitControl InternalAddControl(Type controlType, string controlName)
	{
		BitControl control = base.InternalAddControl(controlType, controlName);
		control.Index = GetNextIndex();
		return control;
	}

	#endregion
}