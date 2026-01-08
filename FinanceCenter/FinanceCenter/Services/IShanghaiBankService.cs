using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 上海銀行服務介面
/// </summary>
public interface IShanghaiBankService
{
	Task<List<ShanghaiBankAccount>> GetAllAsync();
	Task<ShanghaiBankAccount?> GetByIdAsync(int id);
	Task<List<ShanghaiBankAccount>> GetByDepartmentAsync(string department);
	Task<List<ShanghaiBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<ShanghaiBankAccount> AddAsync(ShanghaiBankAccount account);
	Task<ShanghaiBankAccount> UpdateAsync(ShanghaiBankAccount account);
	Task<bool> DeleteAsync(int id);
	
	/// <summary>
	/// 從 Excel 檔案匯入資料（會先清空現有資料）
	/// </summary>
	/// <param name="filePath">Excel 檔案路徑</param>
	/// <returns>匯入的資料筆數</returns>
	Task<int> ImportFromExcelAsync(string filePath);

	/// <summary>
	/// 取得指定年份的帳戶明細（根據匯款日期）
	/// </summary>
	/// <param name="year">年份</param>
	/// <returns>該年度的帳戶明細清單</returns>
	Task<List<ShanghaiBankAccount>> GetByYearAsync(int year);

	/// <summary>
	/// 取得所有有資料的年份清單
	/// </summary>
	/// <returns>年份清單</returns>
	Task<List<int>> GetAvailableYearsAsync();

	/// <summary>
	/// 取得指定年份的年初餘額（上一年度最後淨金額累計）
	/// </summary>
	/// <param name="year">年份</param>
	/// <returns>年初餘額</returns>
	Task<decimal> GetOpeningBalanceAsync(int year);
}
