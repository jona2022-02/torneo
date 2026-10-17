using Torneo.Domain.Players;

namespace Torneo.Application.Players.UpdatePlayer;

public sealed record UpdatePlayerCommand(Guid Id, string Name, int ShirtNumber, LineupRole Role = LineupRole.Defensa);
