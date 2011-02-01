public partial class BitGuiEditorToolbox
{
	private void AddEnhancedCommonControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedGroupControls(ButtonInfo buttonInfo)
	{
		buttonInfo.AddComponent(typeof (BitHorizontalGroup), false);
		buttonInfo.AddComponent(typeof (BitVerticalGroup), false);
	}

	private void AddEnhancedListControls(ButtonInfo buttonInfo)
	{
		buttonInfo.AddComponent(typeof (BitSlotList), false);
	}

	private void AddEnhancedPopupControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedProgressControls(ButtonInfo buttonInfo)
	{
		buttonInfo.AddComponent(typeof (BitHorizontalSegmentedProgressBar), false);
		buttonInfo.AddComponent(typeof (BitVerticalSegmentedProgressBar), false);
	}

	private void AddEnhancedScrollControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedSliderControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedStageControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedTabControls(ButtonInfo buttonInfo)
	{
	}

	private void AddEnhancedTextControls(ButtonInfo buttonInfo)
	{
		buttonInfo.AddComponent(typeof(BitTipTextField), false);
	}
}