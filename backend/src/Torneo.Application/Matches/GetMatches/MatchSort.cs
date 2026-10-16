namespace Torneo.Application.Matches.GetMatches;

public static class MatchSort
{
    public static readonly IReadOnlySet<string> Columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "kickoff",
        "status"
    };
}
