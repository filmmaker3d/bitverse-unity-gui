using UnityEngine;


[ExecuteInEditMode]
public class ToggleWindow : MonoBehaviour
{

	private BitStage stage;
	public BitWindow window;
	public BitToggle toggle;

	private void OnGUI()
	{
		if (stage == null)
		{
			stage = gameObject.GetComponent<BitStage>();
			if (stage == null)
			{
				Debug.Log("Form not found");
				return;
			}
		}

		if (window == null)
		{
			return;
		}
		if (toggle == null)
		{
			return;
		}

		window.Visible = toggle.Value;
	}


}