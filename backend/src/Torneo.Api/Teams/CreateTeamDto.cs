using FluentValidation;
using Torneo.Domain.Teams;

namespace Torneo.Api.Teams;

public sealed class CreateTeamDto
{
    public string? Name { get; init; }
}

public sealed class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(Team.MaxNameLength)
            .WithMessage("El nombre del equipo no puede superar 80 caracteres.");
    }
}
