using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Matches.UpdateMatch;

public sealed class UpdateMatchHandler
{
    private readonly ITeamRepository _teams;
    private readonly IMatchRepository _matches;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMatchHandler(ITeamRepository teams, IMatchRepository matches, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _matches = matches;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateMatchCommand command, CancellationToken cancellationToken)
    {
        var validation = UpdateMatchValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var match = await _matches.GetByIdAsync(command.Id, cancellationToken);
        if (match is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró el partido.");
        }

        if (await _teams.GetByIdAsync(command.HomeTeamId, cancellationToken) is null
            || await _teams.GetByIdAsync(command.AwayTeamId, cancellationToken) is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró uno de los equipos.");
        }

        if (await _matches.DuplicateExistsAsync(command.HomeTeamId, command.AwayTeamId, command.Kickoff, match.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.Conflict, "Ya existe un partido con esos equipos en esa fecha.");
        }

        var replaced = match.ReplaceSchedule(command.HomeTeamId, command.AwayTeamId, command.Kickoff);
        return replaced.IsFailure ? replaced : await _unitOfWork.CommitAsync(cancellationToken);
    }
}
