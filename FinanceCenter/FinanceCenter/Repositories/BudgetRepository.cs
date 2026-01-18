using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 預算資料存取層
/// </summary>
public class BudgetRepository(FinanceCenterDbContext context) : IBudgetRepository
{
	public async Task<List<DepartmentBudget>> GetByYearAsync(int year)
	{
		return await context.DepartmentBudgets
			.Include(b => b.BudgetItems)
			.Where(b => b.Year == year)
			.OrderBy(b => b.DepartmentCode)
			.ToListAsync();
	}

	public async Task<DepartmentBudget?> GetByYearAndDepartmentAsync(int year, string departmentCode)
	{
		return await context.DepartmentBudgets
			.Include(b => b.BudgetItems.OrderBy(i => i.SortOrder))
			.FirstOrDefaultAsync(b => b.Year == year && b.DepartmentCode == departmentCode);
	}

	public async Task<DepartmentBudget?> GetByIdWithItemsAsync(int id)
	{
		return await context.DepartmentBudgets
			.Include(b => b.BudgetItems.OrderBy(i => i.SortOrder))
			.FirstOrDefaultAsync(b => b.Id == id);
	}

	public async Task<bool> ExistsAsync(int year, string departmentCode)
	{
		return await context.DepartmentBudgets
			.AnyAsync(b => b.Year == year && b.DepartmentCode == departmentCode);
	}

	public void Add(DepartmentBudget budget) => context.DepartmentBudgets.Add(budget);

	public void Update(DepartmentBudget budget) => context.DepartmentBudgets.Update(budget);

	public void Delete(DepartmentBudget budget) => context.DepartmentBudgets.Remove(budget);

	public void AddItem(BudgetItem item) => context.BudgetItems.Add(item);

	public void UpdateItem(BudgetItem item) => context.BudgetItems.Update(item);

	public void DeleteItem(BudgetItem item) => context.BudgetItems.Remove(item);

	public void DeleteItems(IEnumerable<BudgetItem> items) => context.BudgetItems.RemoveRange(items);
}
