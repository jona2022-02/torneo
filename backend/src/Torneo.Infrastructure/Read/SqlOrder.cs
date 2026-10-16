namespace Torneo.Infrastructure.Read;

internal static class SqlOrder
{
    public static string Column(string? sortBy, string fallback, IReadOnlyDictionary<string, string> columns)
    {
        if (sortBy is not null && columns.TryGetValue(sortBy, out var column))
        {
            return column;
        }

        return fallback;
    }

    public static string Direction(string sortDirection) =>
        string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
}
