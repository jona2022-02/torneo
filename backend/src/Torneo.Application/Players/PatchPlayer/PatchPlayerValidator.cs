using Torneo.Domain.Common;
using Torneo.Domain.Players;

namespace Torneo.Application.Players.PatchPlayer;

public static class PatchPlayerValidator
{
    public static Result Validate(PatchPlayerCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El jugador es obligatorio.");
        }

        if (command.Name is null && command.ShirtNumber is null)
        {
            return Result.Success();
        }

        if (command.Name is not null && (string.IsNullOrWhiteSpace(command.Name) || command.Name.Trim().Length > Player.MaxNameLength))
        {
            return Result.Failure(ErrorCodes.Validation, "El nombre del jugador no es válido.");
        }

        if (command.ShirtNumber is < Player.MinShirtNumber or > Player.MaxShirtNumber)
        {
            return Result.Failure(ErrorCodes.Validation, "El dorsal debe estar entre 1 y 99.");
        }

        return Result.Success();
    }
}
