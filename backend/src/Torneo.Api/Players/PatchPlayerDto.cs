using FluentValidation;
using Torneo.Domain.Players;

namespace Torneo.Api.Players;

public sealed class PatchPlayerDto
{
    public string? Name { get; init; }

    public int? ShirtNumber { get; init; }

    public string? Role { get; init; }
}

public sealed class PatchPlayerDtoValidator : AbstractValidator<PatchPlayerDto>
{
    public PatchPlayerDtoValidator()
    {
        When(dto => dto.Name is not null, () =>
        {
            RuleFor(dto => dto.Name)
                .Must(name => !string.IsNullOrWhiteSpace(name) && name!.Trim().Length <= Player.MaxNameLength)
                .WithMessage("El nombre del jugador no es válido.");
        });

        When(dto => dto.ShirtNumber is not null, () =>
        {
            RuleFor(dto => dto.ShirtNumber)
                .InclusiveBetween(Player.MinShirtNumber, Player.MaxShirtNumber)
                .WithMessage("El dorsal debe estar entre 1 y 99.");
        });

        When(dto => dto.Role is not null, () =>
        {
            RuleFor(dto => dto.Role)
                .Must(role => LineupRoles.TryParse(role, out _))
                .WithMessage("El puesto debe ser portero, defensa, volante o delantero.");
        });
    }
}
