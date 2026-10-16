using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;
using Torneo.Domain.Standings;

namespace Torneo.Application.Standings.GetStandings;

public sealed class GetStandingsHandler
{
    private readonly IStandingsQueries _queries;

    public GetStandingsHandler(IStandingsQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PagedResult<StandingRow>>> Handle(GetStandingsQuery query, CancellationToken cancellationToken)
    {
        var page = new PagedRequest(query.PageNumber, query.PageSize, query.SortBy, query.SortDirection);
        var sort = SortGuard.Validate(page.SortBy, page.SortDirection, StandingsSort.Columns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<StandingRow>>(sort);
        }

        var data = await _queries.GetPageAsync(page, cancellationToken);
        return Result<PagedResult<StandingRow>>.Success(data);
    }
}
