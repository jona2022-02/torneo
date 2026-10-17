using FluentValidation;
using Torneo.Domain.Teams;

namespace Torneo.Api.Teams;

public sealed class PatchTeamDto
{
    public string? Name { get; init; }
}

public sealed class PatchTeamDtoValidator : AbstractValidator<PatchTeamDto>
{
    public PatchTeamDtoValidator()
    {
        When(dto => dto.Name is not null, () =>
        {
            RuleFor(dto => dto.Name)
                .Must(name => !string.IsNullOrWhiteSpace(name) && name!.Trim().Length <= Team.MaxNameLength)
                .WithMessage("El nombre del equipo no es válido.");
        });
    }
}
