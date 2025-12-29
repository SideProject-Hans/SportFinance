using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 財務資料存取層
/// </summary>
public class FinanceRepository
{
    private readonly FinanceCenterDbContext _context;

    public FinanceRepository(FinanceCenterDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 取得所有現金流紀錄
    /// </summary>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetAllCashFlowsAsync()
    {
        return await _context.CashFlows
            .OrderByDescending(c => c.CreateDay)
            .ToListAsync();
    }

    /// <summary>
    /// 根據 Id 取得現金流紀錄
    /// </summary>
    /// <param name="id">紀錄識別碼</param>
    /// <returns>現金流紀錄</returns>
    public async Task<CashFlow?> GetCashFlowByIdAsync(int id)
    {
        return await _context.CashFlows.FindAsync(id);
    }

    /// <summary>
    /// 根據部門取得現金流紀錄
    /// </summary>
    /// <param name="department">部門名稱</param>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department)
    {
        return await _context.CashFlows
            .Where(c => c.Department == department)
            .OrderByDescending(c => c.CreateDay)
            .ToListAsync();
    }

    /// <summary>
    /// 根據日期範圍取得現金流紀錄
    /// </summary>
    /// <param name="startDate">開始日期</param>
    /// <param name="endDate">結束日期</param>
    /// <returns>現金流紀錄清單</returns>
    public async Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.CashFlows
            .Where(c => c.CreateDay >= startDate && c.CreateDay <= endDate)
            .OrderByDescending(c => c.CreateDay)
            .ToListAsync();
    }

    /// <summary>
    /// 新增現金流紀錄
    /// </summary>
    /// <param name="cashFlow">現金流實體</param>
    /// <returns>新增的現金流紀錄</returns>
    public async Task<CashFlow> AddCashFlowAsync(CashFlow cashFlow)
    {
        _context.CashFlows.Add(cashFlow);
        await _context.SaveChangesAsync();
        return cashFlow;
    }

    /// <summary>
    /// 更新現金流紀錄
    /// </summary>
    /// <param name="cashFlow">現金流實體</param>
    /// <returns>更新後的現金流紀錄</returns>
    public async Task<CashFlow> UpdateCashFlowAsync(CashFlow cashFlow)
    {
        _context.CashFlows.Update(cashFlow);
        await _context.SaveChangesAsync();
        return cashFlow;
    }

    /// <summary>
    /// 刪除現金流紀錄
    /// </summary>
    /// <param name="id">紀錄識別碼</param>
    /// <returns>是否刪除成功</returns>
    public async Task<bool> DeleteCashFlowAsync(int id)
    {
        var cashFlow = await _context.CashFlows.FindAsync(id);
        if (cashFlow is null)
        {
            return false;
        }

        _context.CashFlows.Remove(cashFlow);
        await _context.SaveChangesAsync();
        return true;
    }
}
