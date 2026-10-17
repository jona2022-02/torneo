namespace Torneo.Application.Players.GetPlayers;

public sealed record PlayerListItem(Guid Id, Guid TeamId, string TeamName, string Name, int ShirtNumber, string Role);
