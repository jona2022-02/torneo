using FluentValidation;
using Torneo.Domain.Teams;

namespace Torneo.Api.Teams;

public sealed class UpdateTeamDto
{
    public string? Name { get; init; }
}

public sealed class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(Team.MaxNameLength)
            .WithMessage("El nombre del equipo no puede superar 80 caracteres.");
    }
}
