namespace Torneo.Application.Matches.CreateMatch;

public sealed record CreateMatchCommand(Guid HomeTeamId, Guid AwayTeamId, DateTimeOffset Kickoff);
