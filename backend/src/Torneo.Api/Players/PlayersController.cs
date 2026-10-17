using Microsoft.AspNetCore.Mvc;
using Torneo.Api.Http;
using Torneo.Application.Players.CreatePlayer;
using Torneo.Application.Players.DeletePlayer;
using Torneo.Application.Players.GetPlayerById;
using Torneo.Application.Players.GetPlayers;
using Torneo.Application.Players.PatchPlayer;
using Torneo.Application.Players.UpdatePlayer;
using Torneo.Domain.Common;

namespace Torneo.Api.Players;

[ApiController]
[Route("api/players")]
public sealed class PlayersController(
    GetPlayersHandler list,
    GetPlayerByIdHandler byId,
    CreatePlayerHandler create,
    UpdatePlayerHandler update,
    PatchPlayerHandler patch,
    DeletePlayerHandler delete) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PlayerListItem>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> List(
        [FromQuery] PageQuery page,
        [FromQuery] Guid? teamId,
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await list.Handle(
            new GetPlayersQuery(teamId, name, page.PageNumber, page.PageSize, page.SortBy, page.SortDirection),
            cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PagedResult<PlayerDetail>), StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Get(Guid id, [FromQuery] PageQuery page, CancellationToken cancellationToken)
    {
        var result = await byId.Handle(new GetPlayerByIdQuery(id), cancellationToken);
        return HttpResults.Ok(SinglePage.Wrap(result, page));
    }

    [HttpPost("/api/teams/{teamId:guid}/players")]
    [ProducesResponseType(typeof(CreatedResource), StatusCodes.Status201Created)]
    public async Task<ResultEnvelope> Create(Guid teamId, [FromBody] CreatePlayerDto request, CancellationToken cancellationToken)
    {
        var result = await create.Handle(new CreatePlayerCommand(teamId, request.Name ?? "", request.ShirtNumber), cancellationToken);
        var location = result.IsSuccess ? $"/api/players/{result.Value}" : null;
        return HttpResults.Created(result, location);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Replace(Guid id, [FromBody] UpdatePlayerDto request, CancellationToken cancellationToken)
    {
        var result = await update.Handle(new UpdatePlayerCommand(id, request.Name ?? "", request.ShirtNumber), cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ResultEnvelope> Patch(Guid id, [FromBody] PatchPlayerDto request, CancellationToken cancellationToken)
    {
        var result = await patch.Handle(new PatchPlayerCommand(id, request.Name, request.ShirtNumber), cancellationToken);
        return HttpResults.Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ResultEnvelope> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await delete.Handle(new DeletePlayerCommand(id), cancellationToken);
        return HttpResults.NoContent(result);
    }
}
