using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 設定服務層介面
/// </summary>
public interface ISettingsService
{
	// 部門相關
	Task<List<Department>> GetAllDepartmentsAsync();
	Task<List<Department>> GetActiveDepartmentsAsync();
	Task<Department?> GetDepartmentByIdAsync(int id);
	Task<Department?> GetDepartmentByCodeAsync(string code);
	Task<Department> AddDepartmentAsync(Department department);
	Task<Department> UpdateDepartmentAsync(Department department);
	Task<bool> DeleteDepartmentAsync(int id);
	Task<bool> IsDepartmentCodeExistsAsync(string code, int? excludeId = null);

	// 銀行初始金額相關
	Task<List<BankInitialBalance>> GetAllBankInitialBalancesAsync();
	Task<BankInitialBalance?> GetBankInitialBalanceAsync(string bankType);
	Task SaveBankInitialBalanceAsync(BankInitialBalance balance);
}
