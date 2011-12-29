using UnityEngine;

public class MarketItemData
{
    public string Name { get; private set; }
    public string Price { get; private set; }
    public string Description { get; private set; }
    public Texture2D Picture { get; private set; }

    public MarketItemData(string name, string price, string description, Texture2D picture)
    {
        Name = name;
        Price = price;
        Description = description;
        Picture = picture;
    }
}
