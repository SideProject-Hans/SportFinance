using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 合作金庫帳戶資料存取層介面
/// </summary>
public interface ITaiwanCooperativeBankRepository
{
	Task<List<TaiwanCooperativeBankAccount>> GetAllAsync();
	Task<TaiwanCooperativeBankAccount?> GetByIdAsync(int id);
	Task<List<TaiwanCooperativeBankAccount>> GetByDepartmentAsync(string department);
	Task<List<TaiwanCooperativeBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	void Add(TaiwanCooperativeBankAccount account);
	void Update(TaiwanCooperativeBankAccount account);
	void Delete(TaiwanCooperativeBankAccount account);
}
