namespace Torneo.Application.Players.GetPlayers;

public static class PlayerSort
{
    public static readonly IReadOnlySet<string> Columns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "name",
        "shirtNumber"
    };
}
