using Torneo.Domain.Common;
using Torneo.Domain.Teams;

namespace Torneo.Application.Teams.PatchTeam;

public static class PatchTeamValidator
{
    public static Result Validate(PatchTeamCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El equipo es obligatorio.");
        }

        if (command.Name is null)
        {
            return Result.Success();
        }

        if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Trim().Length > Team.MaxNameLength)
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del equipo no es válido.");
        }

        return Result.Success();
    }
}
