using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Teams.GetTeamById;

public sealed class GetTeamByIdHandler
{
    private readonly ITeamQueries _queries;

    public GetTeamByIdHandler(ITeamQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<TeamDetail>> Handle(GetTeamByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Id == Guid.Empty)
        {
            return Result<TeamDetail>.Failure(ErrorCodes.Validation, "El equipo es obligatorio.");
        }

        var team = await _queries.GetByIdAsync(query.Id, cancellationToken);
        return team is null
            ? Result<TeamDetail>.Failure(ErrorCodes.NotFound, "No se encontró el equipo.")
            : Result<TeamDetail>.Success(team);
    }
}
