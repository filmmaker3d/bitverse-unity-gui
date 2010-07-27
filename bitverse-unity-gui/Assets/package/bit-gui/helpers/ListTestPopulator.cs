using System;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


[ExecuteInEditMode]
public class ListTestPopulator : MonoBehaviour
{

    public int ItemCount = 10;
    private bool _randomItems = true;
    public bool RandomItems = true;

    private BitStage stage;
    public BitList list;
    private BitLabel[] _labels;

	private class EmptyPopulator : IPopulator
	{
        private BitLabel[] _labels;
	    public bool updateText;

        public EmptyPopulator(BitLabel[] labels)
        {
            _labels = labels;
        }

	    public void Populate(BitControl renderer, object data, int index, bool selected)
        {
            if (_labels == null || !updateText)
                return;

	        updateText = false;

		    int i = 0;
            foreach (BitLabel b in _labels)
            {
                b.Text = ((List<String>) data)[i++];
            }
        }
	}


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

        _labels = list.GetComponentsInChildren<BitLabel>();

		if (list.Model == null)
		{
			list.Model = new DefaultBitListModel();
			list.Populator = new EmptyPopulator(_labels);
		}

        if (_randomItems != RandomItems || (list.Model.Count >= 0 && list.Model.Count != ItemCount))
        {
            _randomItems = RandomItems;
            PopulateList();
        }
	}

    private static String str = "item ";

    private void PopulateList()
	{
		list.Model.Clear();
		for (int i = 0; i < ItemCount; i++)
        {
            String s = "";
		    s += str;
            List<String> strings = new List<string>();
            strings.Add(s + i);
            for (int j = 1; j < _labels.Length;j++ )
            {
                if (RandomItems)
                {
                    char letter = (char) Random.Range('a', 'z');
                    int lenght = Random.Range(1, 10);

                    s = "";
                    for (int c = 0; c < lenght; c++)
                        s += letter;

                    strings.Add(s);
                }
                else
                {
                    strings.Add(s+i+"-"+j);
                }
            }
            list.Model.Add(strings);

            PrintList(strings);

		}
        list.RollDownScroll();
        ((EmptyPopulator) list.Populator).updateText = true;
	}

    private void PrintList (List<String> strings)
    {
        String f = "";
        foreach (String s in strings)
        {
            f += s + " ";
        }
        Debug.Log(f);
    }
}