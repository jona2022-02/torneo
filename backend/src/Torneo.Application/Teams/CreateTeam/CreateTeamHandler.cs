using Torneo.Application.Abstractions;
using Torneo.Application.Common;
using Torneo.Domain.Common;
using Torneo.Domain.Teams;

namespace Torneo.Application.Teams.CreateTeam;

public sealed class CreateTeamHandler
{
    private readonly ITeamRepository _teams;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamHandler(ITeamRepository teams, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateTeamCommand command, CancellationToken cancellationToken)
    {
        var validation = CreateTeamValidator.Validate(command);
        if (validation.IsFailure)
        {
            return ResultMap.Failure<Guid>(validation);
        }

        if (await _teams.NameExistsAsync(command.Name, null, cancellationToken))
        {
            return Result<Guid>.Failure(ErrorCodes.TeamNameDuplicate, "Ya existe un equipo con ese nombre.");
        }

        var team = Team.Create(command.Name);
        if (team.IsFailure)
        {
            return ResultMap.Failure<Guid>(team);
        }

        _teams.Add(team.Value);
        var committed = await _unitOfWork.CommitAsync(cancellationToken);
        return committed.IsSuccess
            ? Result<Guid>.Success(team.Value.Id)
            : ResultMap.Failure<Guid>(committed);
    }
}
