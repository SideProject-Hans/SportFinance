using FinanceCenter.Data;
using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Repositories;

/// <summary>
/// 銀行初始金額資料存取層
/// </summary>
public class BankInitialBalanceRepository(FinanceCenterDbContext context) : IBankInitialBalanceRepository
{
	public async Task<List<BankInitialBalance>> GetAllAsync()
	{
		return await context.BankInitialBalances.ToListAsync();
	}

	public async Task<BankInitialBalance?> GetByBankTypeAsync(string bankType)
	{
		return await context.BankInitialBalances
			.FirstOrDefaultAsync(b => b.BankType == bankType);
	}

	public void Add(BankInitialBalance balance)
	{
		context.BankInitialBalances.Add(balance);
	}

	public void Update(BankInitialBalance balance)
	{
		context.BankInitialBalances.Update(balance);
	}
}
