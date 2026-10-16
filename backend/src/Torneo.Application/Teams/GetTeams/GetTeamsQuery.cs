namespace Torneo.Application.Teams.GetTeams;

public sealed record GetTeamsQuery(
    string? Name,
    int? PageNumber,
    int? PageSize,
    string? SortBy,
    string? SortDirection);
