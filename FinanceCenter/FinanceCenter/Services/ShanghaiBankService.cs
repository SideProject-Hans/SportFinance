using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 上海銀行服務層
/// </summary>
public class ShanghaiBankService(IUnitOfWork unitOfWork) : IShanghaiBankService
{
	public async Task<List<ShanghaiBankAccount>> GetAllAsync()
	{
		return await unitOfWork.ShanghaiBank.GetAllAsync();
	}

	public async Task<ShanghaiBankAccount?> GetByIdAsync(int id)
	{
		return await unitOfWork.ShanghaiBank.GetByIdAsync(id);
	}

	public async Task<List<ShanghaiBankAccount>> GetByDepartmentAsync(string department)
	{
		return await unitOfWork.ShanghaiBank.GetByDepartmentAsync(department);
	}

	public async Task<List<ShanghaiBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await unitOfWork.ShanghaiBank.GetByDateRangeAsync(startDate, endDate);
	}

	public async Task<ShanghaiBankAccount> AddAsync(ShanghaiBankAccount account)
	{
		unitOfWork.ShanghaiBank.Add(account);
		await unitOfWork.SaveChangesAsync();
		return account;
	}

	public async Task<ShanghaiBankAccount> UpdateAsync(ShanghaiBankAccount account)
	{
		unitOfWork.ShanghaiBank.Update(account);
		await unitOfWork.SaveChangesAsync();
		return account;
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var account = await unitOfWork.ShanghaiBank.GetByIdAsync(id);
		if (account is null)
		{
			return false;
		}

		unitOfWork.ShanghaiBank.Delete(account);
		await unitOfWork.SaveChangesAsync();
		return true;
	}
}
