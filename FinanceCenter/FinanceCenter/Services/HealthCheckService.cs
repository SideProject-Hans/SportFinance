using FinanceCenter.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Services;

/// <summary>
/// 資料庫健康檢查服務
/// </summary>
public class HealthCheckService(
	FinanceCenterDbContext dbContext,
	IWebHostEnvironment environment) : IHealthCheckService
{
	private const string SqlSchemaFolder = "Doc/MySqlTableScheme";

	/// <summary>
	/// 取得所有資料表的健康狀態
	/// </summary>
	public async Task<List<TableHealthStatus>> GetTableHealthStatusAsync()
	{
		var results = new List<TableHealthStatus>();
		var sqlFiles = GetSqlFiles();

		foreach (var sqlFile in sqlFiles)
		{
			var tableName = Path.GetFileNameWithoutExtension(sqlFile);
			var exists = await CheckTableExistsAsync(tableName);
			results.Add(new TableHealthStatus(tableName, sqlFile, exists));
		}

		return results.OrderBy(r => r.TableName).ToList();
	}

	/// <summary>
	/// 建立單一缺失的資料表
	/// </summary>
	public async Task<(bool Success, string Message)> CreateTableAsync(string tableName)
	{
		try
		{
			var sqlFilePath = GetSqlFilePath(tableName);
			if (!File.Exists(sqlFilePath))
			{
				return (false, $"找不到 SQL 檔案: {tableName}.sql");
			}

			var sql = await File.ReadAllTextAsync(sqlFilePath);
			await dbContext.Database.ExecuteSqlRawAsync(sql);
			return (true, $"資料表 {tableName} 建立成功");
		}
		catch (Exception ex)
		{
			return (false, $"建立失敗: {ex.Message}");
		}
	}

	/// <summary>
	/// 建立所有缺失的資料表
	/// </summary>
	public async Task<(int Created, int Failed, List<string> Errors)> CreateAllMissingTablesAsync()
	{
		var statuses = await GetTableHealthStatusAsync();
		var missingTables = statuses.Where(s => !s.Exists).ToList();

		var created = 0;
		var failed = 0;
		var errors = new List<string>();

		foreach (var table in missingTables)
		{
			var (success, message) = await CreateTableAsync(table.TableName);
			if (success)
			{
				created++;
			}
			else
			{
				failed++;
				errors.Add($"{table.TableName}: {message}");
			}
		}

		return (created, failed, errors);
	}

	/// <summary>
	/// 取得所有 SQL 檔案路徑
	/// </summary>
	private string[] GetSqlFiles()
	{
		var schemaPath = Path.Combine(environment.ContentRootPath, SqlSchemaFolder);
		if (!Directory.Exists(schemaPath))
		{
			return [];
		}

		return Directory.GetFiles(schemaPath, "*.sql");
	}

	/// <summary>
	/// 取得特定資料表的 SQL 檔案路徑
	/// </summary>
	private string GetSqlFilePath(string tableName)
	{
		return Path.Combine(environment.ContentRootPath, SqlSchemaFolder, $"{tableName}.sql");
	}

	/// <summary>
	/// 檢查資料表是否存在
	/// </summary>
	private async Task<bool> CheckTableExistsAsync(string tableName)
	{
		var connection = dbContext.Database.GetDbConnection();
		var wasOpen = connection.State == System.Data.ConnectionState.Open;

		if (!wasOpen)
			await connection.OpenAsync();

		try
		{
			using var command = connection.CreateCommand();
			command.CommandText = @"
				SELECT COUNT(*) 
				FROM information_schema.TABLES 
				WHERE TABLE_SCHEMA = DATABASE() 
				  AND TABLE_NAME = @tableName";

			var parameter = command.CreateParameter();
			parameter.ParameterName = "@tableName";
			parameter.Value = tableName;
			command.Parameters.Add(parameter);

			var result = await command.ExecuteScalarAsync();
			return Convert.ToInt32(result) > 0;
		}
		finally
		{
			if (!wasOpen)
				await connection.CloseAsync();
		}
	}
}
