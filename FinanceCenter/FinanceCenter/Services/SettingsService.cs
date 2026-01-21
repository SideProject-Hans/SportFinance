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
		// 取得已被 DbContext 追蹤的實體，避免追蹤衝突
		var existing = await unitOfWork.Department.GetByIdAsync(department.Id);
		if (existing is null)
		{
			throw new InvalidOperationException($"Department with Id {department.Id} not found.");
		}

		// 更新已追蹤實體的屬性
		existing.Code = department.Code;
		existing.Name = department.Name;
		existing.IsActive = department.IsActive;
		existing.SortOrder = department.SortOrder;

		await unitOfWork.SaveChangesAsync();
		return existing;
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
		return await unitOfWork.Department.ExistsByCodeAsync(code, excludeId);
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
