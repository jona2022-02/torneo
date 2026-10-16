using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;
using Torneo.Domain.Matches;

namespace Torneo.Application.Matches.CreateMatch;

public sealed class CreateMatchHandler
{
    private readonly ITeamRepository _teams;
    private readonly IMatchRepository _matches;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMatchHandler(ITeamRepository teams, IMatchRepository matches, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _matches = matches;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateMatchCommand command, CancellationToken cancellationToken)
    {
        var validation = CreateMatchValidator.Validate(command);
        if (validation.IsFailure)
        {
            return ResultMap.Failure<Guid>(validation);
        }

        if (await _teams.GetByIdAsync(command.HomeTeamId, cancellationToken) is null
            || await _teams.GetByIdAsync(command.AwayTeamId, cancellationToken) is null)
        {
            return Result<Guid>.Failure(ErrorCodes.NotFound, "No se encontró uno de los equipos.");
        }

        if (await _matches.DuplicateExistsAsync(command.HomeTeamId, command.AwayTeamId, command.Kickoff, null, cancellationToken))
        {
            return Result<Guid>.Failure(ErrorCodes.Conflict, "Ya existe un partido con esos equipos en esa fecha.");
        }

        var match = Match.Create(command.HomeTeamId, command.AwayTeamId, command.Kickoff);
        if (match.IsFailure)
        {
            return ResultMap.Failure<Guid>(match);
        }

        _matches.Add(match.Value);
        var committed = await _unitOfWork.CommitAsync(cancellationToken);
        return committed.IsSuccess
            ? Result<Guid>.Success(match.Value.Id)
            : ResultMap.Failure<Guid>(committed);
    }
}
