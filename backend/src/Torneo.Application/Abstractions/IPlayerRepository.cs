using Torneo.Domain.Players;

namespace Torneo.Application.Abstractions;

public interface IPlayerRepository
{
    Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Player>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);

    Task<bool> ShirtNumberExistsAsync(Guid teamId, int shirtNumber, Guid? excludingId, CancellationToken cancellationToken);

    Task<bool> GoalkeeperExistsAsync(Guid teamId, Guid? excludingId, CancellationToken cancellationToken);

    Task<bool> AnyInTeamAsync(Guid teamId, CancellationToken cancellationToken);

    void Add(Player player);

    void Remove(Player player);
}
