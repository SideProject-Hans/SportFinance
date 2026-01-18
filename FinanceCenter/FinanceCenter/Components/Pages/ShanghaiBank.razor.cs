using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 上海銀行帳戶明細頁面
/// </summary>
public partial class ShanghaiBank
{
	[Inject]
	private IShanghaiBankService ShanghaiBankService { get; set; } = null!;

	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Inject]
	private IWebHostEnvironment WebHostEnvironment { get; set; } = null!;

	private List<ShanghaiBankAccount> Accounts { get; set; } = new();
	private List<Department> Departments { get; set; } = new();

	/// <summary>
	/// 每筆紀錄的累計餘額（Key: 紀錄 Id, Value: 累計餘額）
	/// </summary>
	private Dictionary<int, decimal> AccountBalances { get; set; } = new();

	/// <summary>
	/// 目前選擇的年份
	/// </summary>
	private int SelectedYear { get; set; } = DateTime.Now.Year;

	// Loading 狀態
	private bool IsInitializing { get; set; }
	private bool IsDeleting { get; set; }

	// Dialog 狀態
	private bool IsAddDialogOpen { get; set; }
	private bool IsEditDialogOpen { get; set; }
	private bool IsConfirmDialogOpen { get; set; }
	private bool IsDeleteDialogOpen { get; set; }

	// 編輯/刪除中的紀錄
	private ShanghaiBankAccount? EditingAccount { get; set; }
	private ShanghaiBankAccount? DeletingAccount { get; set; }

	// 分頁相關
	private int _currentPage = 1;
	private int _pageSize = 15;
	private int TotalPages => Math.Max(1, (int)Math.Ceiling((double)Accounts.Count / _pageSize));

	protected override async Task OnInitializedAsync()
	{
		Departments = await SettingsService.GetAllDepartmentsAsync();
		await LoadDataAsync();
	}

	private string GetDepartmentDisplay(string code)
	{
		if (string.IsNullOrEmpty(code)) return "-";
		var dept = Departments.FirstOrDefault(d => d.Code == code);
		return dept != null ? $"{dept.Code} - {dept.Name}" : code;
	}

	/// <summary>
	/// 年份變更事件處理
	/// </summary>
	private async Task OnYearChangedAsync(ChangeEventArgs e)
	{
		if (int.TryParse(e.Value?.ToString(), out var year))
		{
			SelectedYear = year;
			_currentPage = 1;
			await LoadDataAsync();
		}
	}

	private async Task LoadDataAsync()
	{
		Accounts = await ShanghaiBankService.GetByYearAsync(SelectedYear);
		var openingBalance = await ShanghaiBankService.GetOpeningBalanceAsync(SelectedYear);
		CalculateAccountBalances(openingBalance);
	}

	/// <summary>
	/// 計算每筆紀錄的累計餘額
	/// </summary>
	private void CalculateAccountBalances(decimal openingBalance)
	{
		AccountBalances.Clear();
		var runningBalance = openingBalance;

		foreach (var account in Accounts)
		{
			runningBalance += account.NetAmount;
			AccountBalances[account.Id] = runningBalance;
		}
	}

	/// <summary>
	/// 取得指定紀錄的累計餘額
	/// </summary>
	private decimal GetBalance(int accountId)
	{
		return AccountBalances.TryGetValue(accountId, out var balance) ? balance : 0;
	}

	// 分頁方法
	private IEnumerable<ShanghaiBankAccount> GetPagedAccounts()
	{
		return Accounts
			.Skip((_currentPage - 1) * _pageSize)
			.Take(_pageSize);
	}

	private void GoToPage(int page)
	{
		if (page >= 1 && page <= TotalPages)
		{
			_currentPage = page;
		}
	}

	private void NextPage()
	{
		if (_currentPage < TotalPages)
		{
			_currentPage++;
		}
	}

	private void PreviousPage()
	{
		if (_currentPage > 1)
		{
			_currentPage--;
		}
	}

	private void OnPageSizeChanged(ChangeEventArgs e)
	{
		if (int.TryParse(e.Value?.ToString(), out var size))
		{
			_pageSize = size;
			_currentPage = 1;
		}
	}

	// 新增 Dialog 相關
	private void OpenAddDialog()
	{
		IsAddDialogOpen = true;
	}

	private void CloseAddDialog()
	{
		IsAddDialogOpen = false;
	}

	private async Task OnAddDialogSubmitAsync(ShanghaiBankAccount account)
	{
		await ShanghaiBankService.AddAsync(account);
		await LoadDataAsync();
		IsAddDialogOpen = false;
	}

	// 編輯 Dialog 相關
	private void OpenEditDialog(ShanghaiBankAccount account)
	{
		EditingAccount = account;
		IsEditDialogOpen = true;
	}

	private void CloseEditDialog()
	{
		IsEditDialogOpen = false;
		EditingAccount = null;
	}

	private async Task OnEditDialogSubmitAsync(ShanghaiBankAccount account)
	{
		await ShanghaiBankService.UpdateAsync(account);
		await LoadDataAsync();
		IsEditDialogOpen = false;
		EditingAccount = null;
	}

	// 刪除 Dialog 相關
	private void OpenDeleteDialog(ShanghaiBankAccount account)
	{
		DeletingAccount = account;
		IsDeleteDialogOpen = true;
	}

	private void CloseDeleteDialog()
	{
		IsDeleteDialogOpen = false;
		DeletingAccount = null;
	}

	private async Task OnConfirmDeleteAsync()
	{
		if (DeletingAccount == null) return;

		IsDeleting = true;
		StateHasChanged();

		try
		{
			await ShanghaiBankService.DeleteAsync(DeletingAccount.Id);
			await LoadDataAsync();
		}
		finally
		{
			IsDeleting = false;
			IsDeleteDialogOpen = false;
			DeletingAccount = null;
		}
	}

	// 初始化確認 Dialog 相關
	private void OpenInitializeDialog()
	{
		IsConfirmDialogOpen = true;
	}

	private void CloseConfirmDialog()
	{
		IsConfirmDialogOpen = false;
	}

	private async Task OnConfirmInitializeAsync()
	{
		IsInitializing = true;
		IsConfirmDialogOpen = false;
		StateHasChanged();

		try
		{
			var contentRoot = WebHostEnvironment.ContentRootPath;
			var excelFilePath = Path.Combine(contentRoot, "Doc", "Temp", "上海-收支表.xlsx");
			await ShanghaiBankService.ImportFromExcelAsync(excelFilePath);
			await LoadDataAsync();
		}
		finally
		{
			IsInitializing = false;
		}
	}
}
