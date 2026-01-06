namespace FinanceCenter.Repositories;

/// <summary>
/// Unit of Work 介面 - 管理交易範圍
/// </summary>
public interface IUnitOfWork : IDisposable
{
	IFinanceRepository Finance { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
