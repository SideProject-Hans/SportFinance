using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 財務服務介面
/// </summary>
public interface IFinanceService
{
	Task<List<CashFlow>> GetAllCashFlowsAsync();
	Task<CashFlow?> GetCashFlowByIdAsync(int id);
	Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department);
	Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<CashFlow> AddCashFlowAsync(CashFlow cashFlow);
	Task<CashFlow> UpdateCashFlowAsync(CashFlow cashFlow);
	Task<bool> DeleteCashFlowAsync(int id);
}
