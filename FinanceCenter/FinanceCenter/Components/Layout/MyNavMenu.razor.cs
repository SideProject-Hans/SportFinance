namespace FinanceCenter.Components.Layout;

/// <summary>
/// MyNavMenu 組件的 code-behind 類別
/// </summary>
public partial class MyNavMenu
{
	private bool _bankExpanded = true;

	private void ToggleBankGroup()
	{
		_bankExpanded = !_bankExpanded;
	}
}
