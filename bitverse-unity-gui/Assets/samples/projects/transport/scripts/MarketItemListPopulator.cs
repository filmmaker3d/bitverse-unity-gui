using System;

public class MarketItemListPopulator : IPopulator
{
    private readonly BitPicture _itemPicture;
    private readonly BitLabel _nameLabel;
    private readonly BitLabel _priceLabel;

    public MarketItemListPopulator(MarketWindowGuiAcessor accessor)
    {
        _itemPicture = accessor.ItemimagePicture;
        _nameLabel = accessor.ItemnameLabel;
        _priceLabel = accessor.ItempriceLabel;
    }

    public void Populate(BitControl renderer, object data, int index, bool selected)
    {
        var marketItemData = data as MarketItemData;
        if (marketItemData == null)
            return;

        _itemPicture.Image = marketItemData.Picture;
        _nameLabel.Content.text = marketItemData.Name;
        _priceLabel.Content.text = marketItemData.Price;
    }
}
