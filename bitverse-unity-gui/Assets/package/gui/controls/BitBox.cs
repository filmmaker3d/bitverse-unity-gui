using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BitBox: BitControl
{
    #region Public Properties

    /// <summary>
    /// 
    /// </summary>
    public string Text
    {
        get { return Content.text; }
        set { Content.text = value; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Texture Image
    {
        get { return Content.image; }
        set { Content.image = value; }
    }

    #endregion

    #region Impmements Control

    /// <summary>
    /// 
    /// </summary>
    public override void DoDraw()
    {
        if (Style != null)
        {
            UnityEngine.GUI.Box(Position, Content, Style);
        }
        else
        {
            UnityEngine.GUI.Box(Position, Content);
        }
    }

    #endregion

}
