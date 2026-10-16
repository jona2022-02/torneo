namespace Torneo.Application.Scorers.GetScorers;

public static class ScorerSort
{
    public static readonly IReadOnlySet<string> Columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "goals",
        "playerName",
        "teamName"
    };
}
