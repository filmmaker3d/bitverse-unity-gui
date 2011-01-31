using UnityEngine;


[ExecuteInEditMode]
public class GridListTestPopulator : MonoBehaviour
{
    private class EmptyPopulator : IPopulator
    {
        private readonly GridListTestPopulator _parent;
        public EmptyPopulator(GridListTestPopulator parent)
        {
            _parent = parent;
        }

        public void Populate(BitControl renderer, object data, int index, bool selected)
        {
            int i = 0;
            if (_parent._labels != null && _parent.texts != null && _parent.texts.Length > 0)
            {
                foreach (BitLabel b in _parent._labels)
                {
                    b.Text = string.Format(_parent._formater, _parent.texts[(index + i++) % _parent.texts.Length], index);
                }
            }

            if (_parent._images == null || _parent.images == null || _parent.images.Length == 0)
                return;

            foreach (BitPicture p in _parent._images)
            {
                p.Image = _parent.images[(index + i++) % _parent.images.Length];
            }
        }
    }

    public int ItemCount = 10;
    public string[] texts;
    public Texture2D[] images;
    public BitGridList list;
    public bool indexer;

    private BitLabel[] _labels;
    private BitPicture[] _images;

    private const string FormaterEmpty = "{0}";
    private const string FormaterIndexed = "{0}{1}";
    private string _formater;

    private void OnGUI()
    {
        if (list == null)
        {
            return;
        }

        if (list.Model == null)
        {
            list.Model = new DefaultBitListModel();
            list.Populator = new EmptyPopulator(this);
            _labels = list.GetComponentsInChildren<BitLabel>();
            _images = list.GetComponentsInChildren<BitPicture>();
        }

        _formater = indexer ? FormaterIndexed : FormaterEmpty;
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
            list.Model.Add(string.Empty);
        }
    }

    void OnDisable()
    {
        list.Model.Clear();
        list.Model = null;
        list.Populator = null;
    }
}