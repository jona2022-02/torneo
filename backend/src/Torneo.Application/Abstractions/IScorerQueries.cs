using Torneo.Application.Scorers.GetScorers;
using Torneo.Domain.Common;

namespace Torneo.Application.Abstractions;

public interface IScorerQueries
{
    Task<PagedResult<ScorerRow>> GetPageAsync(Guid? teamId, PagedRequest page, CancellationToken cancellationToken);
}
