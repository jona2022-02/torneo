using Torneo.Domain.Common;

namespace Torneo.Domain.Matches;

public sealed record MatchResultRegistered(
    Guid MatchId,
    int HomeScore,
    int AwayScore,
    DateTimeOffset OccurredAt) : IDomainEvent;
