using Torneo.Domain.Common;
using Torneo.Domain.Standings;

namespace Torneo.Application.Abstractions;

public interface IStandingsQueries
{
    Task<PagedResult<StandingRow>> GetPageAsync(PagedRequest page, CancellationToken cancellationToken);
}
