namespace Torneo.Application.Players.GetPlayers;

public sealed record GetPlayersQuery(
    Guid? TeamId,
    string? Name,
    int? PageNumber,
    int? PageSize,
    string? SortBy,
    string? SortDirection);
