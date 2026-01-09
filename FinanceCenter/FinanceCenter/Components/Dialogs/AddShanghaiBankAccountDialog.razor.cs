using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Dialogs;

/// <summary>
/// 新增上海銀行帳戶明細對話框元件
/// </summary>
public partial class AddShanghaiBankAccountDialog
{
	[CascadingParameter]
	private IMudDialogInstance MudDialog { get; set; } = null!;

	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	private ShanghaiBankAccount Account { get; set; } = new();
	private List<Department> Departments { get; set; } = new();

	private MudForm _form = null!;
	private bool _isValid;

	protected override async Task OnInitializedAsync()
	{
		Departments = await SettingsService.GetActiveDepartmentsAsync();
	}

	private void Cancel() => MudDialog.Cancel();

	private void Submit() => MudDialog.Close(DialogResult.Ok(Account));
}
