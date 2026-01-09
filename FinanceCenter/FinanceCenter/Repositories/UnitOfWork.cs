using FinanceCenter.Data;

namespace FinanceCenter.Repositories;

/// <summary>
/// Unit of Work 實作 - 管理交易範圍
/// </summary>
public class UnitOfWork(FinanceCenterDbContext context) : IUnitOfWork
{
	private IFinanceRepository? _finance;
	private IShanghaiBankRepository? _shanghaiBank;
	private ITaiwanCooperativeBankRepository? _taiwanCooperativeBank;
	private IDepartmentRepository? _department;
	private IBankInitialBalanceRepository? _bankInitialBalance;

	public IFinanceRepository Finance => _finance ??= new FinanceRepository(context);
	public IShanghaiBankRepository ShanghaiBank => _shanghaiBank ??= new ShanghaiBankRepository(context);
	public ITaiwanCooperativeBankRepository TaiwanCooperativeBank => _taiwanCooperativeBank ??= new TaiwanCooperativeBankRepository(context);
	public IDepartmentRepository Department => _department ??= new DepartmentRepository(context);
	public IBankInitialBalanceRepository BankInitialBalance => _bankInitialBalance ??= new BankInitialBalanceRepository(context);

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await context.SaveChangesAsync(cancellationToken);
	}

	public void Dispose()
	{
		context.Dispose();
		GC.SuppressFinalize(this);
	}
}
