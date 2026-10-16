namespace Torneo.Application.Matches.GetMatches;

public sealed record MatchFilter(DateTimeOffset? From, DateTimeOffset? To, Guid? TeamId, int? Status);
