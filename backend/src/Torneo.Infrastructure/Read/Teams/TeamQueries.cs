using Dapper;
using Torneo.Application.Abstractions;
using Torneo.Application.Teams.GetTeamById;
using Torneo.Application.Teams.GetTeams;
using Torneo.Domain.Common;

namespace Torneo.Infrastructure.Read.Teams;

internal sealed class TeamQueries : ITeamQueries
{
    private static readonly Dictionary<string, string> Columns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["name"] = "Name",
        ["createdAt"] = "CreatedAt"
    };

    private readonly SqlConnectionFactory _connections;

    public TeamQueries(SqlConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<PagedResult<TeamListItem>> GetPageAsync(string? name, PagedRequest page, CancellationToken cancellationToken)
    {
        var orderBy = $"{SqlOrder.Column(page.SortBy, "Name", Columns)} {SqlOrder.Direction(page.SortDirection)}";
        var parameters = new
        {
            Name = TextSearch.Contains(name),
            page.Offset,
            page.PageSize
        };

        var countSql = """
            SELECT COUNT(1)
            FROM Teams
            WHERE (@Name IS NULL OR Name LIKE @Name ESCAPE '\')
            """;

        var pageSql = $"""
            SELECT Id, Name, CreatedAt
            FROM Teams
            WHERE (@Name IS NULL OR Name LIKE @Name ESCAPE '\')
            ORDER BY {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var rows = (await connection.QueryAsync<TeamListItem>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
        return new PagedResult<TeamListItem>(rows, page.PageNumber, page.PageSize, total);
    }

    public async Task<TeamDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Name, CreatedAt
            FROM Teams
            WHERE Id = @Id
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<TeamDetail>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }
}
