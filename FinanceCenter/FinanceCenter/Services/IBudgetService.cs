using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 預算服務層介面
/// </summary>
public interface IBudgetService
{
	/// <summary>
	/// 取得可選擇的年份清單（含下一年度邏輯）
	/// </summary>
	IEnumerable<int> GetAvailableYears();

	/// <summary>
	/// 取得指定年度所有部門預算
	/// </summary>
	Task<List<DepartmentBudget>> GetBudgetsByYearAsync(int year);

	/// <summary>
	/// 取得指定部門預算（含明細）
	/// </summary>
	Task<DepartmentBudget?> GetBudgetByIdAsync(int id);

	/// <summary>
	/// 取得指定年度和部門的預算
	/// </summary>
	Task<DepartmentBudget?> GetBudgetByYearAndDepartmentAsync(int year, string departmentCode);

	/// <summary>
	/// 檢查部門預算是否已存在
	/// </summary>
	Task<bool> IsBudgetExistsAsync(int year, string departmentCode);

	/// <summary>
	/// 新增部門預算
	/// </summary>
	Task<DepartmentBudget> CreateBudgetAsync(DepartmentBudget budget);

	/// <summary>
	/// 更新部門預算項目
	/// </summary>
	Task<DepartmentBudget> UpdateBudgetItemsAsync(int budgetId, List<BudgetItem> items);

	/// <summary>
	/// 刪除部門預算
	/// </summary>
	Task<bool> DeleteBudgetAsync(int id);
}
