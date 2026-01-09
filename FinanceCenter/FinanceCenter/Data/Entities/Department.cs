using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceCenter.Data.Entities;

/// <summary>
/// 部門資料實體
/// </summary>
[Table("Department")]
public class Department
{
	/// <summary>
	/// 唯一識別碼
	/// </summary>
	[Key]
	[Column("Id")]
	public int Id { get; set; }

	/// <summary>
	/// 部門代號（穩定識別碼，存入交易表）
	/// </summary>
	[Required]
	[MaxLength(20)]
	[Column("Code")]
	public string Code { get; set; } = string.Empty;

	/// <summary>
	/// 部門名稱（顯示用，可修改）
	/// </summary>
	[Required]
	[MaxLength(100)]
	[Column("Name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// 是否啟用
	/// </summary>
	[Column("IsActive")]
	public bool IsActive { get; set; } = true;

	/// <summary>
	/// 排序順序
	/// </summary>
	[Column("SortOrder")]
	public int SortOrder { get; set; } = 0;
}
