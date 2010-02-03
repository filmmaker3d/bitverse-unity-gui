/// <summary>
/// Default implementation of <see cref="IBitListPopulator"/>.
/// Populate the list renderer content with data.ToString() method.
/// </summary>
public class DefaultBitListPopulator : IBitListPopulator
{
	public void Populate(BitControl listRenderer, object data, int index, bool selected)
	{
		listRenderer.Content.text = data.ToString();
	}
}