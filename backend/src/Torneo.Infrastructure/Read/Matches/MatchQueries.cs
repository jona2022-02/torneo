using Dapper;
using Torneo.Application.Abstractions;
using Torneo.Application.Matches.GetMatchById;
using Torneo.Application.Matches.GetMatches;
using Torneo.Domain.Common;

namespace Torneo.Infrastructure.Read.Matches;

internal sealed class MatchQueries : IMatchQueries
{
    private static readonly Dictionary<string, string> Columns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["kickoff"] = "m.Kickoff",
        ["status"] = "m.Status"
    };

    private readonly SqlConnectionFactory _connections;

    public MatchQueries(SqlConnectionFactory connections)
    {
        _connections = connections;
    }

    public async Task<PagedResult<MatchListItem>> GetPageAsync(MatchFilter filter, PagedRequest page, CancellationToken cancellationToken)
    {
        var orderBy = $"{SqlOrder.Column(page.SortBy, "m.Kickoff", Columns)} {SqlOrder.Direction(page.SortDirection)}";
        var parameters = new
        {
            filter.From,
            filter.To,
            filter.TeamId,
            filter.Status,
            page.Offset,
            page.PageSize
        };

        const string filterSql = """
            FROM Matches m
            INNER JOIN Teams home ON home.Id = m.HomeTeamId
            INNER JOIN Teams away ON away.Id = m.AwayTeamId
            WHERE (@TeamId IS NULL OR m.HomeTeamId = @TeamId OR m.AwayTeamId = @TeamId)
              AND (@From IS NULL OR m.Kickoff >= @From)
              AND (@To IS NULL OR m.Kickoff <= @To)
              AND (@Status IS NULL OR m.Status = @Status)
            """;

        var countSql = $"SELECT COUNT(1) {filterSql}";
        var pageSql = $"""
            SELECT m.Id, m.HomeTeamId, home.Name AS HomeTeamName, m.AwayTeamId, away.Name AS AwayTeamName,
                   m.Kickoff,
                   CASE m.Status WHEN 1 THEN 'Finalizado' ELSE 'Programado' END AS Status,
                   m.HomeScore, m.AwayScore
            {filterSql}
            ORDER BY {orderBy}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var total = await connection.ExecuteScalarAsync<int>(new CommandDefinition(countSql, parameters, cancellationToken: cancellationToken));
        var rows = (await connection.QueryAsync<MatchListItem>(new CommandDefinition(pageSql, parameters, cancellationToken: cancellationToken))).AsList();
        return new PagedResult<MatchListItem>(rows, page.PageNumber, page.PageSize, total);
    }

    public async Task<MatchDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string matchSql = """
            SELECT m.Id, m.HomeTeamId, home.Name AS HomeTeamName, m.AwayTeamId, away.Name AS AwayTeamName,
                   m.Kickoff,
                   CASE m.Status WHEN 1 THEN 'Finalizado' ELSE 'Programado' END AS Status,
                   m.HomeScore, m.AwayScore
            FROM Matches m
            INNER JOIN Teams home ON home.Id = m.HomeTeamId
            INNER JOIN Teams away ON away.Id = m.AwayTeamId
            WHERE m.Id = @Id
            """;

        const string goalsSql = """
            SELECT g.PlayerId, p.Name AS PlayerName, g.TeamId, g.Minute
            FROM Goals g
            INNER JOIN Players p ON p.Id = g.PlayerId
            WHERE g.MatchId = @Id
            ORDER BY g.Minute
            """;

        await using var connection = _connections.Create();
        await connection.OpenAsync(cancellationToken);
        var match = await connection.QuerySingleOrDefaultAsync<MatchRow>(new CommandDefinition(matchSql, new { Id = id }, cancellationToken: cancellationToken));
        if (match is null)
        {
            return null;
        }

        var goals = (await connection.QueryAsync<MatchGoalItem>(new CommandDefinition(goalsSql, new { Id = id }, cancellationToken: cancellationToken))).AsList();
        return new MatchDetail(
            match.Id,
            match.HomeTeamId,
            match.HomeTeamName,
            match.AwayTeamId,
            match.AwayTeamName,
            match.Kickoff,
            match.Status,
            match.HomeScore,
            match.AwayScore,
            goals);
    }

    private sealed record MatchRow(
        Guid Id,
        Guid HomeTeamId,
        string HomeTeamName,
        Guid AwayTeamId,
        string AwayTeamName,
        DateTimeOffset Kickoff,
        string Status,
        int? HomeScore,
        int? AwayScore);
}
