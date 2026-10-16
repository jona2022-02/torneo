using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Scorers.GetScorers;
using Torneo.Domain.Common;

namespace Torneo.Api.Scorers;

[ApiController]
[Route("api/scorers")]
public sealed class ScorersController(GetScorersHandler scorers) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ScorerRow>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> List([FromQuery] PageQuery page, [FromQuery] Guid? teamId, CancellationToken cancellationToken)
    {
        var result = await scorers.Handle(
            new GetScorersQuery(teamId, page.PageNumber, page.PageSize, page.SortBy, page.SortDirection),
            cancellationToken);
        return HttpResults.Ok(result);
    }
}
