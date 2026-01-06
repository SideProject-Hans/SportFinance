using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 首頁元件
/// </summary>
public partial class Home
{
	[Inject]
	private FinanceService FinanceService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	/// <summary>
	/// 現金流資料清單
	/// </summary>
	private List<CashFlow> CashFlows { get; set; } = new();

	/// <summary>
	/// 元件初始化時載入資料
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		await LoadCashFlowsAsync();
	}

	/// <summary>
	/// 載入所有現金流資料
	/// </summary>
	private async Task LoadCashFlowsAsync()
	{
		CashFlows = await FinanceService.GetAllCashFlowsAsync();
	}

	/// <summary>
	/// 重新整理資料
	/// </summary>
	private async Task RefreshDataAsync()
	{
		await LoadCashFlowsAsync();
	}

	/// <summary>
	/// 開啟新增對話框
	/// </summary>
	private async Task OpenAddDialogAsync()
	{
		var dialog = await DialogService.ShowAsync<AddCashFlowDialog>("新增現金流");
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: CashFlow cashFlow })
		{
			await FinanceService.AddCashFlowAsync(cashFlow);
			await LoadCashFlowsAsync();
		}
	}
}