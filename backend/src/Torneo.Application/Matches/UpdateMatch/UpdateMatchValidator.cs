using Torneo.Domain.Common;

namespace Torneo.Application.Matches.UpdateMatch;

public static class UpdateMatchValidator
{
    public static Result Validate(UpdateMatchCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El partido es obligatorio.");
        }

        if (command.HomeTeamId == Guid.Empty || command.AwayTeamId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El partido necesita equipo local y visitante.");
        }

        if (command.HomeTeamId == command.AwayTeamId)
        {
            return Result.Failure(ErrorCodes.MatchSameTeam, "El equipo local y el visitante no pueden ser el mismo.");
        }

        return Result.Success();
    }
}
