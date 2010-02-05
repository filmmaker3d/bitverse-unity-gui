using System;
using Bitverse.Unity.Gui;
using UnityEngine;


[ExecuteInEditMode]
public class BitForm : MonoBehaviour
{
	#region Accessibility

	public Guid ID
	{
		get { return _window.ID; }
	}

	public int WindowID
	{
		get { return _window.WindowID; }
	}

	#endregion


	#region Behaviour

	private readonly BitWindow _window = new BitWindow();

	[SerializeField]
	private bool _visible = true;

	public WindowModes WindowMode
	{
		get { return _window.WindowMode; }
		set { _window.WindowMode = value; }
	}

	public FormModes FormMode
	{
		get { return _window.FormMode; }
		private set { _window.FormMode = value; }
	}

	public bool Enabled
	{
		get { return _window.Enabled; }
		set { _window.Enabled = value; }
	}

	public bool Disabled
	{
		get { return _window.Disabled; }
		set { _window.Disabled = value; }
	}

	public bool Visible
	{
		get { return _visible; }
		set
		{
			_visible = value;
			SetFocus();
		}
	}

	public bool Draggable
	{
		get { return _window.Draggable; }
		set { _window.Draggable = value; }
	}

	public void ShowModal()
	{
		FormMode = FormModes.Modal;
		BitFormsManager.PushModal(this);
		Visible = true;
	}

	public void Close()
	{
		if (FormMode == FormModes.Modal)
			BitFormsManager.PopModal();
		BitFormsManager.CloseForm(this);
		Destroy(this);
	}


	public virtual void Initialize()
	{
	}


	public delegate void BeforeOnGUIEventHandler();


	public static event BeforeOnGUIEventHandler BeforeOnGUI;

	protected virtual void DoBeforeOnGUI()
	{
		if (BeforeOnGUI != null)
		{
			BeforeOnGUI();
		}
	}

	protected virtual void DoAfterOnGUI()
	{
	}

	public virtual void OnLoad()
	{
	}

	public virtual void OnClose()
	{
	}

	#endregion


	#region Data

	public object Tag
	{
		get { return _window.Tag; }
		set { _window.Tag = value; }
	}

	public string Text
	{
		get { return _window.Text; }
		set { _window.Text = value; }
	}

	public BitControl AddControl(BitControl control)
	{
		//return _window.AddControl(Control);
		return null;
	}

	#endregion


	#region Focus

	public void SetFocus()
	{
		_window.Focus = true;
	}

	#endregion


	#region Hierarchy

	public T FindControl<T>(string controlName) where T : BitControl
	{
		if (string.IsNullOrEmpty(controlName))
		{
			return null;
		}
		for (int i = 0, count = transform.childCount; i < count; i++)
		{
			BitControl c = transform.GetChild(i).GetComponent<T>();
			if (c == null || !controlName.Equals(c.name))
			{
				continue;
			}
			return (T) c;
		}
		return null;
	}

	public T FindControlInChildren<T>(string controlName) where T : BitControl
	{
		if (string.IsNullOrEmpty(controlName))
		{
			return null;
		}

		T[] controls = FindAllControls<T>();
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

	protected T[] FindAllControls<T>() where T : BitControl
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

	public Size Size
	{
		get { return _window.Size; }
		set { _window.Size = value; }
	}

	public Size ViewSize
	{
		get { return _window.ViewSize; }
	}

	public Point Location
	{
		get { return _window.Location; }
		set { _window.Location = value; }
	}

	#endregion


	#region MonoBehaviour

	public void OnGUI()
	{
		if (Event.current.type == EventType.repaint)
		{
			return;
		}
		if (_visible)
		{
			DoBeforeOnGUI();

			GUI.matrix = transform.localToWorldMatrix;
			for (int i = 0, count = transform.childCount; i < count; i++)
			{
				Transform ch = transform.GetChild(i);
				BitControl c = (BitControl) ch.GetComponent(typeof (BitControl));
				c.Draw();
			}

			DoAfterOnGUI();
		}
	}

	#endregion
}