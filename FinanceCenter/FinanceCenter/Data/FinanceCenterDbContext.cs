using FinanceCenter.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceCenter.Data;

/// <summary>
/// 財務中心資料庫上下文
/// </summary>
public class FinanceCenterDbContext : DbContext
{
    public FinanceCenterDbContext(DbContextOptions<FinanceCenterDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 現金流資料表
    /// </summary>
    public DbSet<CashFlow> CashFlows { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // CashFlow 實體配置
        modelBuilder.Entity<CashFlow>(entity =>
        {
            entity.ToTable("CashFlow");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.CreateDay)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.Department)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Applicant)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Reason)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.Expense)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            entity.Property(e => e.Income)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            entity.Property(e => e.Fee)
                .HasPrecision(18, 2)
                .HasDefaultValue(0.00m);

            // 索引配置
            entity.HasIndex(e => e.CreateDay)
                .HasDatabaseName("idx_createday");

            entity.HasIndex(e => e.Department)
                .HasDatabaseName("idx_department");

            entity.HasIndex(e => e.Applicant)
                .HasDatabaseName("idx_applicant");
        });
    }
}
