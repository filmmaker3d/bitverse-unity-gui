using System;
using System.Text.RegularExpressions;
using UnityEngine;

[Obsolete]
public class BitFilteredTextField : AbstractBitTextField
{
	#region Data

	[SerializeField]
	private Regex _regex = new Regex(".*");

	[SerializeField]
	private string _filter = ".*";

	public string Filter
	{
		get { return _regex.ToString(); }
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				_regex = new Regex(".*");
				return;
			}
			try
			{
				_regex = new Regex(value);
			}
			catch
			{
				_regex = new Regex(".*");
			}
		}
	}

	public override string Text
	{
		set
		{
			base.Text = _regex.Match(value).Value;
		}
	}

	protected override bool IsMultiline()
	{
		return false;
	}

	#endregion


	#region Draw

	protected override void DoDraw()
	{
		if (!string.IsNullOrEmpty(_filter) && _filter != Filter)
		{
			Filter = _filter;
		}
		base.DoDraw();
	}

	#endregion

	#region MonoBehaviour

	public override void Awake()
	{
		Debug.LogError("You are using an obsolete component. Change it to TextField and add a BitTextFilter.");
		base.Awake();
	}

	#endregion
}