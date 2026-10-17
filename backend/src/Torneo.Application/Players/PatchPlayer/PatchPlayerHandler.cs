using Torneo.Application.Abstractions;
using Torneo.Application.Players;
using Torneo.Domain.Common;

namespace Torneo.Application.Players.PatchPlayer;

public sealed class PatchPlayerHandler
{
    private readonly IPlayerRepository _players;
    private readonly IUnitOfWork _unitOfWork;

    public PatchPlayerHandler(IPlayerRepository players, IUnitOfWork unitOfWork)
    {
        _players = players;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(PatchPlayerCommand command, CancellationToken cancellationToken)
    {
        var validation = PatchPlayerValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        if (command.Name is null && command.ShirtNumber is null && command.Role is null)
        {
            return Result.Success();
        }

        var player = await _players.GetByIdAsync(command.Id, cancellationToken);
        if (player is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró el jugador.");
        }

        var name = command.Name ?? player.Name;
        var shirtNumber = command.ShirtNumber ?? player.ShirtNumber;
        var role = command.Role ?? player.Role;
        if (await _players.ShirtNumberExistsAsync(player.TeamId, shirtNumber, player.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.PlayerDuplicateNumber, "Ese dorsal ya está usado en el equipo.");
        }

        var goalkeeper = await GoalkeeperGuard.EnsureSingleAsync(_players, player.TeamId, role, player.Id, cancellationToken);
        if (goalkeeper.IsFailure)
        {
            return goalkeeper;
        }

        var updated = player.Update(name, shirtNumber, role);
        return updated.IsFailure ? updated : await _unitOfWork.CommitAsync(cancellationToken);
    }
}
