using FinanceCenter.Data.Entities;

namespace FinanceCenter.Services;

/// <summary>
/// 合作金庫服務介面
/// </summary>
public interface ITaiwanCooperativeBankService
{
	Task<List<TaiwanCooperativeBankAccount>> GetAllAsync();
	Task<TaiwanCooperativeBankAccount?> GetByIdAsync(int id);
	Task<List<TaiwanCooperativeBankAccount>> GetByDepartmentAsync(string department);
	Task<List<TaiwanCooperativeBankAccount>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
	Task<TaiwanCooperativeBankAccount> AddAsync(TaiwanCooperativeBankAccount account);
	Task<TaiwanCooperativeBankAccount> UpdateAsync(TaiwanCooperativeBankAccount account);
	Task<bool> DeleteAsync(int id);
	Task<int> ImportFromExcelAsync(string filePath);

	/// <summary>
	/// 取得指定年份的帳戶明細（根據匯款日期）
	/// </summary>
	/// <param name="year">年份</param>
	/// <returns>該年度的帳戶明細清單</returns>
	Task<List<TaiwanCooperativeBankAccount>> GetByYearAsync(int year);

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
