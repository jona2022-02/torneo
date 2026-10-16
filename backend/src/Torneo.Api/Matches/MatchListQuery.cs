namespace Torneo.Api.Matches;

public sealed class MatchListQuery
{
    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public Guid? TeamId { get; set; }

    public string? Status { get; set; }

    public int? PageNumber { get; set; }

    public int? PageSize { get; set; }

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; }
}
