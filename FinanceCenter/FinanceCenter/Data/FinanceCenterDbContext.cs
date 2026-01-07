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

    /// <summary>
    /// 上海銀行帳戶明細資料表
    /// </summary>
    public DbSet<ShanghaiBankAccount> ShanghaiBankAccounts { get; set; } = null!;

    /// <summary>
    /// 合作金庫帳戶明細資料表
    /// </summary>
    public DbSet<TaiwanCooperativeBankAccount> TaiwanCooperativeBankAccounts { get; set; } = null!;

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

        // ShanghaiBankAccount 實體配置
        modelBuilder.Entity<ShanghaiBankAccount>(entity =>
        {
            entity.ToTable("ShanghaiBankAccount");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreateDay).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Applicant).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Expense).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.Property(e => e.Income).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.Property(e => e.Fee).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.HasIndex(e => e.CreateDay).HasDatabaseName("idx_scsb_createday");
            entity.HasIndex(e => e.Department).HasDatabaseName("idx_scsb_department");
            entity.HasIndex(e => e.Applicant).HasDatabaseName("idx_scsb_applicant");
        });

        // TaiwanCooperativeBankAccount 實體配置
        modelBuilder.Entity<TaiwanCooperativeBankAccount>(entity =>
        {
            entity.ToTable("TaiwanCooperativeBankAccount");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CreateDay).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Department).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Applicant).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Expense).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.Property(e => e.Income).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.Property(e => e.Fee).HasPrecision(18, 2).HasDefaultValue(0.00m);
            entity.HasIndex(e => e.CreateDay).HasDatabaseName("idx_tcb_createday");
            entity.HasIndex(e => e.Department).HasDatabaseName("idx_tcb_department");
            entity.HasIndex(e => e.Applicant).HasDatabaseName("idx_tcb_applicant");
        });
    }
}
