using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Application.Players.GetPlayers;

public sealed class GetPlayersHandler
{
    private readonly IPlayerQueries _queries;

    public GetPlayersHandler(IPlayerQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PagedResult<PlayerListItem>>> Handle(GetPlayersQuery query, CancellationToken cancellationToken)
    {
        var page = new PagedRequest(query.PageNumber, query.PageSize, query.SortBy, query.SortDirection);
        var sort = SortGuard.Validate(page.SortBy, page.SortDirection, PlayerSort.Columns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<PlayerListItem>>(sort);
        }

        var data = await _queries.GetPageAsync(query.TeamId, query.Name, page, cancellationToken);
        return Result<PagedResult<PlayerListItem>>.Success(data);
    }
}
