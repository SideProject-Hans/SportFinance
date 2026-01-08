using FinanceCenter.Components.Dialogs;
using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Pages;

/// <summary>
/// 上海銀行帳戶明細頁面
/// </summary>
public partial class ShanghaiBank
{
	[Inject]
	private IShanghaiBankService ShanghaiBankService { get; set; } = null!;

	[Inject]
	private IDialogService DialogService { get; set; } = null!;

	[Inject]
	private ISnackbar Snackbar { get; set; } = null!;

	[Inject]
	private IWebHostEnvironment WebHostEnvironment { get; set; } = null!;

	private List<ShanghaiBankAccount> Accounts { get; set; } = new();

	protected override async Task OnInitializedAsync()
	{
		await LoadDataAsync();
	}

	private async Task LoadDataAsync()
	{
		Accounts = await ShanghaiBankService.GetAllAsync();
	}

	private async Task RefreshDataAsync()
	{
		await LoadDataAsync();
	}

	private async Task OpenAddDialogAsync()
	{
		var dialog = await DialogService.ShowAsync<AddShanghaiBankAccountDialog>("新增上海銀行帳戶明細");
		var result = await dialog.Result;

		if (result is { Canceled: false, Data: ShanghaiBankAccount account })
		{
			await ShanghaiBankService.AddAsync(account);
			await LoadDataAsync();
		}
	}

	private async Task OpenInitializeDialogAsync()
	{
		var parameters = new DialogParameters<ConfirmDialog>
		{
			{ x => x.ContentText, "此操作會清空所有現有資料，並從 Excel 檔案重新匯入。確定要繼續嗎？" },
			{ x => x.ButtonText, "確認初始化" },
			{ x => x.Color, Color.Error }
		};

		var options = new DialogOptions { CloseOnEscapeKey = true };
		var dialog = await DialogService.ShowAsync<ConfirmDialog>("初始化資料確認", parameters, options);
		var result = await dialog.Result;

		if (result is { Canceled: false })
		{
			await InitializeDataAsync();
		}
	}

	private async Task InitializeDataAsync()
	{
		try
		{
			// Excel 檔案路徑
			var contentRoot = WebHostEnvironment.ContentRootPath;
			var excelFilePath = Path.Combine(contentRoot, "Doc", "Temp", "上海-收支表.xlsx");

			var importedCount = await ShanghaiBankService.ImportFromExcelAsync(excelFilePath);
			
			Snackbar.Add($"成功匯入 {importedCount} 筆資料", Severity.Success);
			await LoadDataAsync();
		}
		catch (FileNotFoundException ex)
		{
			Snackbar.Add($"找不到 Excel 檔案: {ex.Message}", Severity.Error);
		}
		catch (Exception ex)
		{
			Snackbar.Add($"匯入失敗: {ex.Message}", Severity.Error);
		}
	}
}
