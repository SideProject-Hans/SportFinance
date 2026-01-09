using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 設定頁面
/// </summary>
public partial class Settings
{
	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	private List<Department> Departments { get; set; } = new();
	private List<BankInitialBalance> BankBalances { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		Departments = await SettingsService.GetAllDepartmentsAsync();
		await LoadBankBalancesAsync();
	}

	private async Task LoadBankBalancesAsync()
	{
		var balances = await SettingsService.GetAllBankInitialBalancesAsync();

		// 確保兩個銀行都有資料
		BankBalances = new List<BankInitialBalance>
		{
			balances.FirstOrDefault(b => b.BankType == "ShanghaiBank") ?? new BankInitialBalance
			{
				BankType = "ShanghaiBank",
				InitialBalance = 0,
				EffectiveYear = DateTime.Now.Year
			},
			balances.FirstOrDefault(b => b.BankType == "TaiwanCooperativeBank") ?? new BankInitialBalance
			{
				BankType = "TaiwanCooperativeBank",
				InitialBalance = 0,
				EffectiveYear = DateTime.Now.Year
			}
		};
	}

	private static string GetBankDisplayName(string bankType) => bankType switch
	{
		"ShanghaiBank" => "上海銀行",
		"TaiwanCooperativeBank" => "合作金庫",
		_ => bankType
	};

	private async Task OpenAddDepartmentDialogAsync()
	{
		var parameters = new DialogParameters<DepartmentDialog>
		{
			{ x => x.IsEdit, false },
			{ x => x.Department, new Department() }
		};

		var dialog = await DialogService.ShowAsync<DepartmentDialog>("新增部門", parameters);
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: Department department })
		{
			if (await SettingsService.IsDepartmentCodeExistsAsync(department.Code))
			{
				Snackbar.Add("部門代號已存在", Severity.Error);
				return;
			}

			await SettingsService.AddDepartmentAsync(department);
			Snackbar.Add("新增成功", Severity.Success);
			await LoadDataAsync();
		}
	}

	private async Task OpenEditDepartmentDialogAsync(Department department)
	{
		var editDepartment = new Department
		{
			Id = department.Id,
			Code = department.Code,
			Name = department.Name,
			IsActive = department.IsActive,
			SortOrder = department.SortOrder
		};

		var parameters = new DialogParameters<DepartmentDialog>
		{
			{ x => x.IsEdit, true },
			{ x => x.Department, editDepartment }
		};

		var dialog = await DialogService.ShowAsync<DepartmentDialog>("編輯部門", parameters);
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: Department updated })
		{
			if (await SettingsService.IsDepartmentCodeExistsAsync(updated.Code, updated.Id))
			{
				Snackbar.Add("部門代號已存在", Severity.Error);
				return;
			}

			await SettingsService.UpdateDepartmentAsync(updated);
			Snackbar.Add("更新成功", Severity.Success);
			await LoadDataAsync();
		}
	}

	private async Task SaveBankBalancesAsync()
	{
		foreach (var balance in BankBalances)
		{
			await SettingsService.SaveBankInitialBalanceAsync(balance);
		}

		Snackbar.Add("儲存成功", Severity.Success);
	}
}
