using FinanceCenter.Data.Entities;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Dialogs;

/// <summary>
/// 部門新增/編輯對話框元件（原生 Glassmorphism 風格）
/// </summary>
public partial class DepartmentDialog
{
	[Parameter]
	public Department Department { get; set; } = new();

	[Parameter]
	public bool IsEdit { get; set; }

	[Parameter]
	public bool IsVisible { get; set; }

	[Parameter]
	public EventCallback OnCancel { get; set; }

	[Parameter]
	public EventCallback<Department> OnSubmit { get; set; }

	private bool IsFormValid => !string.IsNullOrWhiteSpace(Department.Code) &&
								!string.IsNullOrWhiteSpace(Department.Name);

	private async Task CancelAsync() => await OnCancel.InvokeAsync();

	private async Task SubmitAsync()
	{
		if (IsFormValid)
		{
			await OnSubmit.InvokeAsync(Department);
		}
	}

	private async Task HandleOverlayClickAsync()
	{
		await CancelAsync();
	}
}
