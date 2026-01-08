using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 上海銀行帳戶資料存取層
/// </summary>
public class ShanghaiBankRepository(FinanceCenterDbContext context) : IShanghaiBankRepository
{
	public async Task<List<ShanghaiBankAccount>> GetAllAsync()
	{
		return await context.ShanghaiBankAccounts
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public async Task<ShanghaiBankAccount?> GetByIdAsync(int id)
	{
		return await context.ShanghaiBankAccounts.FindAsync(id);
	}

	public async Task<List<ShanghaiBankAccount>> GetByDepartmentAsync(string department)
	{
		return await context.ShanghaiBankAccounts
			.Where(c => c.Department == department)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public async Task<List<ShanghaiBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await context.ShanghaiBankAccounts
			.Where(c => c.CreateDay >= startDate && c.CreateDay <= endDate)
			.OrderByDescending(c => c.CreateDay)
			.ToListAsync();
	}

	public void Add(ShanghaiBankAccount account)
	{
		context.ShanghaiBankAccounts.Add(account);
	}

	public void AddRange(IEnumerable<ShanghaiBankAccount> accounts)
	{
		context.ShanghaiBankAccounts.AddRange(accounts);
	}

	public void Update(ShanghaiBankAccount account)
	{
		context.ShanghaiBankAccounts.Update(account);
	}

	public void Delete(ShanghaiBankAccount account)
	{
		context.ShanghaiBankAccounts.Remove(account);
	}

	public async Task ClearAllAsync()
	{
		var allAccounts = await context.ShanghaiBankAccounts.ToListAsync();
		context.ShanghaiBankAccounts.RemoveRange(allAccounts);
	}
}
