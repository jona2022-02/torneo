using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Teams.DeleteTeam;

public sealed class DeleteTeamHandler
{
    private readonly ITeamRepository _teams;
    private readonly IPlayerRepository _players;
    private readonly IMatchRepository _matches;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTeamHandler(
        ITeamRepository teams,
        IPlayerRepository players,
        IMatchRepository matches,
        IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _players = players;
        _matches = matches;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTeamCommand command, CancellationToken cancellationToken)
    {
        var validation = DeleteTeamValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var team = await _teams.GetByIdAsync(command.Id, cancellationToken);
        if (team is null)
        {
            return Result.Success();
        }

        if (await _players.AnyInTeamAsync(team.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.Conflict, "El equipo tiene jugadores.");
        }

        if (await _matches.TeamHasMatchesAsync(team.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.TeamHasMatches, "El equipo tiene partidos y no se puede eliminar.");
        }

        _teams.Remove(team);
        return await _unitOfWork.CommitAsync(cancellationToken);
    }
}
