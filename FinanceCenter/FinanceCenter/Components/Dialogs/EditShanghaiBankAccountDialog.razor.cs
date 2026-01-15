using FinanceCenter.Data.Entities;
using FinanceCenter.Services;
using Microsoft.AspNetCore.Components;

namespace FinanceCenter.Components.Dialogs;

/// <summary>
/// 編輯上海銀行帳戶明細 Dialog
/// </summary>
public partial class EditShanghaiBankAccountDialog
{
	[Inject]
	private ISettingsService SettingsService { get; set; } = null!;

	[Parameter]
	public bool IsOpen { get; set; }

	[Parameter]
	public ShanghaiBankAccount Account { get; set; } = null!;

	[Parameter]
	public EventCallback OnCancel { get; set; }

	[Parameter]
	public EventCallback<ShanghaiBankAccount> OnSubmit { get; set; }

	private ShanghaiBankAccount EditAccount { get; set; } = new();
	private List<Department> Departments { get; set; } = new();

	/// <summary>
	/// 日期綁定用
	/// </summary>
	private DateTime RemittanceDateValue
	{
		get => EditAccount.RemittanceDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today;
		set => EditAccount.RemittanceDate = DateOnly.FromDateTime(value);
	}

	/// <summary>
	/// 表單是否有效
	/// </summary>
	private bool IsFormValid =>
		!string.IsNullOrWhiteSpace(EditAccount.Department) &&
		!string.IsNullOrWhiteSpace(EditAccount.Applicant) &&
		!string.IsNullOrWhiteSpace(EditAccount.Reason);

	protected override async Task OnInitializedAsync()
	{
		Departments = await SettingsService.GetActiveDepartmentsAsync();
	}

	protected override void OnParametersSet()
	{
		if (IsOpen && Account != null)
		{
			CopyAccountData();
		}
	}

	/// <summary>
	/// 複製原始資料到編輯物件
	/// </summary>
	private void CopyAccountData()
	{
		EditAccount = new ShanghaiBankAccount
		{
			Id = Account.Id,
			CreateDay = Account.CreateDay,
			RemittanceDate = Account.RemittanceDate,
			Department = Account.Department,
			Applicant = Account.Applicant,
			Reason = Account.Reason,
			Income = Account.Income,
			Expense = Account.Expense,
			Fee = Account.Fee
		};
	}

	private async Task Submit()
	{
		if (IsFormValid)
		{
			await OnSubmit.InvokeAsync(EditAccount);
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
