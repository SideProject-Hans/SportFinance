using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 預算項目明細實體
/// </summary>
[Table("BudgetItem")]
public class BudgetItem
{
	/// <summary>
	/// 唯一識別碼
	/// </summary>
	[Key]
	[Column("Id")]
	public int Id { get; set; }

	/// <summary>
	/// 所屬部門預算 ID
	/// </summary>
	[Column("DepartmentBudgetId")]
	public int DepartmentBudgetId { get; set; }

	/// <summary>
	/// 項目名稱（自由填寫）
	/// </summary>
	[Required]
	[MaxLength(100)]
	[Column("ItemName")]
	public string ItemName { get; set; } = string.Empty;

	/// <summary>
	/// 預算金額
	/// </summary>
	[Column("Amount", TypeName = "decimal(18, 2)")]
	public decimal Amount { get; set; } = 0.00m;

	/// <summary>
	/// 說明（選填）
	/// </summary>
	[MaxLength(500)]
	[Column("Description")]
	public string? Description { get; set; }

	/// <summary>
	/// 排序順序
	/// </summary>
	[Column("SortOrder")]
	public int SortOrder { get; set; } = 0;

	/// <summary>
	/// 所屬部門預算（導航屬性，用於 EF Core Include）
	/// </summary>
	public DepartmentBudget? DepartmentBudget { get; set; }
}
