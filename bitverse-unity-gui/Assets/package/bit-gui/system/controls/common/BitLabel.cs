using UnityEngine;


public class BitLabel : BitControl
{
    #region Appearance

	public override GUIStyle DefaultStyle
	{
		get { return GUI.skin.label; }
	}

	#endregion


	#region Draw
    
	protected override void DoDraw()
    {
        if (Event.current.type == EventType.repaint)
            (Style ?? DefaultStyle).Draw(Position, Content, IsHover, IsActive, IsOn, false);
	}

    #endregion
}