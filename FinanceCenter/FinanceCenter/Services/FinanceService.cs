using FinanceCenter.Data.Entities;
using FinanceCenter.Repositories;

namespace FinanceCenter.Services;

/// <summary>
/// 財務服務層
/// </summary>
public class FinanceService(IUnitOfWork unitOfWork) : IFinanceService
{
	/// <summary>
	/// 取得所有現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetAllCashFlowsAsync()
	{
		return await unitOfWork.Finance.GetAllCashFlowsAsync();
	}

	/// <summary>
	/// 根據 Id 取得現金流紀錄
	/// </summary>
	public async Task<CashFlow?> GetCashFlowByIdAsync(int id)
	{
		return await unitOfWork.Finance.GetCashFlowByIdAsync(id);
	}

	/// <summary>
	/// 根據部門取得現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetCashFlowsByDepartmentAsync(string department)
	{
		return await unitOfWork.Finance.GetCashFlowsByDepartmentAsync(department);
	}

	/// <summary>
	/// 根據日期範圍取得現金流紀錄
	/// </summary>
	public async Task<List<CashFlow>> GetCashFlowsByDateRangeAsync(DateTime startDate, DateTime endDate)
	{
		return await unitOfWork.Finance.GetCashFlowsByDateRangeAsync(startDate, endDate);
	}

	/// <summary>
	/// 新增現金流紀錄
	/// </summary>
	public async Task<CashFlow> AddCashFlowAsync(CashFlow cashFlow)
	{
		unitOfWork.Finance.Add(cashFlow);
		await unitOfWork.SaveChangesAsync();
		return cashFlow;
	}

	/// <summary>
	/// 更新現金流紀錄
	/// </summary>
	public async Task<CashFlow> UpdateCashFlowAsync(CashFlow cashFlow)
	{
		// 取得已被 DbContext 追蹤的實體，避免追蹤衝突
		var existing = await unitOfWork.Finance.GetCashFlowByIdAsync(cashFlow.Id);
		if (existing is null)
		{
			throw new InvalidOperationException($"CashFlow with Id {cashFlow.Id} not found.");
		}

		// 更新已追蹤實體的屬性
		existing.RemittanceDate = cashFlow.RemittanceDate;
		existing.Department = cashFlow.Department;
		existing.Applicant = cashFlow.Applicant;
		existing.Reason = cashFlow.Reason;
		existing.Income = cashFlow.Income;
		existing.Expense = cashFlow.Expense;
		existing.Fee = cashFlow.Fee;

		await unitOfWork.SaveChangesAsync();
		return existing;
	}

	/// <summary>
	/// 刪除現金流紀錄
	/// </summary>
	public async Task<bool> DeleteCashFlowAsync(int id)
	{
		var cashFlow = await unitOfWork.Finance.GetCashFlowByIdAsync(id);
		if (cashFlow is null)
		{
			return false;
		}

		unitOfWork.Finance.Delete(cashFlow);
		await unitOfWork.SaveChangesAsync();
		return true;
	}
}
