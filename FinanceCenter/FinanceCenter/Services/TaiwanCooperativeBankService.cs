using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 合作金庫服務層
/// </summary>
public class TaiwanCooperativeBankService(IUnitOfWork unitOfWork) : ITaiwanCooperativeBankService
{
	public async Task<List<TaiwanCooperativeBankAccount>> GetAllAsync()
	{
		return await unitOfWork.TaiwanCooperativeBank.GetAllAsync();
	}

	public async Task<TaiwanCooperativeBankAccount?> GetByIdAsync(int id)
	{
		return await unitOfWork.TaiwanCooperativeBank.GetByIdAsync(id);
	}

	public async Task<List<TaiwanCooperativeBankAccount>> GetByDepartmentAsync(string department)
	{
		return await unitOfWork.TaiwanCooperativeBank.GetByDepartmentAsync(department);
	}

	public async Task<List<TaiwanCooperativeBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await unitOfWork.TaiwanCooperativeBank.GetByDateRangeAsync(startDate, endDate);
	}

	public async Task<TaiwanCooperativeBankAccount> AddAsync(TaiwanCooperativeBankAccount account)
	{
		unitOfWork.TaiwanCooperativeBank.Add(account);
		await unitOfWork.SaveChangesAsync();
		return account;
	}

	public async Task<TaiwanCooperativeBankAccount> UpdateAsync(TaiwanCooperativeBankAccount account)
	{
		unitOfWork.TaiwanCooperativeBank.Update(account);
		await unitOfWork.SaveChangesAsync();
		return account;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var account = await unitOfWork.TaiwanCooperativeBank.GetByIdAsync(id);
		if (account is null)
		{
			return false;
		}

		unitOfWork.TaiwanCooperativeBank.Delete(account);
		await unitOfWork.SaveChangesAsync();
		return true;
	}
}
