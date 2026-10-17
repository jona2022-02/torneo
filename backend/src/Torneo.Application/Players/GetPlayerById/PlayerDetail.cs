namespace Torneo.Application.Players.GetPlayerById;

public sealed record PlayerDetail(Guid Id, Guid TeamId, string TeamName, string Name, int ShirtNumber, string Role);
