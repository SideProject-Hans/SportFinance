using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 財務資料存取層介面
/// </summary>
public interface IFinanceRepository
{
	Task<List<CashFlow>> GetAllCashFlowsAsync();
	Task<CashFlow?> GetCashFlowByIdAsync(int id);
	Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department);
	Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate);
	void Add(CashFlow cashFlow);
	void Update(CashFlow cashFlow);
	void Delete(CashFlow cashFlow);
}
