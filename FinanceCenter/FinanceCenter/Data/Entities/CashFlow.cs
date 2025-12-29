using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 現金流管理資料實體
/// </summary>
[Table("CashFlow")]
public class CashFlow
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
