using UnityEngine;


[ExecuteInEditMode]
public class SlotListTestPopulator : MonoBehaviour
{
	private class EmptyPopulator : IPopulator
	{
		public void Populate(BitControl renderer, object data, int index, bool selected)
		{
		}
	}


	public int ItemCount = 10;
	public int Interval = 100;

	private BitStage stage;
	public BitSlotList list;
	private int _lastInterval;

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
			list.Model = new SlotListModel();
			list.Populator = new EmptyPopulator();
		}

		if (list.Model != null && (list.Model.Count != ItemCount || _lastInterval != Interval))
		{
			PopulateList();
		}
	}

	private void PopulateList()
	{
		list.Model.Clear();
		for (int i = 0; i < ItemCount; i++)
		{
			list.Model.Add(i * Interval, "item " + i);
		}
		_lastInterval = Interval;
	}
}