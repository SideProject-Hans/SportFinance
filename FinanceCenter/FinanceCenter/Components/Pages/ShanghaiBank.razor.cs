using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 上海銀行帳戶明細頁面
/// </summary>
public partial class ShanghaiBank
{
	[Inject]
	private IShanghaiBankService ShanghaiBankService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	private List<ShanghaiBankAccount> Accounts { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		Accounts = await ShanghaiBankService.GetAllAsync();
	}

	private async Task RefreshDataAsync()
	{
		await LoadDataAsync();
	}

	private async Task OpenAddDialogAsync()
	{
		var dialog = await DialogService.ShowAsync<AddShanghaiBankAccountDialog>("新增上海銀行帳戶明細");
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: ShanghaiBankAccount account })
		{
			await ShanghaiBankService.AddAsync(account);
			await LoadDataAsync();
		}
	}
}
