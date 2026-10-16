using Dapper;
using Torneo.Application.Abstractions;
using Torneo.Domain.Common;
using Torneo.Domain.Standings;

namespace Torneo.Infrastructure.Read.Standings;

internal sealed class StandingsQueries : IStandingsQueries
{
    private static readonly Dictionary<string, string> Columns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["points"] = "Points",
        ["goalDifference"] = "GoalDifference",
        ["goalsFor"] = "GoalsFor",
        ["teamName"] = "TeamName"
    };

    private readonly SqlConnectionFactory _connections;

    public StandingsQueries(SqlConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<PagedResult<StandingRow>> GetPageAsync(PagedRequest page, CancellationToken cancellationToken)
    {
        var orderBy = page.SortBy is null
            ? "Points DESC, GoalDifference DESC, GoalsFor DESC, TeamName ASC"
            : $"{SqlOrder.Column(page.SortBy, "Points", Columns)} {SqlOrder.Direction(page.SortDirection)}, GoalDifference DESC, GoalsFor DESC, TeamName ASC";

        var parameters = new { page.Offset, page.PageSize };
        var pageSql = $"{StandingsSql.Body} ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(StandingsSql.Count, cancellationToken: cancellationToken));
        var rows = (await connection.QueryAsync<StandingRow>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
        return new PagedResult<StandingRow>(rows, page.PageNumber, page.PageSize, total);
    }
}
