using FluentValidation;

namespace Torneo.Api.Auth;

public sealed class LoginDto
{
    public string? Username { get; init; }

    public string? Password { get; init; }
}

public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(dto => dto.Username)
            .NotEmpty()
            .WithMessage("El usuario y la contraseña son obligatorios.");

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .WithMessage("El usuario y la contraseña son obligatorios.");
    }
}
