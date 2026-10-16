using Torneo.Domain.Common;
using Torneo.Domain.Teams;

namespace Torneo.Application.Teams.UpdateTeam;

public static class UpdateTeamValidator
{
    public static Result Validate(UpdateTeamCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El equipo es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del equipo es obligatorio.");
        }

        if (command.Name.Trim().Length > Team.MaxNameLength)
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del equipo no puede superar 80 caracteres.");
        }

        return Result.Success();
    }
}
