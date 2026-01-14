using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Dialogs;

/// <summary>
/// 新增上海銀行帳戶明細 Dialog
/// </summary>
public partial class AddShanghaiBankAccountDialog
{
	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Parameter]
	public bool IsOpen { get; set; }

	[Parameter]
	public EventCallback OnCancel { get; set; }

	[Parameter]
	public EventCallback<ShanghaiBankAccount> OnSubmit { get; set; }

	private ShanghaiBankAccount Account { get; set; } = new();
	private List<Department> Departments { get; set; } = new();

	/// <summary>
	/// 日期綁定用（預設今天）
	/// </summary>
	private DateTime RemittanceDateValue
	{
		get => Account.RemittanceDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
		set => Account.RemittanceDate = DateOnly.FromDateTime(value);
	}

	/// <summary>
	/// 表單是否有效
	/// </summary>
	private bool IsFormValid =>
		!string.IsNullOrWhiteSpace(Account.Department) &&
		!string.IsNullOrWhiteSpace(Account.Applicant) &&
		!string.IsNullOrWhiteSpace(Account.Reason);

	protected override async Task OnInitializedAsync()
	{
		Departments = await SettingsService.GetActiveDepartmentsAsync();
	}

	protected override void OnParametersSet()
	{
		if (IsOpen)
		{
			ResetForm();
		}
	}

	private void ResetForm()
	{
		Account = new ShanghaiBankAccount
		{
			RemittanceDate = DateOnly.FromDateTime(DateTime.Today)
		};
	}

	private async Task Submit()
	{
		if (IsFormValid)
		{
			await OnSubmit.InvokeAsync(Account);
		}
	}

	private async Task Cancel()
	{
		await OnCancel.InvokeAsync();
	}

	private void OnOverlayClick()
	{
		_ = Cancel();
	}
}
