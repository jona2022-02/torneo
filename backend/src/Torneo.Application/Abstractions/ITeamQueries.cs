using Torneo.Application.Teams.GetTeamById;
using Torneo.Application.Teams.GetTeams;
using Torneo.Domain.Common;

namespace Torneo.Application.Abstractions;

public interface ITeamQueries
{
    Task<PagedResult<TeamListItem>> GetPageAsync(string? name, PagedRequest page, CancellationToken cancellationToken);

    Task<TeamDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
