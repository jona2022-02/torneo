using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Teams.PatchTeam;

public sealed class PatchTeamHandler
{
    private readonly ITeamRepository _teams;
    private readonly IUnitOfWork _unitOfWork;

    public PatchTeamHandler(ITeamRepository teams, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PatchTeamCommand command, CancellationToken cancellationToken)
    {
        var validation = PatchTeamValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        if (command.Name is null)
        {
            return Result.Success();
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
        return renamed.IsFailure ? renamed : await _unitOfWork.CommitAsync(cancellationToken);
    }
}
