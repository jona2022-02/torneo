using Torneo.Domain.Matches;
using Torneo.Domain.Teams;

namespace Torneo.Domain.Standings;

public static class StandingsCalculator
{
    public static IReadOnlyList<StandingRow> Calculate(IReadOnlyCollection<Team> teams, IReadOnlyCollection<Match> matches)
    {
        var rows = teams.ToDictionary(team => team.Id, team => new MutableRow(team.Id, team.Name));

        foreach (var match in matches)
        {
            if (match.Status != MatchStatus.Finalizado || match.HomeScore is null || match.AwayScore is null)
            {
                continue;
            }

            if (!rows.TryGetValue(match.HomeTeamId, out var home) || !rows.TryGetValue(match.AwayTeamId, out var away))
            {
                continue;
            }

            Apply(home, away, match.HomeScore.Value, match.AwayScore.Value);
        }

        return rows.Values
            .Select(row => row.ToRow())
            .OrderByDescending(row => row.Points)
            .ThenByDescending(row => row.GoalDifference)
            .ThenByDescending(row => row.GoalsFor)
            .ThenBy(row => row.TeamName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static void Apply(MutableRow home, MutableRow away, int homeScore, int awayScore)
    {
        home.Played++;
        away.Played++;
        home.GoalsFor += homeScore;
        home.GoalsAgainst += awayScore;
        away.GoalsFor += awayScore;
        away.GoalsAgainst += homeScore;

        if (homeScore > awayScore)
        {
            home.Won++;
            away.Lost++;
            return;
        }

        if (homeScore < awayScore)
        {
            away.Won++;
            home.Lost++;
            return;
        }

        home.Drawn++;
        away.Drawn++;
    }

    private sealed class MutableRow(Guid teamId, string teamName)
    {
        public int Played { get; set; }

        public int Won { get; set; }

        public int Drawn { get; set; }

        public int Lost { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public StandingRow ToRow() => new(
            teamId,
            teamName,
            Played,
            Won,
            Drawn,
            Lost,
            GoalsFor,
            GoalsAgainst,
            GoalsFor - GoalsAgainst,
            (Won * 3) + Drawn);
    }
}
