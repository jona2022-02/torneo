using Torneo.Application.Abstractions;
using Torneo.Domain.Common;

namespace Torneo.Application.Matches.GetMatchById;

public sealed class GetMatchByIdHandler
{
    private readonly IMatchQueries _queries;

    public GetMatchByIdHandler(IMatchQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<MatchDetail>> Handle(GetMatchByIdQuery query, CancellationToken cancellationToken)
    {
        if (query.Id == Guid.Empty)
        {
            return Result<MatchDetail>.Failure(ErrorCodes.Validation, "El partido es obligatorio.");
        }

        var match = await _queries.GetByIdAsync(query.Id, cancellationToken);
        return match is null
            ? Result<MatchDetail>.Failure(ErrorCodes.NotFound, "No se encontró el partido.")
            : Result<MatchDetail>.Success(match);
    }
}
