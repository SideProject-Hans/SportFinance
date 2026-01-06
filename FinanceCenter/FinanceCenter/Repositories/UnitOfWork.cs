using FinanceCenter.Data;

namespace FinanceCenter.Repositories;

/// <summary>
/// Unit of Work 實作 - 管理交易範圍
/// </summary>
public class UnitOfWork(FinanceCenterDbContext context) : IUnitOfWork
{
	private IFinanceRepository? _finance;

	public IFinanceRepository Finance => _finance ??= new FinanceRepository(context);

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
