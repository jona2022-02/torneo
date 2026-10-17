using FluentValidation;

namespace Torneo.Api.Matches;

public sealed class CreateMatchDto
{
    public Guid HomeTeamId { get; init; }

    public Guid AwayTeamId { get; init; }

    public DateTimeOffset Kickoff { get; init; }
}

public sealed class CreateMatchDtoValidator : AbstractValidator<CreateMatchDto>
{
    public CreateMatchDtoValidator()
    {
        RuleFor(dto => dto.HomeTeamId)
            .NotEmpty()
            .WithMessage("El partido necesita equipo local y visitante.");

        RuleFor(dto => dto.AwayTeamId)
            .NotEmpty()
            .WithMessage("El partido necesita equipo local y visitante.");
    }
}
