namespace Torneo.Application.Matches.GetMatchById;

public sealed record MatchDetail(
    Guid Id,
    Guid HomeTeamId,
    string HomeTeamName,
    Guid AwayTeamId,
    string AwayTeamName,
    DateTimeOffset Kickoff,
    string Status,
    int? HomeScore,
    int? AwayScore,
    IReadOnlyList<MatchGoalItem> Goals);
