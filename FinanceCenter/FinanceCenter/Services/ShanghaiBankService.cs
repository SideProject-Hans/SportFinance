using ClosedXML.Excel;
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
		// 取得已被 DbContext 追蹤的實體，避免追蹤衝突
		var existing = await unitOfWork.ShanghaiBank.GetByIdAsync(account.Id);
		if (existing is null)
		{
			throw new InvalidOperationException($"ShanghaiBankAccount with Id {account.Id} not found.");
		}

		// 更新已追蹤實體的屬性
		existing.RemittanceDate = account.RemittanceDate;
		existing.Department = account.Department;
		existing.Applicant = account.Applicant;
		existing.Reason = account.Reason;
		existing.Income = account.Income;
		existing.Expense = account.Expense;
		existing.Fee = account.Fee;

		await unitOfWork.SaveChangesAsync();
		return existing;
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

	public async Task<List<ShanghaiBankAccount>> GetByYearAsync(int year)
	{
		var allAccounts = await unitOfWork.ShanghaiBank.GetAllAsync();
		return allAccounts
			.Where(a => a.RemittanceDate.HasValue && a.RemittanceDate.Value.Year == year)
			.OrderBy(a => a.RemittanceDate)
			.ToList();
	}

	public async Task<List<int>> GetAvailableYearsAsync()
	{
		var allAccounts = await unitOfWork.ShanghaiBank.GetAllAsync();
		return allAccounts
			.Where(a => a.RemittanceDate.HasValue)
			.Select(a => a.RemittanceDate!.Value.Year)
			.Distinct()
			.OrderByDescending(y => y)
			.ToList();
	}

	public async Task<decimal> GetOpeningBalanceAsync(int year)
	{
		var allAccounts = await unitOfWork.ShanghaiBank.GetAllAsync();
		var config = await unitOfWork.BankInitialBalance.GetByBankTypeAsync("ShanghaiBank");

		// 如果沒有設定初始金額，使用舊邏輯
		if (config is null)
		{
			return allAccounts
				.Where(a => a.RemittanceDate.HasValue && a.RemittanceDate.Value.Year < year)
				.Sum(a => a.NetAmount);
		}

		// 計算生效年份到指定年份之前的淨金額總和
		var historicalSum = allAccounts
			.Where(a => a.RemittanceDate.HasValue
				&& a.RemittanceDate.Value.Year >= config.EffectiveYear
				&& a.RemittanceDate.Value.Year < year)
			.Sum(a => a.NetAmount);

		return config.InitialBalance + historicalSum;
	}

	public async Task<int> ImportFromExcelAsync(string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Excel 檔案不存在: {filePath}");
		}

		// 先清空現有資料
		await unitOfWork.ShanghaiBank.ClearAllAsync();
		await unitOfWork.SaveChangesAsync();

		var accounts = new List<ShanghaiBankAccount>();

		using var workbook = new XLWorkbook(filePath);

		// 遍歷所有頁簽 (2019~2025)
		foreach (var worksheet in workbook.Worksheets)
		{
			var rows = worksheet.RowsUsed().Skip(1); // 跳過標題列
			ShanghaiBankAccount? previousAccount = null;

			foreach (var row in rows)
			{
				// 讀取 Excel 欄位
				var year = row.Cell(1).GetValue<int>();      // 年度 (民國年)
				var month = row.Cell(2).GetValue<int>();     // 月
				var day = row.Cell(3).GetValue<int>();       // 日
				var reason = row.Cell(5).GetString().Trim(); // 內容摘要
				var income = row.Cell(6).GetValue<decimal?>() ?? 0;   // 存入金額
				var expense = row.Cell(7).GetValue<decimal?>() ?? 0;  // 支出金額
				var applicant = row.Cell(9).GetString().Trim(); // 申請人

				// 處理手續費特殊邏輯：將金額加到前一筆紀錄的 Fee 欄位
				if (reason == "手續費")
				{
					if (previousAccount != null)
					{
						previousAccount.Fee = expense;
					}
					continue; // 不建立新紀錄
				}

				// 民國年轉西元年
				var westernYear = year + 1911;
				var remittanceDate = new DateOnly(westernYear, month, day);

				var account = new ShanghaiBankAccount
				{
					CreateDay = DateTime.Now,
					RemittanceDate = remittanceDate,
					Department = "",
					Applicant = applicant,
					Reason = reason,
					Income = income,
					Expense = expense,
					Fee = 0
				};

				accounts.Add(account);
				previousAccount = account;
			}
		}

		unitOfWork.ShanghaiBank.AddRange(accounts);
		await unitOfWork.SaveChangesAsync();

		return accounts.Count;
	}
}
