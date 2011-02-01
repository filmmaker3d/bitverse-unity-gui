using System.Text.RegularExpressions;
using UnityEngine;


public class BitTextFilter : MonoBehaviour
{
	private AbstractBitTextField _field;

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
			}
			else
			{
				try
				{
					_regex = new Regex(value);
				}
				catch
				{
					_regex = new Regex(".*");
				}
			}
			SetText(_field.Text);
		}
	}

	private void SetText(string text)
	{
		_field.SetText(_regex.Match(text).Value);
	}

	public void Start()
	{
		_field = gameObject.GetComponent<AbstractBitTextField>();
		if (_field == null)
		{
			Debug.LogError("This script must be attached inside an AbstractTextField.");
			return;
		}
		_field.TextChanged += FieldTextChanged;
	}

	private void FieldTextChanged(object sender, Bitverse.Unity.Gui.ValueChangedEventArgs e)
	{
		SetText((string) e.Value);
	}

	private void OnGUI()
	{
		if (!string.IsNullOrEmpty(_filter) && _filter != Filter)
		{
			Filter = _filter;
		}
	}
}