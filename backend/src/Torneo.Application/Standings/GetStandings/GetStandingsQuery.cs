namespace Torneo.Application.Standings.GetStandings;

public sealed record GetStandingsQuery(int? PageNumber, int? PageSize, string? SortBy, string? SortDirection);
