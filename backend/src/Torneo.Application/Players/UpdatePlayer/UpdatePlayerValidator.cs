using Torneo.Domain.Common;
using Torneo.Domain.Players;

namespace Torneo.Application.Players.UpdatePlayer;

public static class UpdatePlayerValidator
{
    public static Result Validate(UpdatePlayerCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El jugador es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(command.Name) || command.Name.Trim().Length > Player.MaxNameLength)
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
