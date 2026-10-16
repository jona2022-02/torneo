using Torneo.Application.Abstractions;
using Torneo.Domain.Common;
using Torneo.Domain.Matches;

namespace Torneo.Application.Matches.RegisterMatchResult;

public sealed class RegisterMatchResultHandler
{
    private readonly IMatchRepository _matches;
    private readonly IPlayerRepository _players;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterMatchResultHandler(IMatchRepository matches, IPlayerRepository players, IUnitOfWork unitOfWork)
    {
        _matches = matches;
        _players = players;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RegisterMatchResultCommand command, CancellationToken cancellationToken)
    {
        var validation = RegisterMatchResultValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var match = await _matches.GetByIdAsync(command.MatchId, cancellationToken);
        if (match is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró el partido.");
        }

        var playerIds = command.Goals.Select(goal => goal.PlayerId).Distinct().ToArray();
        var players = await _players.GetByIdsAsync(playerIds, cancellationToken);
        if (players.Count != playerIds.Length)
        {
            return Result.Failure(ErrorCodes.NotFound, "Uno de los goleadores no existe.");
        }

        var playersById = players.ToDictionary(player => player.Id);
        var goals = command.Goals
            .Select(goal => new GoalRegistration(goal.PlayerId, playersById[goal.PlayerId].TeamId, goal.Minute))
            .ToArray();

        var registered = match.RegisterResult(command.HomeScore, command.AwayScore, goals);
        return registered.IsFailure ? registered : await _unitOfWork.CommitAsync(cancellationToken);
    }
}
