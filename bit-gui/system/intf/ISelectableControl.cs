public interface ISelectableControl<T>
{
	int IndexOf(T item);

	bool MultiSelection { get; set; }

	void AddSelectionItem(T item, short modifiers);

	void SelectRange(int first, int last);

	bool RemoveSelectionItem(T item);

	void ClearSelection();

	bool IsSelected(T item);
}