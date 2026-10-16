using Torneo.Domain.Common;

namespace Torneo.Application.Matches.CreateMatch;

public static class CreateMatchValidator
{
    public static Result Validate(CreateMatchCommand command)
    {
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
