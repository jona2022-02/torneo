namespace Torneo.Domain.Players;

public enum LineupRole
{
    Portero = 0,
    Defensa = 1,
    Volante = 2,
    Delantero = 3
}

public static class LineupRoles
{
    public static bool TryParse(string? text, out LineupRole role)
    {
        role = text?.Trim().ToLowerInvariant() switch
        {
            "portero" => LineupRole.Portero,
            "defensa" => LineupRole.Defensa,
            "volante" => LineupRole.Volante,
            "delantero" => LineupRole.Delantero,
            _ => default
        };
        return text is not null && text.Trim().ToLowerInvariant() is "portero" or "defensa" or "volante" or "delantero";
    }
}
