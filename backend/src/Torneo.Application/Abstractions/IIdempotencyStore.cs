using Torneo.Application.Idempotency;

namespace Torneo.Application.Abstractions;

public interface IIdempotencyStore
{
    Task<StoredResponse?> FindAsync(string key, CancellationToken cancellationToken);

    Task SaveAsync(StoredResponse response, CancellationToken cancellationToken);
}
