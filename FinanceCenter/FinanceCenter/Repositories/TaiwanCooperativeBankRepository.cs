using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 合作金庫帳戶資料存取層
/// </summary>
public class TaiwanCooperativeBankRepository(FinanceCenterDbContext context) : ITaiwanCooperativeBankRepository
{
	public async Task<List<TaiwanCooperativeBankAccount>> GetAllAsync()
	{
		return await context.TaiwanCooperativeBankAccounts
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public async Task<TaiwanCooperativeBankAccount?> GetByIdAsync(int id)
	{
		return await context.TaiwanCooperativeBankAccounts.FindAsync(id);
	}

	public async Task<List<TaiwanCooperativeBankAccount>> GetByDepartmentAsync(string department)
	{
		return await context.TaiwanCooperativeBankAccounts
			.Where(c => c.Department == department)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public async Task<List<TaiwanCooperativeBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await context.TaiwanCooperativeBankAccounts
			.Where(c => c.CreateDay >= startDate && c.CreateDay <= endDate)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public void Add(TaiwanCooperativeBankAccount account)
	{
		context.TaiwanCooperativeBankAccounts.Add(account);
	}

	public void Update(TaiwanCooperativeBankAccount account)
	{
		context.TaiwanCooperativeBankAccounts.Update(account);
	}

	public void Delete(TaiwanCooperativeBankAccount account)
	{
		context.TaiwanCooperativeBankAccounts.Remove(account);
	}
}
