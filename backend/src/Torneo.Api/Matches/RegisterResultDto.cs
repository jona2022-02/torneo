using FluentValidation;

namespace Torneo.Api.Matches;

public sealed class RegisterResultDto
{
    public int HomeScore { get; init; }

    public int AwayScore { get; init; }

    public IReadOnlyList<GoalLineDto>? Goals { get; init; }
}

public sealed class RegisterResultDtoValidator : AbstractValidator<RegisterResultDto>
{
    public RegisterResultDtoValidator()
    {
        RuleFor(dto => dto.HomeScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El marcador no puede ser negativo.");

        RuleFor(dto => dto.AwayScore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El marcador no puede ser negativo.");

        RuleFor(dto => dto.Goals)
            .NotNull()
            .WithMessage("Cada gol necesita un jugador y un minuto entre 1 y 120.");

        RuleForEach(dto => dto.Goals)
            .SetValidator(new GoalLineDtoValidator());
    }
}
