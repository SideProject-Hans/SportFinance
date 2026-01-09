using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 銀行初始金額資料實體
/// </summary>
[Table("BankInitialBalance")]
public class BankInitialBalance
{
	/// <summary>
	/// 唯一識別碼
	/// </summary>
	[Key]
	[Column("Id")]
	public int Id { get; set; }

	/// <summary>
	/// 銀行類型（ShanghaiBank, TaiwanCooperativeBank）
	/// </summary>
	[Required]
	[MaxLength(50)]
	[Column("BankType")]
	public string BankType { get; set; } = string.Empty;

	/// <summary>
	/// 初始金額
	/// </summary>
	[Column("InitialBalance", TypeName = "decimal(18, 2)")]
	public decimal InitialBalance { get; set; } = 0.00m;

	/// <summary>
	/// 生效年份（從哪年開始累計）
	/// </summary>
	[Column("EffectiveYear")]
	public int EffectiveYear { get; set; }
}
