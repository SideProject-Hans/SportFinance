using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 財務資料存取層
/// </summary>
public class FinanceRepository(FinanceCenterDbContext context) : IFinanceRepository
{
	/// <summary>
	/// 取得所有現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetAllCashFlowsAsync()
	{
		return await context.CashFlows
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	/// <summary>
	/// 根據 Id 取得現金流紀錄
	/// </summary>
	public async Task<CashFlow?> GetCashFlowByIdAsync(int id)
	{
		return await context.CashFlows.FindAsync(id);
	}

	/// <summary>
	/// 根據部門取得現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department)
	{
		return await context.CashFlows
			.Where(c => c.Department == department)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	/// <summary>
	/// 根據日期範圍取得現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await context.CashFlows
			.Where(c => c.CreateDay >= startDate && c.CreateDay <= endDate)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	/// <summary>
	/// 新增現金流紀錄（追蹤變更，不立即存檔）
	/// </summary>
	public void Add(CashFlow cashFlow)
	{
		context.CashFlows.Add(cashFlow);
	}

	/// <summary>
	/// 更新現金流紀錄（追蹤變更，不立即存檔）
	/// </summary>
	public void Update(CashFlow cashFlow)
	{
		context.CashFlows.Update(cashFlow);
	}

	/// <summary>
	/// 刪除現金流紀錄（追蹤變更，不立即存檔）
	/// </summary>
	public void Delete(CashFlow cashFlow)
	{
		context.CashFlows.Remove(cashFlow);
	}
}
