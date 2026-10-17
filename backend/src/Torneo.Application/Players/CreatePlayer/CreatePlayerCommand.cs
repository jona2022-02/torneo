using Torneo.Domain.Players;

namespace Torneo.Application.Players.CreatePlayer;

public sealed record CreatePlayerCommand(Guid TeamId, string Name, int ShirtNumber, LineupRole Role = LineupRole.Defensa);
