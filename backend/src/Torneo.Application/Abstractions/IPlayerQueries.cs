using Torneo.Application.Players.GetPlayerById;
using Torneo.Application.Players.GetPlayers;
using Torneo.Domain.Common;

namespace Torneo.Application.Abstractions;

public interface IPlayerQueries
{
    Task<PagedResult<PlayerListItem>> GetPageAsync(Guid? teamId, string? name, PagedRequest page, CancellationToken cancellationToken);

    Task<PlayerDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
