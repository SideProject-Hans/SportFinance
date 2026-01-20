using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 年度預算頁面
/// </summary>
public partial class AnnualBudget
{
	[Inject]
	private IBudgetService BudgetService { get; set; } = null!;

	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	// 資料狀態
	private List<DepartmentBudget> Budgets { get; set; } = new();
	private List<Department> Departments { get; set; } = new();
	private IEnumerable<int> AvailableYears { get; set; } = Enumerable.Empty<int>();
	private int SelectedYear { get; set; }
	private DepartmentBudget? SelectedBudget { get; set; }

	// 編輯狀態
	private List<BudgetItem> EditingItems { get; set; } = new();
	private List<BudgetItem> OriginalItems { get; set; } = new();

	// 計算屬性
	private decimal YearlyTotalBudget => Budgets.Sum(b => b.TotalAmount);
	private bool HasChanges => !ItemsEqual(EditingItems, OriginalItems);

	protected override async Task OnInitializedAsync()
	{
		AvailableYears = BudgetService.GetAvailableYears();
		SelectedYear = DateTime.Now.Year;
		// 載入所有部門（含停用），以便顯示歷史記錄的部門名稱
		Departments = await SettingsService.GetAllDepartmentsAsync();
		await LoadBudgetsAsync();
	}

	/// <summary>
	/// 處理年度選擇變更 (原生 HTML select)
	/// </summary>
	private async Task OnYearSelectChanged(ChangeEventArgs e)
	{
		if (int.TryParse(e.Value?.ToString(), out var year))
		{
			await OnYearChangedAsync(year);
		}
	}

	private async Task OnYearChangedAsync(int year)
	{
		SelectedYear = year;
		SelectedBudget = null;
		EditingItems.Clear();
		OriginalItems.Clear();
		await LoadBudgetsAsync();
	}

	private async Task LoadBudgetsAsync()
	{
		Budgets = await BudgetService.GetBudgetsByYearAsync(SelectedYear);

		// 自動為尚未建立預算的啟用部門建立預算記錄
		var departmentsWithoutBudget = Departments
			.Where(d => d.IsActive && !Budgets.Any(b => b.DepartmentCode == d.Code))
			.ToList();

		foreach (var dept in departmentsWithoutBudget)
		{
			var budget = new DepartmentBudget
			{
				Year = SelectedYear,
				DepartmentCode = dept.Code
			};
			await BudgetService.CreateBudgetAsync(budget);
		}

		// 如果有新增預算，重新載入清單
		if (departmentsWithoutBudget.Any())
		{
			Budgets = await BudgetService.GetBudgetsByYearAsync(SelectedYear);
		}
	}

	private string GetDepartmentName(string code)
	{
		return Departments.FirstOrDefault(d => d.Code == code)?.Name ?? code;
	}

	private string GetBudgetItemClass(DepartmentBudget budget)
	{
		return SelectedBudget?.Id == budget.Id ? "mud-primary-text" : "";
	}

	private async Task SelectBudgetAsync(DepartmentBudget budget)
	{
		SelectedBudget = await BudgetService.GetBudgetByIdAsync(budget.Id);
		LoadEditingItems();
	}

	private void LoadEditingItems()
	{
		if (SelectedBudget is null) return;

		EditingItems = SelectedBudget.BudgetItems.Select(CloneItem).ToList();
		OriginalItems = SelectedBudget.BudgetItems.Select(CloneItem).ToList();
	}

	private static BudgetItem CloneItem(BudgetItem item) => new()
	{
		Id = item.Id,
		DepartmentBudgetId = item.DepartmentBudgetId,
		ItemName = item.ItemName,
		Amount = item.Amount,
		Description = item.Description,
		SortOrder = item.SortOrder
	};

	private void AddItem()
	{
		EditingItems.Add(new BudgetItem
		{
			SortOrder = EditingItems.Count
		});
	}

	private void RemoveItem(int index)
	{
		EditingItems.RemoveAt(index);
		ReorderItems();
	}

	private void ReorderItems()
	{
		for (var i = 0; i < EditingItems.Count; i++)
		{
			EditingItems[i].SortOrder = i;
		}
	}

	private async Task SaveBudgetAsync()
	{
		if (SelectedBudget is null) return;

		try
		{
			// 過濾空白項目
			var validItems = EditingItems
				.Where(i => !string.IsNullOrWhiteSpace(i.ItemName))
				.ToList();

			var budgetId = SelectedBudget.Id;
			await BudgetService.UpdateBudgetItemsAsync(budgetId, validItems);
			await LoadBudgetsAsync();

			// 重新載入選取的預算
			SelectedBudget = await BudgetService.GetBudgetByIdAsync(budgetId);
			if (SelectedBudget is not null)
			{
				LoadEditingItems();
			}

			Snackbar.Add("儲存成功", Severity.Success);
		}
		catch (Exception ex)
		{
			Snackbar.Add($"儲存失敗：{ex.Message}", Severity.Error);
		}
	}

	private async Task DeleteBudgetAsync(DepartmentBudget budget)
	{
		var deptName = GetDepartmentName(budget.DepartmentCode);
		
		var parameters = new DialogParameters<GlassConfirmDialog>
		{
			{ x => x.Title, "確認刪除" },
			{ x => x.ContentText, $"確定要刪除「{deptName}」的 {SelectedYear} 年度預算嗎？" },
			{ x => x.WarningText, "此操作無法復原" },
			{ x => x.ButtonText, "刪除" },
			{ x => x.Type, GlassConfirmDialog.DialogType.Danger }
		};

		var options = new DialogOptions
		{
			MaxWidth = MaxWidth.Small,
			FullWidth = true,
			BackdropClick = false
		};

		var dialog = await DialogService.ShowAsync<GlassConfirmDialog>("確認刪除", parameters, options);
		var result = await dialog.Result;

		if (result is null || result.Canceled) return;

		try
		{
			await BudgetService.DeleteBudgetAsync(budget.Id);
			
			// 如果刪除的是目前選取的預算，清除選取狀態
			if (SelectedBudget?.Id == budget.Id)
			{
				SelectedBudget = null;
				EditingItems.Clear();
				OriginalItems.Clear();
			}
			
			await LoadBudgetsAsync();
			Snackbar.Add("已刪除預算", Severity.Success);
		}
		catch (Exception ex)
		{
			Snackbar.Add($"刪除失敗：{ex.Message}", Severity.Error);
		}
	}

	private async Task ClearBudgetItemsAsync()
	{
		if (SelectedBudget is null) return;

		var deptName = GetDepartmentName(SelectedBudget.DepartmentCode);
		
		var parameters = new DialogParameters<GlassConfirmDialog>
		{
			{ x => x.Title, "確認清除" },
			{ x => x.ContentText, $"確定要清除「{deptName}」的所有預算項目嗎？" },
			{ x => x.ButtonText, "清除" },
			{ x => x.Type, GlassConfirmDialog.DialogType.Warning }
		};

		var options = new DialogOptions
		{
			MaxWidth = MaxWidth.Small,
			FullWidth = true,
			BackdropClick = false
		};

		var dialog = await DialogService.ShowAsync<GlassConfirmDialog>("確認清除", parameters, options);
		var result = await dialog.Result;

		if (result is null || result.Canceled) return;

		EditingItems.Clear();
		Snackbar.Add("已清除所有項目，請點擊儲存以套用變更", Severity.Info);
	}

	private static bool ItemsEqual(List<BudgetItem> a, List<BudgetItem> b)
	{
		if (a.Count != b.Count) return false;

		for (var i = 0; i < a.Count; i++)
		{
			if (a[i].ItemName != b[i].ItemName ||
				a[i].Amount != b[i].Amount ||
				a[i].Description != b[i].Description)
			{
				return false;
			}
		}

		return true;
	}
}
