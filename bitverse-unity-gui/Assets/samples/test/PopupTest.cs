using UnityEngine;


public class PopupTest : MonoBehaviour
{
	private class MyPopulator : IPopulator
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


	private BitPopup p;
	//BitButton b;

	private void Start()
	{
        /*
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


		p = form.FindControlInChildren<BitPopup>("Popup1");
		if (p == null)
		{
			Debug.Log("'Popup1' not found");
			return;
		}

		BitList o = p.FindControlInChildren<BitList>("options");
		if (o == null)
		{
			Debug.Log("'options' List not found");
			return;
		}


		b = w.FindControl<BitButton>("Button1");
		if (b == null)
		{
			Debug.Log("'Button1' not found");
		}
		else
		{
			Debug.Log("installing selection changed event");
			p.SelectionChanged += PopupSelectionChanged;
		}


		DefaultBitListModel model = new DefaultBitListModel();

		for (int i = 0; i < 10; i++)
		{
			model.Add("item " + i);
		}


		o.Model = model;
		o.Populator = new MyPopulator();
        */
	}

	void PopupSelectionChanged(object sender, Bitverse.Unity.Gui.SelectionChangedEventArgs<object> e)
	{
		//if (e.Selection.Length > 0)
		//	b.Content.text = e.Selection[0].ToString();
	}
}