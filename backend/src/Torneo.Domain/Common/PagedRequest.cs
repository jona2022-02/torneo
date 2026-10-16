namespace Torneo.Domain.Common;

public sealed record PagedRequest
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 50;

    public PagedRequest(int? pageNumber, int? pageSize, string? sortBy, string? sortDirection)
    {
        PageNumber = pageNumber is null or < 1 ? DefaultPageNumber : pageNumber.Value;
        PageSize = pageSize is null or < 1
            ? DefaultPageSize
            : Math.Min(pageSize.Value, MaxPageSize);
        SortBy = string.IsNullOrWhiteSpace(sortBy) ? null : sortBy.Trim();
        SortDirection = string.IsNullOrWhiteSpace(sortDirection)
            ? "asc"
            : sortDirection.Trim().ToLowerInvariant();
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public string? SortBy { get; }

    public string SortDirection { get; }

    public int Offset => (PageNumber - 1) * PageSize;
}
