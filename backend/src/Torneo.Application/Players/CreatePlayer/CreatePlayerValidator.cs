using Torneo.Domain.Common;
using Torneo.Domain.Players;

namespace Torneo.Application.Players.CreatePlayer;

public static class CreatePlayerValidator
{
    public static Result Validate(CreatePlayerCommand command)
    {
        if (command.TeamId == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El jugador debe pertenecer a un equipo.");
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
