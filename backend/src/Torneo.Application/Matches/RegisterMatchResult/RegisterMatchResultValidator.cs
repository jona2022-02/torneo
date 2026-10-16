using Torneo.Domain.Common;
using Torneo.Domain.Goals;

namespace Torneo.Application.Matches.RegisterMatchResult;

public static class RegisterMatchResultValidator
{
    public static Result Validate(RegisterMatchResultCommand command)
    {
        if (command.MatchId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El partido es obligatorio.");
        }

        if (command.HomeScore < 0 || command.AwayScore < 0)
        {
            return Result.Failure(ErrorCodes.Validation, "El marcador no puede ser negativo.");
        }

        if (command.Goals is null || command.Goals.Any(goal => goal.PlayerId == Guid.Empty || goal.Minute is < Goal.MinMinute or > Goal.MaxMinute))
        {
            return Result.Failure(ErrorCodes.Validation, "Cada gol necesita un jugador y un minuto entre 1 y 120.");
        }

        return Result.Success();
    }
}
