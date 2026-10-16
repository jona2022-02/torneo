using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Application.Matches.GetMatches;

public sealed class GetMatchesHandler
{
    private readonly IMatchQueries _queries;

    public GetMatchesHandler(IMatchQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PagedResult<MatchListItem>>> Handle(GetMatchesQuery query, CancellationToken cancellationToken)
    {
        var page = new PagedRequest(query.PageNumber, query.PageSize, query.SortBy, query.SortDirection);
        var sort = SortGuard.Validate(page.SortBy, page.SortDirection, MatchSort.Columns);
        if (sort.IsFailure)
        {
            return ResultMap.Failure<PagedResult<MatchListItem>>(sort);
        }

        if (query.From is not null && query.To is not null && query.From > query.To)
        {
            return Result<PagedResult<MatchListItem>>.Failure(ErrorCodes.Validation, "La fecha inicial no puede ser posterior a la final.");
        }

        var status = ParseStatus(query.Status);
        if (status.IsFailure)
        {
            return ResultMap.Failure<PagedResult<MatchListItem>>(status);
        }

        var data = await _queries.GetPageAsync(new MatchFilter(query.From, query.To, query.TeamId, status.Value), page, cancellationToken);
        return Result<PagedResult<MatchListItem>>.Success(data);
    }

    private static Result<int?> ParseStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return Result<int?>.Success(null);
        }

        if (status.Equals("Programado", StringComparison.OrdinalIgnoreCase))
        {
            return Result<int?>.Success(0);
        }

        if (status.Equals("Finalizado", StringComparison.OrdinalIgnoreCase))
        {
            return Result<int?>.Success(1);
        }

        return Result<int?>.Failure(ErrorCodes.Validation, "El estado del partido no es válido.");
    }
}
