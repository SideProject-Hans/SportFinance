using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 上海銀行服務介面
/// </summary>
public interface IShanghaiBankService
{
	Task<List<ShanghaiBankAccount>> GetAllAsync();
	Task<ShanghaiBankAccount?> GetByIdAsync(int id);
	Task<List<ShanghaiBankAccount>> GetByDepartmentAsync(string department);
	Task<List<ShanghaiBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<ShanghaiBankAccount> AddAsync(ShanghaiBankAccount account);
	Task<ShanghaiBankAccount> UpdateAsync(ShanghaiBankAccount account);
	Task<bool> DeleteAsync(int id);
}
