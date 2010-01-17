using UnityEngine;
using Bitverse.Unity.Gui;


public class BitRepeatButton : BitControl
{
	#region Appearance

    public override string DefaultStyleName
	{
		get { return "button"; }
	}

	#endregion


	#region Events

	public event MouseClickEventHandler ButtonClick;

	#endregion


	#region Data

    public string Text
    {
        get { return Content.text; }
        set { Content.text = value; }
    }

    public Texture Image
    {
        get { return Content.image; }
        set { Content.image = value; }
    }

	#endregion


	#region Draw

    public override void DoDraw()
    {
        bool button = false;
        int currentButton = -1;
        //bool doubleClick = false;

        if (Style == null)
        {
			GUI.RepeatButton(Position, Content);
        }
        else
        {
			GUI.RepeatButton(Position, Content, Style);
        }

        //doubleClick = Event.current.isMouse && Event.current.clickCount == 2;
        currentButton = Event.current.button;

        if (button)
        {
            if (ButtonClick != null)
            {
                ButtonClick(this, new MouseClickEventArgs((MouseButtons)currentButton));
            }
        }
    }

	#endregion
}

