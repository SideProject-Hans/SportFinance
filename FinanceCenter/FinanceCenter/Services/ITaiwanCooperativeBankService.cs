using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 合作金庫服務介面
/// </summary>
public interface ITaiwanCooperativeBankService
{
	Task<List<TaiwanCooperativeBankAccount>> GetAllAsync();
	Task<TaiwanCooperativeBankAccount?> GetByIdAsync(int id);
	Task<List<TaiwanCooperativeBankAccount>> GetByDepartmentAsync(string department);
	Task<List<TaiwanCooperativeBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<TaiwanCooperativeBankAccount> AddAsync(TaiwanCooperativeBankAccount account);
	Task<TaiwanCooperativeBankAccount> UpdateAsync(TaiwanCooperativeBankAccount account);
	Task<bool> DeleteAsync(int id);
	Task<int> ImportFromExcelAsync(string filePath);
}
