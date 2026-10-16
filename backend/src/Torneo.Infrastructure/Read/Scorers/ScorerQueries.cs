using Dapper;
using Torneo.Application.Abstractions;
using Torneo.Application.Scorers.GetScorers;
using Torneo.Domain.Common;

namespace Torneo.Infrastructure.Read.Scorers;

internal sealed class ScorerQueries : IScorerQueries
{
    private static readonly Dictionary<string, string> Columns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["goals"] = "Goals",
        ["playerName"] = "PlayerName",
        ["teamName"] = "TeamName"
    };

    private readonly SqlConnectionFactory _connections;

    public ScorerQueries(SqlConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<PagedResult<ScorerRow>> GetPageAsync(Guid? teamId, PagedRequest page, CancellationToken cancellationToken)
    {
        var orderBy = page.SortBy is null
            ? "Goals DESC, PlayerName ASC"
            : $"{SqlOrder.Column(page.SortBy, "Goals", Columns)} {SqlOrder.Direction(page.SortDirection)}, PlayerName ASC";

        var parameters = new { TeamId = teamId, page.Offset, page.PageSize };
        const string grouped = """
            FROM Goals g
            INNER JOIN Players p ON p.Id = g.PlayerId
            INNER JOIN Teams t ON t.Id = p.TeamId
            WHERE (@TeamId IS NULL OR p.TeamId = @TeamId)
            GROUP BY p.Id, p.Name, t.Id, t.Name
            """;

        var countSql = $"SELECT COUNT(1) FROM (SELECT p.Id {grouped}) AS Total";
        var pageSql = $"""
            SELECT p.Id AS PlayerId, p.Name AS PlayerName, t.Id AS TeamId, t.Name AS TeamName, COUNT(g.Id) AS Goals
            {grouped}
            ORDER BY {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var rows = (await connection.QueryAsync<ScorerRow>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
        return new PagedResult<ScorerRow>(rows, page.PageNumber, page.PageSize, total);
    }
}
