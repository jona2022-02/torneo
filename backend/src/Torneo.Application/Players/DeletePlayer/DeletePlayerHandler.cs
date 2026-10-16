using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Players.DeletePlayer;

public sealed class DeletePlayerHandler
{
    private readonly IPlayerRepository _players;
    private readonly IMatchRepository _matches;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePlayerHandler(IPlayerRepository players, IMatchRepository matches, IUnitOfWork unitOfWork)
    {
        _players = players;
        _matches = matches;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePlayerCommand command, CancellationToken cancellationToken)
    {
        var validation = DeletePlayerValidator.Validate(command);
        if (validation.IsFailure)
        {
            return validation;
        }

        var player = await _players.GetByIdAsync(command.Id, cancellationToken);
        if (player is null)
        {
            return Result.Success();
        }

        if (await _matches.PlayerHasGoalsAsync(player.Id, cancellationToken))
        {
            return Result.Failure(ErrorCodes.PlayerHasGoals, "El jugador tiene goles y no se puede eliminar.");
        }

        _players.Remove(player);
        return await _unitOfWork.CommitAsync(cancellationToken);
    }
}
