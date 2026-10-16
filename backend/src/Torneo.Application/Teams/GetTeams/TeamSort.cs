namespace Torneo.Application.Teams.GetTeams;

public static class TeamSort
{
    public static readonly IReadOnlySet<string> Columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "name",
        "createdAt"
    };
}
