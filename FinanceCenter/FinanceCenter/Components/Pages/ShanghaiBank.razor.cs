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

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	[Inject]
	private IWebHostEnvironment WebHostEnvironment { get; set; } = null!;

	private List<ShanghaiBankAccount> Accounts { get; set; } = new();

	private List<int> AvailableYears { get; set; } = new();

	private int SelectedYear { get; set; }

	private IEnumerable<ShanghaiBankAccount> FilteredAccounts => GetFilteredAccountsWithOpeningBalance();

	/// <summary>
	/// 取得包含期初餘額的篩選帳戶清單
	/// </summary>
	private IEnumerable<ShanghaiBankAccount> GetFilteredAccountsWithOpeningBalance()
	{
		var yearAccounts = Accounts
			.Where(x => x.RemittanceDate.HasValue && x.RemittanceDate.Value.Year == SelectedYear)
			.OrderBy(x => x.RemittanceDate)
			.ToList();

		// 計算上一年度的期末餘額作為本年度期初餘額
		var openingBalance = CalculateOpeningBalance(SelectedYear);

		if (openingBalance != 0)
		{
			// 建立期初餘額記錄
			var openingRecord = new ShanghaiBankAccount
			{
				Id = -1, // 使用負數 ID 表示這是虛擬記錄
				RemittanceDate = new DateOnly(SelectedYear, 1, 1),
				Department = "",
				Applicant = "",
				Reason = "期初餘額",
				Income = openingBalance > 0 ? openingBalance : 0,
				Expense = openingBalance < 0 ? Math.Abs(openingBalance) : 0,
				Fee = 0
			};

			yearAccounts.Insert(0, openingRecord);
		}

		return yearAccounts;
	}

	/// <summary>
	/// 計算指定年度的期初餘額（從最早年度開始累計到上一年度結束的總餘額）
	/// </summary>
	private decimal CalculateOpeningBalance(int year)
	{
		// 取得所有早於指定年度的資料，計算累計淨金額
		// NetAmount = Income - Expense - Fee，所以直接加總所有之前年度的 NetAmount
		var previousYearsNetAmount = Accounts
			.Where(x => x.RemittanceDate.HasValue && x.RemittanceDate.Value.Year < year)
			.Sum(x => x.NetAmount);

		return previousYearsNetAmount;
	}

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		Accounts = await ShanghaiBankService.GetAllAsync();
		UpdateAvailableYears();
	}

	private void UpdateAvailableYears()
	{
		AvailableYears = Accounts
			.Where(x => x.RemittanceDate.HasValue)
			.Select(x => x.RemittanceDate!.Value.Year)
			.Distinct()
			.OrderByDescending(x => x)
			.ToList();

		if (AvailableYears.Count != 0 && !AvailableYears.Contains(SelectedYear))
		{
			SelectedYear = AvailableYears.First();
		}
	}

	private void OnYearSelected(int year)
	{
		SelectedYear = year;
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

	private async Task OpenInitializeDialogAsync()
	{
		var parameters = new DialogParameters<ConfirmDialog>
		{
			{ x => x.ContentText, "此操作會清空所有現有資料，並從 Excel 檔案重新匯入。確定要繼續嗎？" },
			{ x => x.ButtonText, "確認初始化" },
			{ x => x.Color, Color.Error }
		};

		var options = new DialogOptions { CloseOnEscapeKey = true };
		var dialog = await DialogService.ShowAsync<ConfirmDialog>("初始化資料確認", parameters, options);
		var result = await dialog.Result;

		if (result is { Canceled: false })
		{
			await InitializeDataAsync();
		}
	}

	private async Task InitializeDataAsync()
	{
		try
		{
			// Excel 檔案路徑
			var contentRoot = WebHostEnvironment.ContentRootPath;
			var excelFilePath = Path.Combine(contentRoot, "Doc", "Temp", "上海-收支表.xlsx");

			var importedCount = await ShanghaiBankService.ImportFromExcelAsync(excelFilePath);
			
			Snackbar.Add($"成功匯入 {importedCount} 筆資料", Severity.Success);
			await LoadDataAsync();
		}
		catch (FileNotFoundException ex)
		{
			Snackbar.Add($"找不到 Excel 檔案: {ex.Message}", Severity.Error);
		}
		catch (Exception ex)
		{
			Snackbar.Add($"匯入失敗: {ex.Message}", Severity.Error);
		}
	}
}
