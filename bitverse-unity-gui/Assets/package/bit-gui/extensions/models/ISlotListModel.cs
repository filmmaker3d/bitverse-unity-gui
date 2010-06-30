public interface ISlotListModel : IListModel
{
	void Add(int slot, object item);

	object GetDataAtIndex(int index);

	object GetDataAtSlot(int slot);

	int GetSlotOf(object item);

	int GetSlotOfIndex(int index);

	void SetSlotOf(object item, int slot);

	void RemoveAtSlot(int slot);

	int GetLastSlot();

	bool ContainsSlot(int slot);

	int GetNextEmptySlot();
}