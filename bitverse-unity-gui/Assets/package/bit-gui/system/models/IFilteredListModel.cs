public interface IFilteredListModel
{
	object Filter { get; set; }

	bool IsFilteredValidItem(object item);

	object GetDataIgnoringFilter(int index);
}