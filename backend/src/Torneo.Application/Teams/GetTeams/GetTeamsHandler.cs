using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Application.Teams.GetTeams;

public sealed class GetTeamsHandler
{
    private readonly ITeamQueries _queries;

    public GetTeamsHandler(ITeamQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PagedResult<TeamListItem>>> Handle(GetTeamsQuery query, CancellationToken cancellationToken)
    {
        var page = new PagedRequest(query.PageNumber, query.PageSize, query.SortBy, query.SortDirection);
        var sort = SortGuard.Validate(page.SortBy, page.SortDirection, TeamSort.Columns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<TeamListItem>>(sort);
        }

        var data = await _queries.GetPageAsync(query.Name, page, cancellationToken);
        return Result<PagedResult<TeamListItem>>.Success(data);
    }
}
