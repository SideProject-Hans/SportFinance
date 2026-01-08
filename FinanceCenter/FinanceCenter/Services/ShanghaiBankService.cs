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
