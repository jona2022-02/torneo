namespace Torneo.Application.Matches.UpdateMatch;

public sealed record UpdateMatchCommand(Guid Id, Guid HomeTeamId, Guid AwayTeamId, DateTimeOffset Kickoff);
