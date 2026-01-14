using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

	private List<Department> Departments { get; set; } = [];
	private List<BankInitialBalance> BankBalances { get; set; } = [];
	private int SelectedTab { get; set; }
	private List<int> AvailableYears { get; } = Enumerable.Range(DateTime.Now.Year - 5, 11).ToList();
	private CancellationTokenSource? _saveCts;

	private void SelectTab(int tab)
	{
		SelectedTab = tab;
	}

	/// <summary>
	/// 處理導航卡片的鍵盤事件
	/// </summary>
	private void HandleNavKeyDown(KeyboardEventArgs e, int tab)
	{
		if (e.Key is "Enter" or " ")
		{
			SelectTab(tab);
		}
	}

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

		BankBalances =
		[
			GetOrCreateBankBalance(balances, "ShanghaiBank"),
			GetOrCreateBankBalance(balances, "TaiwanCooperativeBank")
		];
	}

	private static BankInitialBalance GetOrCreateBankBalance(List<BankInitialBalance> balances, string bankType)
	{
		return balances.FirstOrDefault(b => b.BankType == bankType) ?? new BankInitialBalance
		{
			BankType = bankType,
			InitialBalance = 0,
			EffectiveYear = DateTime.Now.Year
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

	private void ScheduleSave()
	{
		_saveCts?.Cancel();
		_saveCts = new CancellationTokenSource();
		var token = _saveCts.Token;

		_ = DelayedSaveAsync(token);
	}

	private async Task DelayedSaveAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(400, token);
			await InvokeAsync(SaveBankBalancesAsync);
		}
		catch (TaskCanceledException)
		{
			// 延遲被取消，不需處理
		}
	}
}
