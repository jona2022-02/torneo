using Torneo.Domain.Common;

namespace Torneo.Domain.Teams;

public sealed record TeamCreated(Guid TeamId, string Name, DateTimeOffset OccurredAt) : IDomainEvent;
