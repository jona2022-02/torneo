using Torneo.Domain.Common;

namespace Torneo.Application.Matches.DeleteMatch;

public static class DeleteMatchValidator
{
    public static Result Validate(DeleteMatchCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            return Result.Failure(ErrorCodes.Validation, "El partido es obligatorio.");
        }

        return Result.Success();
    }
}
