using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 設定服務層
/// </summary>
public class SettingsService(IUnitOfWork unitOfWork) : ISettingsService
{
	// 部門相關
	public async Task<List<Department>> GetAllDepartmentsAsync()
	{
		return await unitOfWork.Department.GetAllAsync();
	}

	public async Task<List<Department>> GetActiveDepartmentsAsync()
	{
		return await unitOfWork.Department.GetActiveAsync();
	}

	public async Task<Department?> GetDepartmentByIdAsync(int id)
	{
		return await unitOfWork.Department.GetByIdAsync(id);
	}

	public async Task<Department?> GetDepartmentByCodeAsync(string code)
	{
		return await unitOfWork.Department.GetByCodeAsync(code);
	}

	public async Task<Department> AddDepartmentAsync(Department department)
	{
		unitOfWork.Department.Add(department);
		await unitOfWork.SaveChangesAsync();
		return department;
	}

	public async Task<Department> UpdateDepartmentAsync(Department department)
	{
		unitOfWork.Department.Update(department);
		await unitOfWork.SaveChangesAsync();
		return department;
	}

	public async Task<bool> DeleteDepartmentAsync(int id)
	{
		var department = await unitOfWork.Department.GetByIdAsync(id);
		if (department is null)
		{
			return false;
		}

		unitOfWork.Department.Delete(department);
		await unitOfWork.SaveChangesAsync();
		return true;
	}

	public async Task<bool> IsDepartmentCodeExistsAsync(string code, int? excludeId = null)
	{
		var existing = await unitOfWork.Department.GetByCodeAsync(code);
		if (existing is null)
		{
			return false;
		}

		return excludeId is null || existing.Id != excludeId;
	}

	// 銀行初始金額相關
	public async Task<List<BankInitialBalance>> GetAllBankInitialBalancesAsync()
	{
		return await unitOfWork.BankInitialBalance.GetAllAsync();
	}

	public async Task<BankInitialBalance?> GetBankInitialBalanceAsync(string bankType)
	{
		return await unitOfWork.BankInitialBalance.GetByBankTypeAsync(bankType);
	}

	public async Task SaveBankInitialBalanceAsync(BankInitialBalance balance)
	{
		var existing = await unitOfWork.BankInitialBalance.GetByBankTypeAsync(balance.BankType);
		if (existing is null)
		{
			unitOfWork.BankInitialBalance.Add(balance);
		}
		else
		{
			existing.InitialBalance = balance.InitialBalance;
			existing.EffectiveYear = balance.EffectiveYear;
			unitOfWork.BankInitialBalance.Update(existing);
		}

		await unitOfWork.SaveChangesAsync();
	}
}
