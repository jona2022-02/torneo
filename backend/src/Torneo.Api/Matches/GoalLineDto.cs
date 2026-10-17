using FluentValidation;
using Torneo.Domain.Goals;

namespace Torneo.Api.Matches;

public sealed class GoalLineDto
{
    public Guid PlayerId { get; init; }

    public int Minute { get; init; }
}

public sealed class GoalLineDtoValidator : AbstractValidator<GoalLineDto>
{
    public GoalLineDtoValidator()
    {
        RuleFor(dto => dto)
            .Must(dto => dto.PlayerId != Guid.Empty && dto.Minute is >= Goal.MinMinute and <= Goal.MaxMinute)
            .WithMessage("Cada gol necesita un jugador y un minuto entre 1 y 120.");
    }
}
