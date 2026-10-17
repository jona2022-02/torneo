using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Teams.CreateTeam;
using Torneo.Application.Teams.DeleteTeam;
using Torneo.Application.Teams.GetTeamById;
using Torneo.Application.Teams.GetTeams;
using Torneo.Application.Teams.PatchTeam;
using Torneo.Application.Teams.UpdateTeam;
using Torneo.Domain.Common;

namespace Torneo.Api.Teams;

[ApiController]
[Route("api/teams")]
public sealed class TeamsController(
    GetTeamsHandler list,
    GetTeamByIdHandler byId,
    CreateTeamHandler create,
    UpdateTeamHandler update,
    PatchTeamHandler patch,
    DeleteTeamHandler delete) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<TeamListItem>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> List([FromQuery] PageQuery page, [FromQuery] string? name, CancellationToken cancellationToken)
    {
        var result = await list.Handle(
            new GetTeamsQuery(name, page.PageNumber, page.PageSize, page.SortBy, page.SortDirection),
            cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PagedResult<TeamDetail>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Get(Guid id, [FromQuery] PageQuery page, CancellationToken cancellationToken)
    {
        var result = await byId.Handle(new GetTeamByIdQuery(id), cancellationToken);
        return HttpResults.Ok(SinglePage.Wrap(result, page));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResource), StatusCodes.Status201Created)]
    public async Task<ResultEnvelope> Create([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreateTeamCommand(request.Name), cancellationToken);
        var location = result.IsSuccess ? $"/api/teams/{result.Value}" : null;
        return HttpResults.Created(result, location);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Replace(Guid id, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdateTeamCommand(id, request.Name), cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Patch(Guid id, [FromBody] PatchTeamRequest request, CancellationToken cancellationToken)
    {
        var result = await patch.Handle(new PatchTeamCommand(id, request.Name), cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ResultEnvelope> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await delete.Handle(new DeleteTeamCommand(id), cancellationToken);
        return HttpResults.NoContent(result);
    }
}
