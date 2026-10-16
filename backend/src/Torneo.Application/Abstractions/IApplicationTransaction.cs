namespace Torneo.Application.Abstractions;

public interface IApplicationTransaction
{
    Task BeginAsync(CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}
