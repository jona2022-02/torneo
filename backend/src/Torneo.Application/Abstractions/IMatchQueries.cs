using Torneo.Application.Matches.GetMatchById;
using Torneo.Application.Matches.GetMatches;
using Torneo.Domain.Common;

namespace Torneo.Application.Abstractions;

public interface IMatchQueries
{
    Task<PagedResult<MatchListItem>> GetPageAsync(MatchFilter filter, PagedRequest page, CancellationToken cancellationToken);

    Task<MatchDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
