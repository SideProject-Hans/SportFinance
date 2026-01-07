using FinanceCenter.Data.Entities;
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

	private ShanghaiBankAccount Account { get; set; } = new();

	private MudForm _form = null!;
	private bool _isValid;

	private void Cancel() => MudDialog.Cancel();

	private void Submit() => MudDialog.Close(DialogResult.Ok(Account));
}
