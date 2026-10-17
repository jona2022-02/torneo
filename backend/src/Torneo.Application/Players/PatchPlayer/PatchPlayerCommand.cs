using Torneo.Domain.Players;

namespace Torneo.Application.Players.PatchPlayer;

public sealed record PatchPlayerCommand(Guid Id, string? Name, int? ShirtNumber, LineupRole? Role = null);
