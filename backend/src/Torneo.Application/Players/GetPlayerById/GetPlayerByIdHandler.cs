using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Players.GetPlayerById;

public sealed class GetPlayerByIdHandler
{
    private readonly IPlayerQueries _queries;

    public GetPlayerByIdHandler(IPlayerQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PlayerDetail>> Handle(GetPlayerByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Id == Guid.Empty)
        {
            return Result<PlayerDetail>.Failure(ErrorCodes.Validation, "El jugador es obligatorio.");
        }

        var player = await _queries.GetByIdAsync(query.Id, cancellationToken);
        return player is null
            ? Result<PlayerDetail>.Failure(ErrorCodes.NotFound, "No se encontró el jugador.")
            : Result<PlayerDetail>.Success(player);
    }
}
