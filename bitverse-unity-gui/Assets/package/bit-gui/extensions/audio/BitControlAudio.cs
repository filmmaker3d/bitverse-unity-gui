using Bitverse.Unity.Gui;
using UnityEngine;


public partial class BitControl
{
	#region Audio 

	// TODO fix this in a way that Main partial class BitControl doesn't know about this partial class
	public bool AudioManagerAwake()
	{
		MouseUp += OnMouseUpAudio;
		MouseDown += OnMouseDownAudio;
		MouseClick += OnMouseClickAudio;
	    MouseEnter += OnMouseEnterAudio;
	    MouseExit += OnMouseExitAudio;
	    Scroll += OnScroll;
        KeyPressed += OnKeypress;

		return true;
	}

	private void OnMouseUpAudio(object sender, MouseEventArgs e)
	{
        if (Enabled && Visible)
		    Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseUp, MouseUpAudioName);
	}

	private void OnMouseDownAudio(object sender, MouseEventArgs e)
	{
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseDown, MouseDownAudioName);
    }

    private void OnMouseClickAudio(object sender, MouseEventArgs e)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseClick, MouseClickAudioName);
    }

    private void OnMouseEnterAudio(object sender, MouseMoveEventArgs e)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseEnter, MouseEnterAudioName);
    }

    private void OnMouseExitAudio(object sender, MouseMoveEventArgs e)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseExit, MouseExitAudioName);
    }

    private void OnScroll(object sender)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.Scroll, null);
    }

    private void OnKeypress(object sender, KeyPressedEventArgs e)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.Keypress, null);
    }


	[HideInInspector]
	[SerializeField]
	private string _mouseUpAudioName = "default";

	public string MouseUpAudioName
	{
		get { return _mouseUpAudioName; }
		set { _mouseUpAudioName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private string _mouseDownAudioName = "default";

	public string MouseDownAudioName
	{
		get { return _mouseDownAudioName; }
		set { _mouseDownAudioName = value; }
	}

	[HideInInspector]
	[SerializeField]
	private string _mouseClickAudioName = "default";

	public string MouseClickAudioName
	{
		get { return _mouseClickAudioName; }
		set { _mouseClickAudioName = value; }
    }

    [HideInInspector]
    [SerializeField]
    private string _mouseEnterAudioName = "default";

    public string MouseEnterAudioName
    {
        get { return _mouseEnterAudioName; }
        set { _mouseEnterAudioName = value; }
    }

    [HideInInspector]
    [SerializeField]
    private string _mouseExitAudioName = "default";

    public string MouseExitAudioName
    {
        get { return _mouseExitAudioName; }
        set { _mouseExitAudioName = value; }
    }

	#endregion
}