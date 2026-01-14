using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 合作金庫帳戶明細頁面
/// </summary>
public partial class TaiwanCooperativeBank
{
	[Inject]
	private ITaiwanCooperativeBankService TaiwanCooperativeBankService { get; set; } = null!;

	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Inject]
	private IWebHostEnvironment WebHostEnvironment { get; set; } = null!;

	private List<TaiwanCooperativeBankAccount> Accounts { get; set; } = new();
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

	// 訊息
	private string? _successMessage;
	private string? _errorMessage;

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
		Accounts = await TaiwanCooperativeBankService.GetByYearAsync(SelectedYear);
		OpeningBalance = await TaiwanCooperativeBankService.GetOpeningBalanceAsync(SelectedYear);
	}

	private async Task RefreshDataAsync()
	{
		await LoadDataAsync();
	}

	// 分頁方法
	private IEnumerable<TaiwanCooperativeBankAccount> GetPagedAccounts()
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
	private void OpenAddDialogAsync()
	{
		IsAddDialogOpen = true;
	}

	private void CloseAddDialog()
	{
		IsAddDialogOpen = false;
	}

	private async Task OnAddDialogSubmitAsync(TaiwanCooperativeBankAccount account)
	{
		await TaiwanCooperativeBankService.AddAsync(account);
		await LoadDataAsync();
		IsAddDialogOpen = false;
	}

	// 確認 Dialog 相關
	private void OpenInitializeDialogAsync()
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
			var excelFilePath = Path.Combine(contentRoot, "Doc", "Temp", "合作-收支表.xlsx");
			var importedCount = await TaiwanCooperativeBankService.ImportFromExcelAsync(excelFilePath);
			
			_successMessage = $"成功匯入 {importedCount} 筆資料";
			await LoadDataAsync();
		}
		catch (FileNotFoundException ex)
		{
			_errorMessage = $"找不到 Excel 檔案: {ex.Message}";
		}
		catch (Exception ex)
		{
			_errorMessage = $"匯入失敗: {ex.Message}";
		}
		finally
		{
			IsInitializing = false;
		}
	}
}
