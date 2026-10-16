using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Application.Scorers.GetScorers;

public sealed class GetScorersHandler
{
    private readonly IScorerQueries _queries;

    public GetScorersHandler(IScorerQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PagedResult<ScorerRow>>> Handle(GetScorersQuery query, CancellationToken cancellationToken)
    {
        var page = new PagedRequest(query.PageNumber, query.PageSize, query.SortBy, query.SortDirection);
        var sort = SortGuard.Validate(page.SortBy, page.SortDirection, ScorerSort.Columns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<ScorerRow>>(sort);
        }

        var data = await _queries.GetPageAsync(query.TeamId, page, cancellationToken);
        return Result<PagedResult<ScorerRow>>.Success(data);
    }
}
