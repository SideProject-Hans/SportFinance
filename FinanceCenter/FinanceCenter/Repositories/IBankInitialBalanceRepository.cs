using FinanceCenter.Data.Entities;

namespace FinanceCenter.Repositories;

/// <summary>
/// 銀行初始金額資料存取層介面
/// </summary>
public interface IBankInitialBalanceRepository
{
	Task<List<BankInitialBalance>> GetAllAsync();
	Task<BankInitialBalance?> GetByBankTypeAsync(string bankType);
	void Add(BankInitialBalance balance);
	void Update(BankInitialBalance balance);
}
