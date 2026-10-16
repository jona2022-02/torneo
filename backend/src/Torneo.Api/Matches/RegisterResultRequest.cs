namespace Torneo.Api.Matches;

public sealed record RegisterResultRequest(int HomeScore, int AwayScore, IReadOnlyList<GoalLineRequest>? Goals);
