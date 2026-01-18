using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 預算資料存取層介面
/// </summary>
public interface IBudgetRepository
{
	// 部門預算
	Task<List<DepartmentBudget>> GetByYearAsync(int year);
	Task<DepartmentBudget?> GetByYearAndDepartmentAsync(int year, string departmentCode);
	Task<DepartmentBudget?> GetByIdWithItemsAsync(int id);
	Task<bool> ExistsAsync(int year, string departmentCode);
	void Add(DepartmentBudget budget);
	void Update(DepartmentBudget budget);
	void Delete(DepartmentBudget budget);

	// 預算項目明細
	void AddItem(BudgetItem item);
	void UpdateItem(BudgetItem item);
	void DeleteItem(BudgetItem item);
	void DeleteItems(IEnumerable<BudgetItem> items);
}
