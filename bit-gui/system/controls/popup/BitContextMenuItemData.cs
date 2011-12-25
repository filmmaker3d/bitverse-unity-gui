using Bitverse.Unity.Gui;

public class BitContextMenuItemData
{
    public string OptionName;
    public object ContextMenuData;
    public MouseClickEventHandler EventHandler;

    public BitContextMenuItemData(string optionName, MouseClickEventHandler evt)
    {
        OptionName = optionName;
        ContextMenuData = null;
        EventHandler = evt;
    }

    public BitContextMenuItemData(string optionName, object contextMenuData)
    {
        OptionName = optionName;
        ContextMenuData = contextMenuData;
        EventHandler = null;
    }
}