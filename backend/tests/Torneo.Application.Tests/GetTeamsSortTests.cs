using Torneo.Application.Abstractions;
using Torneo.Application.Teams.GetTeamById;
using Torneo.Application.Teams.GetTeams;
using Torneo.Domain.Common;

namespace Torneo.Application.Tests;

public class GetTeamsSortTests
{
    [Fact]
    public async Task SortBy_invalido_devuelve_error_de_validacion()
    {
        var queries = new UnusedTeamQueries();
        var handler = new GetTeamsHandler(queries);

        var result = await handler.Handle(
            new GetTeamsQuery(null, 1, 10, "no-existe", "asc"),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorCodes.Validation, result.Error!.Code);
        Assert.Equal(0, queries.Calls);
    }

    [Fact]
    public async Task SortDirection_invalida_devuelve_error_de_validacion()
    {
        var queries = new UnusedTeamQueries();
        var handler = new GetTeamsHandler(queries);

        var result = await handler.Handle(
            new GetTeamsQuery(null, 1, 10, "name", "lado"),
            CancellationToken.None);

        Assert.Equal(ErrorCodes.Validation, result.Error!.Code);
        Assert.Equal(0, queries.Calls);
    }

    private sealed class UnusedTeamQueries : ITeamQueries
    {
        public int Calls { get; private set; }

        public Task<PagedResult<TeamListItem>> GetPageAsync(string? name, PagedRequest page, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(new PagedResult<TeamListItem>([], page.PageNumber, page.PageSize, 0));
        }

        public Task<TeamDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult<TeamDetail?>(null);
        }
    }
}
