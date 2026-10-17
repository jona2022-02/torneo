using Torneo.Application.Abstractions;
using Torneo.Domain.Common;
using Torneo.Domain.Players;

namespace Torneo.Application.Players;

public static class GoalkeeperGuard
{
    public static async Task<Result> EnsureSingleAsync(
        IPlayerRepository players,
        Guid teamId,
        LineupRole role,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        if (role != LineupRole.Portero)
        {
            return Result.Success();
        }

        var taken = await players.GoalkeeperExistsAsync(teamId, excludingId, cancellationToken);
        return taken
            ? Result.Failure(ErrorCodes.PlayerGoalkeeperTaken, "El equipo ya tiene un portero.")
            : Result.Success();
    }
}
