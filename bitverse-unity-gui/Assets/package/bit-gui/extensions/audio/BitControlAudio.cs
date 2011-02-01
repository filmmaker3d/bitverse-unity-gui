using System;
using Bitverse.Unity.Gui;
using UnityEngine;


public partial class BitControl
{
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
		    Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseUp, AudioGuidMouseUp);
	}

	private void OnMouseDownAudio(object sender, MouseEventArgs e)
	{
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseDown, AudioGuidMouseDown);
    }

    private void OnMouseClickAudio(object sender, MouseEventArgs e)
    {
        if (Enabled && Visible)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseClick, AudioGuidMouseClick);
    }

    private void OnMouseEnterAudio(object sender, Vector2 mousePosition)
    {
        if (Enabled && Visible && !Stage.DragManager.IsDragging)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseEnter, AudioGuidMouseEnter);
    }

    private void OnMouseExitAudio(object sender, Vector2 mousePosition)
    {
        if (Enabled && Visible && !Stage.DragManager.IsDragging)
            Stage.RaiseAudio(this, BitAudioEventTypeEnum.MouseExit, AudioGuidMouseExit);
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
    public string AudioGuidMouseUp = Guid.Empty.ToString();

	[HideInInspector]
	[SerializeField]
    public string AudioGuidMouseDown = Guid.Empty.ToString();

	[HideInInspector]
	[SerializeField]
    public string AudioGuidMouseClick = Guid.Empty.ToString();


    [HideInInspector]
    [SerializeField]
    public string AudioGuidMouseEnter = Guid.Empty.ToString();

    [HideInInspector]
    [SerializeField]
    public string AudioGuidMouseExit = Guid.Empty.ToString();
}

public partial class BitToggle : BitControl
{
    [HideInInspector]
    [SerializeField]
    public string AudioGuidToggleOn = Guid.Empty.ToString();

    [HideInInspector]
    [SerializeField]
    public string AudioGuidToggleOff = Guid.Empty.ToString();
}

public partial class BitScrollView : BitContainer
{
    [HideInInspector]
    [SerializeField]
    public string AudioGuidScroll = Guid.Empty.ToString();
}