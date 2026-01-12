using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 部門年度預算實體
/// </summary>
[Table("DepartmentBudget")]
public class DepartmentBudget
{
	/// <summary>
	/// 唯一識別碼
	/// </summary>
	[Key]
	[Column("Id")]
	public int Id { get; set; }

	/// <summary>
	/// 年度
	/// </summary>
	[Column("Year")]
	public int Year { get; set; }

	/// <summary>
	/// 部門代號（關聯 Department.Code）
	/// </summary>
	[Required]
	[MaxLength(20)]
	[Column("DepartmentCode")]
	public string DepartmentCode { get; set; } = string.Empty;

	/// <summary>
	/// 建立時間
	/// </summary>
	[Column("CreatedAt")]
	public DateTime CreatedAt { get; set; } = DateTime.Now;

	/// <summary>
	/// 預算項目明細
	/// </summary>
	public ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();

	/// <summary>
	/// 總預算金額（計算屬性，不存入資料庫）
	/// </summary>
	[NotMapped]
	public decimal TotalAmount => BudgetItems.Sum(x => x.Amount);
}
