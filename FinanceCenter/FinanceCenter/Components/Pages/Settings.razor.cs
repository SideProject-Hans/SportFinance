using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 設定頁面
/// </summary>
public partial class Settings : IDisposable
{
	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	private List<Department> Departments { get; set; } = [];
	private List<BankInitialBalance> BankBalances { get; set; } = [];
	private int SelectedTab { get; set; }
	private List<int> AvailableYears { get; } = Enumerable.Range(2015, DateTime.Now.Year - 2015 + 6).ToList();
	private CancellationTokenSource? _saveCts;

	// 對話窗狀態
	private bool IsDialogVisible { get; set; }
	private bool IsEditMode { get; set; }
	private Department DialogDepartment { get; set; } = new();

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

	private void OpenAddDepartmentDialog()
	{
		DialogDepartment = new Department { IsActive = true };
		IsEditMode = false;
		IsDialogVisible = true;
	}

	private void OpenEditDepartmentDialog(Department department)
	{
		DialogDepartment = new Department
		{
			Id = department.Id,
			Code = department.Code,
			Name = department.Name,
			IsActive = department.IsActive,
			SortOrder = department.SortOrder
		};
		IsEditMode = true;
		IsDialogVisible = true;
	}

	private void CloseDialog()
	{
		IsDialogVisible = false;
	}

	private async Task HandleDepartmentSubmit(Department department)
	{
		if (IsEditMode)
		{
			if (await SettingsService.IsDepartmentCodeExistsAsync(department.Code, department.Id))
			{
				Snackbar.Add("部門代號已存在", Severity.Error);
				return;
			}

			await SettingsService.UpdateDepartmentAsync(department);
			Snackbar.Add("更新成功", Severity.Success);
		}
		else
		{
			if (await SettingsService.IsDepartmentCodeExistsAsync(department.Code))
			{
				Snackbar.Add("部門代號已存在", Severity.Error);
				return;
			}

			await SettingsService.AddDepartmentAsync(department);
			Snackbar.Add("新增成功", Severity.Success);
		}

		CloseDialog();
		await LoadDataAsync();
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
		_saveCts?.Dispose();
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
			// Debounce 觸發新的儲存 - 預期行為
		}
	}

	public void Dispose()
	{
		_saveCts?.Cancel();
		_saveCts?.Dispose();
		GC.SuppressFinalize(this);
	}
}
