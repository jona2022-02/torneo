using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Standings.GetStandings;
using Torneo.Domain.Common;
using Torneo.Domain.Standings;

namespace Torneo.Api.Standings;

[ApiController]
[Route("api/standings")]
public sealed class StandingsController(GetStandingsHandler standings) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<StandingRow>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> List([FromQuery] PageQuery page, CancellationToken cancellationToken)
    {
        var result = await standings.Handle(
            new GetStandingsQuery(page.PageNumber, page.PageSize, page.SortBy, page.SortDirection),
            cancellationToken);
        return HttpResults.Ok(result);
    }
}
