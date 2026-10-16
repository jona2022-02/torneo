namespace Torneo.Application.Scorers.GetScorers;

public sealed record ScorerRow(Guid PlayerId, string PlayerName, Guid TeamId, string TeamName, int Goals);
