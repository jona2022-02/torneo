namespace Torneo.Application.Scorers.GetScorers;

public sealed record GetScorersQuery(Guid? TeamId, int? PageNumber, int? PageSize, string? SortBy, string? SortDirection);
