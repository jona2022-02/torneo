using Dapper;
using Torneo.Application.Abstractions;
using Torneo.Application.Players.GetPlayerById;
using Torneo.Application.Players.GetPlayers;
using Torneo.Domain.Common;

namespace Torneo.Infrastructure.Read.Players;

internal sealed class PlayerQueries : IPlayerQueries
{
    private static readonly Dictionary<string, string> Columns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = "p.Name",
        ["shirtNumber"] = "p.ShirtNumber"
    };

    private readonly SqlConnectionFactory _connections;

    public PlayerQueries(SqlConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<PagedResult<PlayerListItem>> GetPageAsync(Guid? teamId, string? name, PagedRequest page, CancellationToken cancellationToken)
    {
        var orderBy = $"{SqlOrder.Column(page.SortBy, "p.Name", Columns)} {SqlOrder.Direction(page.SortDirection)}";
        var parameters = new
        {
            TeamId = teamId,
            Name = TextSearch.Contains(name),
            page.Offset,
            page.PageSize
        };

        const string filter = """
            FROM Players p
            INNER JOIN Teams t ON t.Id = p.TeamId
            WHERE (@TeamId IS NULL OR p.TeamId = @TeamId)
              AND (@Name IS NULL OR p.Name LIKE @Name ESCAPE '\')
            """;

        var countSql = $"SELECT COUNT(1) {filter}";
        var pageSql = $"""
            SELECT p.Id, p.TeamId, t.Name AS TeamName, p.Name, p.ShirtNumber,
                   CASE p.Role WHEN 0 THEN 'Portero' WHEN 2 THEN 'Volante' WHEN 3 THEN 'Delantero' ELSE 'Defensa' END AS Role
            {filter}
            ORDER BY {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var rows = (await connection.QueryAsync<PlayerListItem>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
        return new PagedResult<PlayerListItem>(rows, page.PageNumber, page.PageSize, total);
    }

    public async Task<PlayerDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT p.Id, p.TeamId, t.Name AS TeamName, p.Name, p.ShirtNumber,
                   CASE p.Role WHEN 0 THEN 'Portero' WHEN 2 THEN 'Volante' WHEN 3 THEN 'Delantero' ELSE 'Defensa' END AS Role
            FROM Players p
            INNER JOIN Teams t ON t.Id = p.TeamId
            WHERE p.Id = @Id
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<PlayerDetail>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }
}
