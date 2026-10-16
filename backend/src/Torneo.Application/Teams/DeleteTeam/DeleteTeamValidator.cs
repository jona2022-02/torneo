using Torneo.Domain.Common;

namespace Torneo.Application.Teams.DeleteTeam;

public static class DeleteTeamValidator
{
    public static Result Validate(DeleteTeamCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El equipo es obligatorio.");
        }

        return Result.Success();
    }
}
