using UnityEngine;


[ExecuteInEditMode]
public class BitEditorStage : BitStage
{
	public Texture Background
	{
		get { return Content.image; }
		set { Content.image = value; }
	}

	public ScaleMode BackgroundScaleMode = ScaleMode.StretchToFill;


	protected override void DoDraw()
	{
		if (Content.image != null)
		{
			GUI.DrawTexture(Position, Content.image, BackgroundScaleMode);
		}
	}

	public override void OnGUI()
	{
		DoDraw();
		base.OnGUI();
	}
}