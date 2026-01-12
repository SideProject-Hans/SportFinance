using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 預算服務層
/// </summary>
public class BudgetService(IUnitOfWork unitOfWork) : IBudgetService
{
	private const int YearRangeStart = -2;
	private const int YearRangeEnd = 1;
	private const int NextYearVisibleDay = 22;
	private const int NextYearVisibleMonth = 12;

	public IEnumerable<int> GetAvailableYears()
	{
		var now = DateTime.Now;
		var currentYear = now.Year;
		var endYear = currentYear + YearRangeEnd;

		// 12/22 起顯示下一年選項
		if (now.Month == NextYearVisibleMonth && now.Day >= NextYearVisibleDay)
		{
			endYear = currentYear + YearRangeEnd + 1;
		}

		var startYear = currentYear + YearRangeStart;
		return Enumerable.Range(startYear, endYear - startYear + 1);
	}

	public async Task<List<DepartmentBudget>> GetBudgetsByYearAsync(int year)
	{
		return await unitOfWork.Budget.GetByYearAsync(year);
	}

	public async Task<DepartmentBudget?> GetBudgetByIdAsync(int id)
	{
		return await unitOfWork.Budget.GetByIdWithItemsAsync(id);
	}

	public async Task<DepartmentBudget?> GetBudgetByYearAndDepartmentAsync(int year, string departmentCode)
	{
		return await unitOfWork.Budget.GetByYearAndDepartmentAsync(year, departmentCode);
	}

	public async Task<bool> IsBudgetExistsAsync(int year, string departmentCode)
	{
		return await unitOfWork.Budget.ExistsAsync(year, departmentCode);
	}

	public async Task<DepartmentBudget> CreateBudgetAsync(DepartmentBudget budget)
	{
		// 檢查是否已存在
		if (await unitOfWork.Budget.ExistsAsync(budget.Year, budget.DepartmentCode))
		{
			throw new InvalidOperationException($"部門 {budget.DepartmentCode} 在 {budget.Year} 年度已有預算");
		}

		unitOfWork.Budget.Add(budget);
		await unitOfWork.SaveChangesAsync();
		return budget;
	}

	public async Task<DepartmentBudget> UpdateBudgetItemsAsync(int budgetId, List<BudgetItem> items)
	{
		var existing = await unitOfWork.Budget.GetByIdWithItemsAsync(budgetId);
		if (existing is null)
		{
			throw new InvalidOperationException("預算不存在");
		}

		// 刪除舊的明細項目
		unitOfWork.Budget.DeleteItems(existing.BudgetItems);

		// 新增新的明細項目
		foreach (var item in items)
		{
			item.DepartmentBudgetId = budgetId;
			item.Id = 0; // 確保是新增
			unitOfWork.Budget.AddItem(item);
		}

		await unitOfWork.SaveChangesAsync();
		return (await unitOfWork.Budget.GetByIdWithItemsAsync(budgetId))!;
	}

	public async Task<bool> DeleteBudgetAsync(int id)
	{
		var budget = await unitOfWork.Budget.GetByIdWithItemsAsync(id);
		if (budget is null)
		{
			return false;
		}

		// 先刪除明細項目
		unitOfWork.Budget.DeleteItems(budget.BudgetItems);
		unitOfWork.Budget.Delete(budget);
		await unitOfWork.SaveChangesAsync();
		return true;
	}
}
