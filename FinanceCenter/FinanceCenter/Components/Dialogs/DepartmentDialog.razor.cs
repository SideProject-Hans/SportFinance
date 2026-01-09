using FinanceCenter.Data.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinanceCenter.Components.Dialogs;

/// <summary>
/// 部門新增/編輯對話框元件
/// </summary>
public partial class DepartmentDialog
{
	[CascadingParameter]
	private IMudDialogInstance MudDialog { get; set; } = null!;

	[Parameter]
	public Department Department { get; set; } = new();

	[Parameter]
	public bool IsEdit { get; set; }

	private MudForm _form = null!;
	private bool _isValid;

	private void Cancel() => MudDialog.Cancel();

	private void Submit() => MudDialog.Close(DialogResult.Ok(Department));
}
