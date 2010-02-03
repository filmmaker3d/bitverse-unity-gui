/// <summary>
/// Populator to list controls.
/// </summary>
public interface IBitListPopulator
{
	/// <summary>
	/// Populates one list item at once.
	/// </summary>
	/// <param name="listRenderer">A not-null list renderer.</param>
	/// <param name="data">Population data.</param>
	/// <param name="index">Item index.</param>
	/// <param name="selected">Whether the item is selected.</param>
	void Populate(BitControl listRenderer, object data, int index, bool selected);
}