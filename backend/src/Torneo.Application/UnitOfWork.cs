using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IApplicationTransaction _transaction;

    public UnitOfWork(IApplicationTransaction transaction)
    {
        _transaction = transaction;
    }

    public async Task<Result> CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.BeginAsync(cancellationToken);

        try
        {
            await _transaction.SaveAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception)
        {
            await _transaction.RollbackAsync(cancellationToken);
            return Result.Failure(ErrorCodes.Conflict, "No se pudo confirmar la transacción.");
        }
    }
}
