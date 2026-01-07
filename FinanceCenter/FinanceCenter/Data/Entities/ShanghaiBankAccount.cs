using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 上海銀行帳戶明細資料實體
/// </summary>
[Table("ShanghaiBankAccount")]
public class ShanghaiBankAccount
{
	/// <summary>
	/// 唯一識別碼
	/// </summary>
	[Key]
	[Column("Id")]
	public int Id { get; set; }

	/// <summary>
	/// 建立日期
	/// </summary>
	[Column("CreateDay")]
	public DateTime CreateDay { get; set; } = DateTime.Now;

	/// <summary>
	/// 匯款日期
	/// </summary>
	[Column("RemittanceDate")]
	public DateOnly? RemittanceDate { get; set; }

	/// <summary>
	/// 申請部門
	/// </summary>
	[Required]
	[MaxLength(100)]
	[Column("Department")]
	public string Department { get; set; } = string.Empty;

	/// <summary>
	/// 申請人
	/// </summary>
	[Required]
	[MaxLength(50)]
	[Column("Applicant")]
	public string Applicant { get; set; } = string.Empty;

	/// <summary>
	/// 申請原因
	/// </summary>
	[Required]
	[MaxLength(500)]
	[Column("Reason")]
	public string Reason { get; set; } = string.Empty;

	/// <summary>
	/// 支出金額
	/// </summary>
	[Column("Expense", TypeName = "decimal(18, 2)")]
	public decimal Expense { get; set; } = 0.00m;

	/// <summary>
	/// 收入金額
	/// </summary>
	[Column("Income", TypeName = "decimal(18, 2)")]
	public decimal Income { get; set; } = 0.00m;

	/// <summary>
	/// 手續費
	/// </summary>
	[Column("Fee", TypeName = "decimal(18, 2)")]
	public decimal Fee { get; set; } = 0.00m;

	/// <summary>
	/// 計算淨金額 (收入 - 支出 - 手續費)
	/// </summary>
	[NotMapped]
	public decimal NetAmount => Income - Expense - Fee;
}
