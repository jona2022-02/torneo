using Microsoft.Extensions.Logging;
using Torneo.Application.Matches.CreateMatch;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Application.Players.CreatePlayer;
using Torneo.Application.Teams.CreateTeam;

namespace Torneo.Infrastructure.Persistence;

internal sealed class TournamentSeeder
{
    private readonly CreateTeamHandler _teams;
    private readonly CreatePlayerHandler _players;
    private readonly CreateMatchHandler _matches;
    private readonly RegisterMatchResultHandler _results;
    private readonly ILogger<TournamentSeeder> _logger;

    public TournamentSeeder(
        CreateTeamHandler teams,
        CreatePlayerHandler players,
        CreateMatchHandler matches,
        RegisterMatchResultHandler results,
        ILogger<TournamentSeeder> logger)
    {
        _teams = teams;
        _players = players;
        _matches = matches;
        _results = results;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var teams = new Dictionary<string, Guid>();
        var players = new Dictionary<(string Team, int Shirt), Guid>();

        foreach (var name in TournamentCatalog.Teams)
        {
            var created = await _teams.Handle(new CreateTeamCommand(name), cancellationToken);
            if (created.IsFailure)
            {
                throw new InvalidOperationException(created.Error!.Message);
            }

            teams[name] = created.Value;
        }

        for (var index = 0; index < TournamentCatalog.Teams.Length; index++)
        {
            var team = TournamentCatalog.Teams[index];
            for (var shirt = 1; shirt <= TournamentCatalog.Players[index].Length; shirt++)
            {
                var created = await _players.Handle(
                    new CreatePlayerCommand(teams[team], TournamentCatalog.Players[index][shirt - 1], shirt),
                    cancellationToken);
                if (created.IsFailure)
                {
                    throw new InvalidOperationException(created.Error!.Message);
                }

                players[(team, shirt)] = created.Value;
            }
        }

        foreach (var fixture in TournamentCatalog.Fixtures)
        {
            await SaveFixture(fixture, teams, players, cancellationToken);
        }

        _logger.LogInformation("Semilla del Mundialito cargada.");
    }

    private async Task SaveFixture(
        SeedFixture fixture,
        Dictionary<string, Guid> teams,
        Dictionary<(string Team, int Shirt), Guid> players,
        CancellationToken cancellationToken)
    {
        var created = await _matches.Handle(
            new CreateMatchCommand(teams[fixture.Home], teams[fixture.Away], fixture.Kickoff),
            cancellationToken);
        if (created.IsFailure)
        {
            throw new InvalidOperationException(created.Error!.Message);
        }

        if (!fixture.Finished)
        {
            return;
        }

        var goals = fixture.Goals
            .Select(goal => new GoalLine(players[(goal.Team, goal.Shirt)], goal.Minute))
            .ToArray();
        var registered = await _results.Handle(
            new RegisterMatchResultCommand(created.Value, fixture.HomeScore, fixture.AwayScore, goals),
            cancellationToken);
        if (registered.IsFailure)
        {
            throw new InvalidOperationException(registered.Error!.Message);
        }
    }
}
