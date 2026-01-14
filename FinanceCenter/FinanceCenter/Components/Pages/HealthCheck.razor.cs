using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 系統健康檢查頁面
/// </summary>
public partial class HealthCheck
{
	[Inject]
	private IHealthCheckService HealthCheckService { get; set; } = null!;

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	private List<TableHealthStatus> TableStatuses { get; set; } = [];
	private bool IsLoading { get; set; }

	private int TotalCount => TableStatuses.Count;
	private int ExistsCount => TableStatuses.Count(s => s.Exists);
	private int MissingCount => TableStatuses.Count(s => !s.Exists);

	protected override async Task OnInitializedAsync()
	{
		await RefreshAsync();
	}

	private async Task RefreshAsync()
	{
		IsLoading = true;
		try
		{
			TableStatuses = await HealthCheckService.GetTableHealthStatusAsync();
		}
		catch (Exception ex)
		{
			Snackbar.Add($"無法取得資料表狀態: {ex.Message}", Severity.Error);
			TableStatuses = [];
		}
		finally
		{
			IsLoading = false;
		}
	}

	private async Task CreateTableAsync(string tableName)
	{
		IsLoading = true;
		try
		{
			var (success, message) = await HealthCheckService.CreateTableAsync(tableName);
			Snackbar.Add(message, success ? Severity.Success : Severity.Error);

			if (success)
			{
				TableStatuses = await HealthCheckService.GetTableHealthStatusAsync();
			}
		}
		catch (Exception ex)
		{
			Snackbar.Add($"操作失敗: {ex.Message}", Severity.Error);
		}
		finally
		{
			IsLoading = false;
		}
	}

	private async Task CreateAllMissingAsync()
	{
		IsLoading = true;
		try
		{
			var (created, failed, errors) = await HealthCheckService.CreateAllMissingTablesAsync();

			if (created > 0)
			{
				Snackbar.Add($"成功建立 {created} 個資料表", Severity.Success);
			}

			foreach (var error in errors)
			{
				Snackbar.Add(error, Severity.Error);
			}

			TableStatuses = await HealthCheckService.GetTableHealthStatusAsync();
		}
		catch (Exception ex)
		{
			Snackbar.Add($"操作失敗: {ex.Message}", Severity.Error);
		}
		finally
		{
			IsLoading = false;
		}
	}
}
