using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 上海銀行帳戶資料存取層介面
/// </summary>
public interface IShanghaiBankRepository
{
	Task<List<ShanghaiBankAccount>> GetAllAsync();
	Task<ShanghaiBankAccount?> GetByIdAsync(int id);
	Task<List<ShanghaiBankAccount>> GetByDepartmentAsync(string department);
	Task<List<ShanghaiBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	void Add(ShanghaiBankAccount account);
	void AddRange(IEnumerable<ShanghaiBankAccount> accounts);
	void Update(ShanghaiBankAccount account);
	void Delete(ShanghaiBankAccount account);
	Task ClearAllAsync();
}
