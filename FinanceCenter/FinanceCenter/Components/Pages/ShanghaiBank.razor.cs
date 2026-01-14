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
	/// 目前選擇的年份
	/// </summary>
	private int SelectedYear { get; set; } = DateTime.Now.Year;

	/// <summary>
	/// 年初餘額
	/// </summary>
	private decimal OpeningBalance { get; set; }

	/// <summary>
	/// 本年度淨金額總和
	/// </summary>
	private decimal CurrentYearNetAmount => Accounts.Sum(a => a.NetAmount);

	/// <summary>
	/// 目前餘額（年初餘額 + 本年度淨金額）
	/// </summary>
	private decimal CurrentBalance => OpeningBalance + CurrentYearNetAmount;

	// Loading 狀態
	private bool IsInitializing { get; set; }

	// Dialog 狀態
	private bool IsAddDialogOpen { get; set; }
	private bool IsConfirmDialogOpen { get; set; }

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
		OpeningBalance = await ShanghaiBankService.GetOpeningBalanceAsync(SelectedYear);
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

	// 確認 Dialog 相關
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
