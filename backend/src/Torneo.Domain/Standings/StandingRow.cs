namespace Torneo.Domain.Standings;

public sealed record StandingRow(
    Guid TeamId,
    string TeamName,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalDifference,
    int Points
    );
