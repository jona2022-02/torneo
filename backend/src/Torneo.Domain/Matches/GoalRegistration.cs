namespace Torneo.Domain.Matches;

public sealed record GoalRegistration(Guid PlayerId, Guid TeamId, int Minute);
