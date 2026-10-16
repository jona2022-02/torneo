using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;

namespace Torneo.Application.Auth.Login;

public sealed class LoginHandler
{
    private readonly IOperatorRepository _operators;
    private readonly IPasswordHasher _passwords;
    private readonly IAccessTokenIssuer _tokens;

    public LoginHandler(IOperatorRepository operators, IPasswordHasher passwords, IAccessTokenIssuer tokens)
    {
        _operators = operators;
        _passwords = passwords;
        _tokens = tokens;
    }

    public async Task<Result<LoginSession>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var validation = LoginValidator.Validate(command);
        if (validation.IsFailure)
        {
            return ResultMap.Failure<LoginSession>(validation);
        }

        var op = await _operators.FindByUsernameAsync(command.Username, cancellationToken);
        if (op is null || !_passwords.Verify(command.Password, op.PasswordHash))
        {
            return Result<LoginSession>.Failure(ErrorCodes.AuthInvalid, "Usuario o contraseña incorrectos.");
        }

        var token = _tokens.Issue(op.Id, op.Username, op.DisplayName);
        return Result<LoginSession>.Success(new LoginSession(token, op.Username, op.DisplayName));
    }
}
