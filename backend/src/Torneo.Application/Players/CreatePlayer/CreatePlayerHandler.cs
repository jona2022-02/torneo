using Torneo.Application.Abstractions;
using Torneo.Application.Players;
using Torneo.Application.Common;
using Torneo.Domain.Common;
using Torneo.Domain.Players;

namespace Torneo.Application.Players.CreatePlayer;

public sealed class CreatePlayerHandler
{
    private readonly ITeamRepository _teams;
    private readonly IPlayerRepository _players;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlayerHandler(ITeamRepository teams, IPlayerRepository players, IUnitOfWork unitOfWork)
    {
        _teams = teams;
        _players = players;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        var validation = CreatePlayerValidator.Validate(command);
        if (validation.IsFailure)
        {
            return ResultMap.Failure<Guid>(validation);
        }

        if (await _teams.GetByIdAsync(command.TeamId, cancellationToken) is null)
        {
            return Result<Guid>.Failure(ErrorCodes.NotFound, "No se encontró el equipo.");
        }

        if (await _players.ShirtNumberExistsAsync(command.TeamId, command.ShirtNumber, null, cancellationToken))
        {
            return Result<Guid>.Failure(ErrorCodes.PlayerDuplicateNumber, "Ese dorsal ya está usado en el equipo.");
        }

        var goalkeeper = await GoalkeeperGuard.EnsureSingleAsync(_players, command.TeamId, command.Role, null, cancellationToken);
        if (goalkeeper.IsFailure)
        {
            return ResultMap.Failure<Guid>(goalkeeper);
        }

        var player = Player.Create(command.TeamId, command.Name, command.ShirtNumber, command.Role);
        if (player.IsFailure)
        {
            return ResultMap.Failure<Guid>(player);
        }

        _players.Add(player.Value);
        var committed = await _unitOfWork.CommitAsync(cancellationToken);
        return committed.IsSuccess
            ? Result<Guid>.Success(player.Value.Id)
            : ResultMap.Failure<Guid>(committed);
    }
}
