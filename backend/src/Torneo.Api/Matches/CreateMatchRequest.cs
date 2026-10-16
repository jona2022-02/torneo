namespace Torneo.Api.Matches;

public sealed record CreateMatchRequest(Guid HomeTeamId, Guid AwayTeamId, DateTimeOffset Kickoff);
