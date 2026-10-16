using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Players.UpdatePlayer;

public sealed class UpdatePlayerHandler
{
    private readonly IPlayerRepository _players;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePlayerHandler(IPlayerRepository players, IUnitOfWork unitOfWork)
    {
        _players = players;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdatePlayerCommand command, CancellationToken cancellationToken)
    {
        var validation = UpdatePlayerValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var player = await _players.GetByIdAsync(command.Id, cancellationToken);
        if (player is null)
        {
            return Result.Failure(ErrorCodes.NotFound, "No se encontró el jugador.");
        }

        if (await _players.ShirtNumberExistsAsync(player.TeamId, command.ShirtNumber, player.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.PlayerDuplicateNumber, "Ese dorsal ya está usado en el equipo.");
        }

        var updated = player.Update(command.Name, command.ShirtNumber);
        return updated.IsFailure ? updated : await _unitOfWork.CommitAsync(cancellationToken);
    }
}
