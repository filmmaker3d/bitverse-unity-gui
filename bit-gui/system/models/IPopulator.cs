/// <summary>
/// Populator to list controls.
/// </summary>
public interface IPopulator
{
	/// <summary>
	/// Populates one list item at once.
	/// </summary>
	/// <param name="renderer">A not-null list renderer.</param>
	/// <param name="data">Population data.</param>
	/// <param name="index">Item index.</param>
	/// <param name="selected">Whether the item is selected.</param>
	void Populate(BitControl renderer, object data, int index, bool selected);
}