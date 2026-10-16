using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Teams.UpdateTeam;

public sealed class UpdateTeamHandler
{
    private readonly ITeamRepository _teams;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTeamHandler(ITeamRepository teams, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateTeamCommand command, CancellationToken cancellationToken)
    {
        var validation = UpdateTeamValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var team = await _teams.GetByIdAsync(command.Id, cancellationToken);
        if (team is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró el equipo.");
        }

        if (await _teams.NameExistsAsync(command.Name, team.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.TeamNameDuplicate, "Ya existe un equipo con ese nombre.");
        }

        var renamed = team.Rename(command.Name);
        if (renamed.IsFailure)
        {
            return renamed;
        }

        return await _unitOfWork.CommitAsync(cancellationToken);
    }
}
