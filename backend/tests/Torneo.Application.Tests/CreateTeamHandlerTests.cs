using Torneo.Application.Abstractions;
using Torneo.Application.Teams.CreateTeam;
using Torneo.Domain.Common;
using Torneo.Domain.Teams;

namespace Torneo.Application.Tests;

public class CreateTeamHandlerTests
{
    [Fact]
    public async Task Crear_equipo_confirma_la_transaccion_y_registra_el_evento()
    {
        var teams = new FakeTeamRepository();
        var unitOfWork = new CountingUnitOfWork();
        var handler = new CreateTeamHandler(teams, unitOfWork);

        var result = await handler.Handle(new CreateTeamCommand("Atlántida"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, unitOfWork.Commits);
        var team = Assert.Single(teams.Added);
        Assert.IsType<TeamCreated>(Assert.Single(team.DomainEvents));
    }

    [Fact]
    public async Task Nombre_duplicado_no_confirma_la_transaccion()
    {
        var teams = new FakeTeamRepository { NameTaken = true };
        var unitOfWork = new CountingUnitOfWork();
        var handler = new CreateTeamHandler(teams, unitOfWork);

        var result = await handler.Handle(new CreateTeamCommand("Atlántida"), CancellationToken.None);

        Assert.Equal(ErrorCodes.TeamNameDuplicate, result.Error!.Code);
        Assert.Equal(0, unitOfWork.Commits);
        Assert.Empty(teams.Added);
    }

    private sealed class FakeTeamRepository : ITeamRepository
    {
        public List<Team> Added { get; } = [];

        public bool NameTaken { get; init; }

        public Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => Task.FromResult<Team?>(null);

        public Task<bool> NameExistsAsync(string name, Guid? excludingId, CancellationToken cancellationToken) =>
            Task.FromResult(NameTaken);

        public void Add(Team team) => Added.Add(team);

        public void Remove(Team team)
        {
        }
    }

    private sealed class CountingUnitOfWork : IUnitOfWork
    {
        public int Commits { get; private set; }

        public Task<Result> CommitAsync(CancellationToken cancellationToken = default)
        {
            Commits++;
            return Task.FromResult(Result.Success());
        }
    }
}
