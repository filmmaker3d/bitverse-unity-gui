using UnityEngine;


public class ListAndGridTest : MonoBehaviour
{
	private class MyPopulator : IBitListPopulator
	{
		public void Populate(BitControl listRenderer, object data, int index, bool selected)
		{
			if (data == null)
			{
				return;
			}
			listRenderer.Content.text = (selected ? "* " : "") + data;
		}
	}


	private void Start()
	{
		BitForm form = gameObject.GetComponent<BitForm>();
		if (form == null)
		{
			Debug.Log("Form not found");
			return;
		}
		BitWindow w = form.FindControl<BitWindow>("Window1");
		if (w == null)
		{
			Debug.Log("'Window1' not found");
			return;
		}
		BitList b = w.FindControlInChildren<BitList>();
		if (b == null)
		{
			Debug.Log("List not found");
		}

		BitGridList c = w.FindControlInChildren<BitGridList>();
		if (c == null)
		{
			Debug.Log("GridList not found");
		}

		DefaultBitListModel model = new DefaultBitListModel();

		for (int i = 0; i < 10; i++)
		{
			model.Add("item " + i);
		}

		if (b != null)
		{
			b.Model = model;
			b.Populator = new MyPopulator();
		}
		if (c != null)
		{
			c.Model = model;
			c.Populator = new MyPopulator();
		}
	}
}