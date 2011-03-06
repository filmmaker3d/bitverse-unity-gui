/// <summary>
/// Interface to list model.
/// </summary>
public interface IListModel
{
	/// <summary>
	/// Gets or sets the <see cref="index"/>º element.
	/// </summary>
	/// <param name="index">Valid index within the data structure.</param>
	/// <returns></returns>
	object this[int index] { get; set; }

	/// <summary>
	/// Size of the data structure.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Whether the model contains the given item;
	/// </summary>
	/// <param name="item">Item to verify.</param>
	/// <returns>True if the model contains the item.</returns>
	bool Contains(object item);

	/// <summary>
	/// Adds an object to the item structure.
	/// </summary>
	/// <param name="item">Object to add.</param>
	void Add(object item);

	/// <summary>
	/// Removes an object from the item structure.
	/// </summary>
	/// <param name="item">Object to remove.</param>
	void Remove(object item);

	/// <summary>
	/// Removes the object in the given index from the data structure.
	/// </summary>
	/// <param name="index">Index of the object to remove.</param>
	void RemoveAt(int index);

	/// <summary>
	/// Removes all the objects from the data structure.
	/// </summary>
	void Clear();

	/// <summary>
	/// Gets the item index inside the data structure.
	/// </summary>
	/// <param name="item">Item to get its index.</param>
	/// <returns></returns>
	int IndexOf(object item);
}