using ClosedXML.Excel;
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

	public async Task<int> ImportFromExcelAsync(string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException($"Excel 檔案不存在: {filePath}");
		}

		// 先清空現有資料
		await unitOfWork.TaiwanCooperativeBank.ClearAllAsync();
		await unitOfWork.SaveChangesAsync();

		var accounts = new List<TaiwanCooperativeBankAccount>();

		using var workbook = new XLWorkbook(filePath);

		// 遍歷所有頁簽 (2019~2025)
		foreach (var worksheet in workbook.Worksheets)
		{
			var rows = worksheet.RowsUsed().Skip(1); // 跳過標題列
			TaiwanCooperativeBankAccount? previousAccount = null;

			foreach (var row in rows)
			{
				// 檢查年度欄位是否為空或非數字，如果是則跳過該列
				var yearCell = row.Cell(1);
				if (yearCell.IsEmpty() || !yearCell.TryGetValue<int>(out var year))
				{
					continue;
				}

				// 檢查月、日欄位是否為空或非數字
				var monthCell = row.Cell(2);
				var dayCell = row.Cell(3);
				if (monthCell.IsEmpty() || !monthCell.TryGetValue<int>(out var month) ||
				    dayCell.IsEmpty() || !dayCell.TryGetValue<int>(out var day))
				{
					continue;
				}

				// 讀取其他 Excel 欄位
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

				var account = new TaiwanCooperativeBankAccount
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

		unitOfWork.TaiwanCooperativeBank.AddRange(accounts);
		await unitOfWork.SaveChangesAsync();

		return accounts.Count;
	}
}
