namespace Torneo.Application.Matches.GetMatches;

public sealed record GetMatchesQuery(
    DateTimeOffset? From,
    DateTimeOffset? To,
    Guid? TeamId,
    string? Status,
    int? PageNumber,
    int? PageSize,
    string? SortBy,
    string? SortDirection);
