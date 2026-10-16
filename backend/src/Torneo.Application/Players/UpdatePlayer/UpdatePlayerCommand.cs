namespace Torneo.Application.Players.UpdatePlayer;

public sealed record UpdatePlayerCommand(Guid Id, string Name, int ShirtNumber);
