using Torneo.Domain.Common;

namespace Torneo.Application.Auth.Login;

public static class LoginValidator
{
    public static Result Validate(LoginCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
        {
            return Result.Failure(ErrorCodes.Validation, "El usuario y la contraseña son obligatorios.");
        }

        return Result.Success();
    }
}
