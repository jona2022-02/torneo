namespace Torneo.Application.Players.PatchPlayer;

public sealed record PatchPlayerCommand(Guid Id, string? Name, int? ShirtNumber);
