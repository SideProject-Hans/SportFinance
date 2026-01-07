using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 合作金庫帳戶明細頁面
/// </summary>
public partial class TaiwanCooperativeBank
{
	[Inject]
	private ITaiwanCooperativeBankService TaiwanCooperativeBankService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	private List<TaiwanCooperativeBankAccount> Accounts { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		Accounts = await TaiwanCooperativeBankService.GetAllAsync();
	}

	private async Task RefreshDataAsync()
	{
		await LoadDataAsync();
	}

	private async Task OpenAddDialogAsync()
	{
		var dialog = await DialogService.ShowAsync<AddTaiwanCooperativeBankAccountDialog>("新增合作金庫帳戶明細");
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: TaiwanCooperativeBankAccount account })
		{
			await TaiwanCooperativeBankService.AddAsync(account);
			await LoadDataAsync();
		}
	}
}
