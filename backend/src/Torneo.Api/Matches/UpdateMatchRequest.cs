namespace Torneo.Api.Matches;

public sealed record UpdateMatchRequest(Guid HomeTeamId, Guid AwayTeamId, DateTimeOffset Kickoff);
