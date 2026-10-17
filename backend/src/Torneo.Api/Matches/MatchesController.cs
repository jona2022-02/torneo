using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Matches.CreateMatch;
using Torneo.Application.Matches.DeleteMatch;
using Torneo.Application.Matches.GetMatchById;
using Torneo.Application.Matches.GetMatches;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Application.Matches.UpdateMatch;
using Torneo.Domain.Common;

namespace Torneo.Api.Matches;

[ApiController]
[Route("api/matches")]
public sealed class MatchesController(
    GetMatchesHandler list,
    GetMatchByIdHandler byId,
    CreateMatchHandler create,
    UpdateMatchHandler update,
    DeleteMatchHandler delete,
    RegisterMatchResultHandler register) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<MatchListItem>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> List([FromQuery] MatchListQuery query, CancellationToken cancellationToken)
    {
        var result = await list.Handle(
            new GetMatchesQuery(query.From, query.To, query.TeamId, query.Status, query.PageNumber, query.PageSize, query.SortBy, query.SortDirection),
            cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PagedResult<MatchDetail>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Get(Guid id, [FromQuery] PageQuery page, CancellationToken cancellationToken)
    {
        var result = await byId.Handle(new GetMatchByIdQuery(id), cancellationToken);
        return HttpResults.Ok(SinglePage.Wrap(result, page));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResource), StatusCodes.Status201Created)]
    public async Task<ResultEnvelope> Create([FromBody] CreateMatchDto request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateMatchCommand(request.HomeTeamId, request.AwayTeamId, request.Kickoff), cancellationToken);
        var location = result.IsSuccess ? $"/api/matches/{result.Value}" : null;
        return HttpResults.Created(result, location);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Replace(Guid id, [FromBody] UpdateMatchDto request, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateMatchCommand(id, request.HomeTeamId, request.AwayTeamId, request.Kickoff), cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ResultEnvelope> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await delete.Handle(new DeleteMatchCommand(id), cancellationToken);
        return HttpResults.NoContent(result);
    }

    [HttpPut("{id:guid}/result")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> RegisterResult(Guid id, [FromBody] RegisterResultDto request, CancellationToken cancellationToken)
    {
        var goals = (request.Goals ?? []).Select(goal => new GoalLine(goal.PlayerId, goal.Minute)).ToArray();
        var result = await register.Handle(new RegisterMatchResultCommand(id, request.HomeScore, request.AwayScore, goals), cancellationToken);
        return HttpResults.Ok(result);
    }
}
