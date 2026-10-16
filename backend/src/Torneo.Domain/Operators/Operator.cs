using Torneo.Domain.Common;

namespace Torneo.Domain.Operators;

public sealed class Operator : Entity
{
    public const int MaxUsernameLength = 80;
    public const int MaxDisplayNameLength = 80;

    private Operator(Guid id, string username, string passwordHash, string displayName)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
    }

    public Guid Id { get; }

    public string Username { get; }

    public string PasswordHash { get; }

    public string DisplayName { get; }

    public static Result<Operator> Create(string username, string passwordHash, string displayName)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Trim().Length > MaxUsernameLength)
        {
            return Result<Operator>.Failure(ErrorCodes.Validation, "El usuario no es válido.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result<Operator>.Failure(ErrorCodes.Validation, "La contraseña no es válida.");
        }

        if (string.IsNullOrWhiteSpace(displayName) || displayName.Trim().Length > MaxDisplayNameLength)
        {
            return Result<Operator>.Failure(ErrorCodes.Validation, "El nombre del operador no es válido.");
        }

        return Result<Operator>.Success(new Operator(
            Guid.NewGuid(),
            username.Trim(),
            passwordHash,
            displayName.Trim()));
    }
}
