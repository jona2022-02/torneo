namespace Torneo.Application.Matches.RegisterMatchResult;

public sealed record RegisterMatchResultCommand(
    Guid MatchId,
    int HomeScore,
    int AwayScore,
    IReadOnlyList<GoalLine> Goals);
