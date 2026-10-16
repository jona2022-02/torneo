using Microsoft.EntityFrameworkCore;
using Torneo.Application.Abstractions;
using Torneo.Application.Idempotency;

namespace Torneo.Infrastructure.Persistence;

internal sealed class IdempotencyStore : IIdempotencyStore
{
    private readonly TorneoDbContext _context;

    public IdempotencyStore(TorneoDbContext context)
    {
        _context = context;
    }

    public async Task<StoredResponse?> FindAsync(string key, CancellationToken cancellationToken)
    {
        var record = await _context.IdempotencyRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(row => row.Key == key, cancellationToken);

        return record is null
            ? null
            : new StoredResponse(record.Key, record.RequestHash, record.StatusCode, record.ResponseBody);
    }

    public async Task SaveAsync(StoredResponse response, CancellationToken cancellationToken)
    {
        _context.IdempotencyRecords.Add(new IdempotencyRecord
        {
            Key = response.Key,
            RequestHash = response.RequestHash,
            StatusCode = response.StatusCode,
            ResponseBody = response.ResponseBody,
            CreatedAt = DateTimeOffset.UtcNow
        });

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            foreach (var entry in _context.ChangeTracker.Entries<IdempotencyRecord>().ToArray())
            {
                entry.State = EntityState.Detached;
            }
        }
    }
}
