using FluentValidation;
using Torneo.Domain.Players;

namespace Torneo.Api.Players;

public sealed class CreatePlayerDto
{
    public string? Name { get; init; }

    public int ShirtNumber { get; init; }
}

public sealed class CreatePlayerDtoValidator : AbstractValidator<CreatePlayerDto>
{
    public CreatePlayerDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length <= Player.MaxNameLength)
            .WithMessage("El nombre del jugador no es válido.");

        RuleFor(dto => dto.ShirtNumber)
            .InclusiveBetween(Player.MinShirtNumber, Player.MaxShirtNumber)
            .WithMessage("El dorsal debe estar entre 1 y 99.");
    }
}
