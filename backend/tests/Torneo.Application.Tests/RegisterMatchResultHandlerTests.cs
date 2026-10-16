using Torneo.Application.Abstractions;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Domain.Common;
using Torneo.Domain.Matches;
using Torneo.Domain.Players;

namespace Torneo.Application.Tests;

public class RegisterMatchResultHandlerTests
{
    [Fact]
    public async Task Registrar_resultado_confirma_y_publica_el_evento()
    {
        var home = Guid.NewGuid();
        var away = Guid.NewGuid();
        var match = Match.Create(home, away, new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero)).Value;
        var player = Player.Create(home, "Juan", 9).Value;
        var matches = new FakeMatchRepository(match);
        var players = new FakePlayerRepository(player);
        var unitOfWork = new CountingUnitOfWork();
        var handler = new RegisterMatchResultHandler(matches, players, unitOfWork);

        var result = await handler.Handle(
            new RegisterMatchResultCommand(match.Id, 1, 0, [new GoalLine(player.Id, 12)]),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, unitOfWork.Commits);
        Assert.Equal(MatchStatus.Finalizado, match.Status);
        Assert.IsType<MatchResultRegistered>(Assert.Single(match.DomainEvents));
    }

    [Fact]
    public async Task Marcador_incoherente_no_confirma_la_transaccion()
    {
        var home = Guid.NewGuid();
        var away = Guid.NewGuid();
        var match = Match.Create(home, away, new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero)).Value;
        var player = Player.Create(home, "Juan", 9).Value;
        var unitOfWork = new CountingUnitOfWork();
        var handler = new RegisterMatchResultHandler(new FakeMatchRepository(match), new FakePlayerRepository(player), unitOfWork);

        var result = await handler.Handle(
            new RegisterMatchResultCommand(match.Id, 2, 0, [new GoalLine(player.Id, 12)]),
            CancellationToken.None);

        Assert.Equal(ErrorCodes.MatchResultGoalMismatch, result.Error!.Code);
        Assert.Equal(0, unitOfWork.Commits);
    }

    private sealed class FakeMatchRepository : IMatchRepository
    {
        private readonly Match _match;

        public FakeMatchRepository(Match match) => _match = match;

        public Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult<Match?>(id == _match.Id ? _match : null);

        public Task<bool> TeamHasMatchesAsync(Guid teamId, CancellationToken cancellationToken) => Task.FromResult(false);

        public Task<bool> PlayerHasGoalsAsync(Guid playerId, CancellationToken cancellationToken) => Task.FromResult(false);

        public Task<bool> DuplicateExistsAsync(Guid homeTeamId, Guid awayTeamId, DateTimeOffset kickoff, Guid? excludingId, CancellationToken cancellationToken) =>
            Task.FromResult(false);

        public void Add(Match match)
        {
        }

        public void Remove(Match match)
        {
        }
    }

    private sealed class FakePlayerRepository : IPlayerRepository
    {
        private readonly Player _player;

        public FakePlayerRepository(Player player) => _player = player;

        public Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
            Task.FromResult<Player?>(id == _player.Id ? _player : null);

        public Task<IReadOnlyList<Player>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<Player>>(ids.Contains(_player.Id) ? [_player] : []);

        public Task<bool> ShirtNumberExistsAsync(Guid teamId, int shirtNumber, Guid? excludingId, CancellationToken cancellationToken) =>
            Task.FromResult(false);

        public Task<bool> AnyInTeamAsync(Guid teamId, CancellationToken cancellationToken) => Task.FromResult(false);

        public void Add(Player player)
        {
        }

        public void Remove(Player player)
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
