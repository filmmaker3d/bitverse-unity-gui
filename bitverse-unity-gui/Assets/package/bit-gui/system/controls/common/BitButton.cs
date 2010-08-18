using UnityEngine;


public class BitButton : BitControl
{
	#region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.button; }
	}

	#endregion


    #region Event
    
    protected override bool ConsumeEvent(EventType type)
	{
		return true;
    }

    #endregion

    #region Draw

    protected override void DoDraw()
    {
        (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn | ForceOnState, Focus);
	}

	#endregion
}