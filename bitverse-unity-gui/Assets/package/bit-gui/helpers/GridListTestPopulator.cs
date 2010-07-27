using UnityEngine;
using Debug=UnityEngine.Debug;


[ExecuteInEditMode]
public class GridListTestPopulator : MonoBehaviour
{
	private class EmptyPopulator : IPopulator
	{
		public void Populate(BitControl renderer, object data, int index, bool selected)
		{
			renderer.Content.text = "";
		}
	}


	public int ItemCount = 10;

	private BitStage stage;
	public BitGridList list;

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

		if (list == null)
		{
			return;
		}

		if (list.Model == null)
		{
			list.Model = new DefaultBitListModel();
			list.Populator = new EmptyPopulator();
		}

		if (list.Model.Count != ItemCount)
		{
			PopulateList();
		}
	}

	private void PopulateList()
	{
		list.Model.Clear();
		for (int i = 0; i < ItemCount; i++)
		{
			list.Model.Add("");
		}
	}
}