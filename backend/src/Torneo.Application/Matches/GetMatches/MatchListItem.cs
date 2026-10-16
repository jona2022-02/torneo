namespace Torneo.Application.Matches.GetMatches;

public sealed record MatchListItem(
    Guid Id,
    Guid HomeTeamId,
    string HomeTeamName,
    Guid AwayTeamId,
    string AwayTeamName,
    DateTimeOffset Kickoff,
    string Status,
    int? HomeScore,
    int? AwayScore);
