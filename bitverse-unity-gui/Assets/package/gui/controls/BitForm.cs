using UnityEngine;
using System;
using Bitverse.Unity.Gui;

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
        get { return _window.WindowId; }
    }
    
	#endregion


	#region Appearance

    public GUISkin DefaultSkin
    {
        get { return _window.DefaultSkin; }
        set { _window.DefaultSkin = value; }
    }

	#endregion


	#region Behaviour

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
    
    #endregion

    #region Control
    
    private BitWindow _window;

    public virtual void Initialize() 
    { 
    }

	protected virtual void BeforeOnGUI()
	{
	}

	protected virtual void AfterOnGUI()
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
        //return _window.AddControl(control);
        return null;
    }

	#endregion


	#region Focus

    public void SetFocus()
    {
        _window.Focus = true;
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
        if (_visible)
        {
            BeforeOnGUI();

            GUI.matrix = transform.localToWorldMatrix;
            //Debug.Log(transform.localToWorldMatrix);
            for (int i = 0, count = transform.childCount; i < count; i++)
            {
                Transform ch = transform.GetChild(i);
                BitControl c = (BitControl)ch.GetComponent(typeof(BitControl));
                c.Draw();
            }

            AfterOnGUI();
        }
    }

    #endregion

    #region Public Methods


    public void Show()
    {
        _visible = true;
        SetFocus();
    }


    public void Hide()
    {
        _visible = false;
    }


    #endregion

}




