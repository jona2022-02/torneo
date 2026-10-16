using Torneo.Domain.Common;

namespace Torneo.Domain.Goals;

public sealed class Goal : Entity
{
    public const int MinMinute = 1;
    public const int MaxMinute = 120;

    private Goal(Guid id, Guid matchId, Guid playerId, Guid teamId, int minute)
    {
        Id = id;
        MatchId = matchId;
        PlayerId = playerId;
        TeamId = teamId;
        Minute = minute;
    }

    public Guid Id { get; }

    public Guid MatchId { get; }

    public Guid PlayerId { get; }

    public Guid TeamId { get; }

    public int Minute { get; }

    internal static Result<Goal> Create(Guid matchId, Guid playerId, Guid teamId, int minute)
    {
        if (minute is < MinMinute or > MaxMinute)
        {
            return Result<Goal>.Failure(ErrorCodes.Validation, "El minuto debe estar entre 1 y 120.");
        }

        return Result<Goal>.Success(new Goal(Guid.NewGuid(), matchId, playerId, teamId, minute));
    }
}
