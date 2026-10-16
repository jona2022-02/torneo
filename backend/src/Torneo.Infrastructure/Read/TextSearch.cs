namespace Torneo.Infrastructure.Read;

internal static class TextSearch
{
    public static string? Contains(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var escaped = value.Trim()
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_")
            .Replace("[", "\\[");

        return $"%{escaped}%";
    }
}
