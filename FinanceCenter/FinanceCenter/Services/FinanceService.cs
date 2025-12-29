using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 財務服務層
/// </summary>
public class FinanceService
{
    private readonly FinanceRepository _financeRepository;

    public FinanceService(FinanceRepository financeRepository)
    {
        _financeRepository = financeRepository;
    }

    /// <summary>
    /// 取得所有現金流紀錄
    /// </summary>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetAllCashFlowsAsync()
    {
        return await _financeRepository.GetAllCashFlowsAsync();
    }

    /// <summary>
    /// 根據 Id 取得現金流紀錄
    /// </summary>
    /// <param name="id">紀錄識別碼</param>
    /// <returns>現金流紀錄</returns>
    public async Task<CashFlow?> GetCashFlowByIdAsync(int id)
    {
        return await _financeRepository.GetCashFlowByIdAsync(id);
    }

    /// <summary>
    /// 根據部門取得現金流紀錄
    /// </summary>
    /// <param name="department">部門名稱</param>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department)
    {
        return await _financeRepository.GetCashFlowsByDepartmentAsync(department);
    }

    /// <summary>
    /// 根據日期範圍取得現金流紀錄
    /// </summary>
    /// <param name="startDate">開始日期</param>
    /// <param name="endDate">結束日期</param>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _financeRepository.GetCashFlowsByDateRangeAsync(startDate, endDate);
    }

    /// <summary>
    /// 新增現金流紀錄
    /// </summary>
    /// <param name="cashFlow">現金流實體</param>
    /// <returns>新增的現金流紀錄</returns>
    public async Task<CashFlow> AddCashFlowAsync(CashFlow cashFlow)
    {
        return await _financeRepository.AddCashFlowAsync(cashFlow);
    }

    /// <summary>
    /// 更新現金流紀錄
    /// </summary>
    /// <param name="cashFlow">現金流實體</param>
    /// <returns>更新後的現金流紀錄</returns>
    public async Task<CashFlow> UpdateCashFlowAsync(CashFlow cashFlow)
    {
        return await _financeRepository.UpdateCashFlowAsync(cashFlow);
    }

    /// <summary>
    /// 刪除現金流紀錄
    /// </summary>
    /// <param name="id">紀錄識別碼</param>
    /// <returns>是否刪除成功</returns>
    public async Task<bool> DeleteCashFlowAsync(int id)
    {
        return await _financeRepository.DeleteCashFlowAsync(id);
    }
}
