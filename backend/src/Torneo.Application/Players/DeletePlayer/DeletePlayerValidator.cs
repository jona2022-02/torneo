using Torneo.Domain.Common;

namespace Torneo.Application.Players.DeletePlayer;

public static class DeletePlayerValidator
{
    public static Result Validate(DeletePlayerCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El jugador es obligatorio.");
        }

        return Result.Success();
    }
}
