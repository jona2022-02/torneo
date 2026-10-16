namespace Torneo.Application.Matches.GetMatchById;

public sealed record MatchGoalItem(Guid PlayerId, string PlayerName, Guid TeamId, int Minute);
