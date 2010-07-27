/// <summary>
/// Default implementation of <see cref="IPopulator"/>.
/// Populate the list renderer content with data.ToString() method.
/// </summary>
public class DefaultBitListPopulator : IPopulator
{
	public void Populate(BitControl renderer, object data, int index, bool selected)
	{
		renderer.Content.text = data.ToString();
	}
}