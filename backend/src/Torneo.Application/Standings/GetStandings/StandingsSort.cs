namespace Torneo.Application.Standings.GetStandings;

public static class StandingsSort
{
    public static readonly IReadOnlySet<string> Columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "points",
        "goalDifference",
        "goalsFor",
        "teamName"
    };
}
