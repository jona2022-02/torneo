namespace Torneo.Infrastructure.Read.Standings;

internal static class StandingsSql
{
    public const string Count = "SELECT COUNT(1) FROM Teams";

    public const string Body = """
        SELECT
            t.Id AS TeamId,
            t.Name AS TeamName,
            COALESCE(SUM(r.Played), 0) AS Played,
            COALESCE(SUM(r.Won), 0) AS Won,
            COALESCE(SUM(r.Drawn), 0) AS Drawn,
            COALESCE(SUM(r.Lost), 0) AS Lost,
            COALESCE(SUM(r.GoalsFor), 0) AS GoalsFor,
            COALESCE(SUM(r.GoalsAgainst), 0) AS GoalsAgainst,
            COALESCE(SUM(r.GoalsFor), 0) - COALESCE(SUM(r.GoalsAgainst), 0) AS GoalDifference,
            (COALESCE(SUM(r.Won), 0) * 3) + COALESCE(SUM(r.Drawn), 0) AS Points
        FROM Teams t
        LEFT JOIN (
            SELECT HomeTeamId AS TeamId, 1 AS Played,
                   CASE WHEN HomeScore > AwayScore THEN 1 ELSE 0 END AS Won,
                   CASE WHEN HomeScore = AwayScore THEN 1 ELSE 0 END AS Drawn,
                   CASE WHEN HomeScore < AwayScore THEN 1 ELSE 0 END AS Lost,
                   HomeScore AS GoalsFor, AwayScore AS GoalsAgainst
            FROM Matches
            WHERE Status = 1
            UNION ALL
            SELECT AwayTeamId, 1,
                   CASE WHEN AwayScore > HomeScore THEN 1 ELSE 0 END,
                   CASE WHEN HomeScore = AwayScore THEN 1 ELSE 0 END,
                   CASE WHEN AwayScore < HomeScore THEN 1 ELSE 0 END,
                   AwayScore, HomeScore
            FROM Matches
            WHERE Status = 1
        ) r ON r.TeamId = t.Id
        GROUP BY t.Id, t.Name
        """;
}
