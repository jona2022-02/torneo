namespace Torneo.Api.Http;

public sealed class PageQuery
{
    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }
}
