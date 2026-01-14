namespace FinanceCenter.Services;

/// <summary>
/// 資料庫健康檢查服務介面
/// </summary>
public interface IHealthCheckService
{
	/// <summary>
	/// 取得所有資料表的健康狀態
	/// </summary>
	Task<List<TableHealthStatus>> GetTableHealthStatusAsync();

	/// <summary>
	/// 建立單一缺失的資料表
	/// </summary>
	Task<(bool Success, string Message)> CreateTableAsync(string tableName);

	/// <summary>
	/// 建立所有缺失的資料表
	/// </summary>
	Task<(int Created, int Failed, List<string> Errors)> CreateAllMissingTablesAsync();
}

/// <summary>
/// 資料表健康狀態
/// </summary>
public record TableHealthStatus(
	string TableName,
	string SqlFilePath,
	bool Exists
);
