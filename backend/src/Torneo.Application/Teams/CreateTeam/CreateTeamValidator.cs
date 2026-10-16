using Torneo.Domain.Common;
using Torneo.Domain.Teams;

namespace Torneo.Application.Teams.CreateTeam;

public static class CreateTeamValidator
{
    public static Result Validate(CreateTeamCommand command)
    {
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
