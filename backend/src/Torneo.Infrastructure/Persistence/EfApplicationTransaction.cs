using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Torneo.Application.Abstractions;
using Torneo.Infrastructure.Events;

namespace Torneo.Infrastructure.Persistence;

internal sealed class EfApplicationTransaction : IApplicationTransaction
{
    private readonly TorneoDbContext _context;
    private readonly ILogger<EfApplicationTransaction> _logger;
    private readonly DomainEventDispatcher _events;
    private IDbContextTransaction? _transaction;

    public EfApplicationTransaction(
        TorneoDbContext context,
        ILogger<EfApplicationTransaction> logger,
        DomainEventDispatcher events)
    {
        _context = context;
        _logger = logger;
        _events = events;
    }

    public async Task BeginAsync(CancellationToken cancellationToken)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "No se pudieron guardar los cambios.");
            throw;
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.CommitAsync(cancellationToken);
        _events.Dispatch();
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null)
        {
            return;
        }

        await _transaction.RollbackAsync(cancellationToken);
        _events.Discard();
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
