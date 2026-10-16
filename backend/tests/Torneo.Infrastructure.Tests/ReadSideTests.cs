using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Torneo.Application.Matches.CreateMatch;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Application.Players.CreatePlayer;
using Torneo.Application.Scorers.GetScorers;
using Torneo.Application.Standings.GetStandings;
using Torneo.Application.Teams.CreateTeam;
using Torneo.Application.Teams.GetTeams;
using Torneo.Infrastructure;
using Torneo.Infrastructure.Persistence;

namespace Torneo.Infrastructure.Tests;

public class ReadSideTests
{
    private const string ConnectionString =
        "Server=localhost,1433;Database=TorneoTests;User Id=sa;Password=TorneoDev_2026!;TrustServerCertificate=True;Encrypt=True";

    [Fact]
    public async Task Tabla_y_goleadores_salen_ordenados_desde_sql()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TorneoDbContext>();
        await context.Database.MigrateAsync();
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Goals; DELETE FROM Matches; DELETE FROM Players; DELETE FROM Teams;");

        var atlas = (await Handler<CreateTeamHandler>(scope).Handle(new CreateTeamCommand("Atlas"), CancellationToken.None)).Value;
        var boca = (await Handler<CreateTeamHandler>(scope).Handle(new CreateTeamCommand("Boca"), CancellationToken.None)).Value;
        var scorer = (await Handler<CreatePlayerHandler>(scope).Handle(new CreatePlayerCommand(atlas, "Juan", 9), CancellationToken.None)).Value;
        var kickoff = new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero);
        var played = (await Handler<CreateMatchHandler>(scope).Handle(new CreateMatchCommand(atlas, boca, kickoff), CancellationToken.None)).Value;
        var registered = await Handler<RegisterMatchResultHandler>(scope).Handle(
            new RegisterMatchResultCommand(played, 2, 0, [new GoalLine(scorer, 10), new GoalLine(scorer, 20)]),
            CancellationToken.None);
        Assert.True(registered.IsSuccess, registered.Error?.Message);
        await Handler<CreateMatchHandler>(scope).Handle(new CreateMatchCommand(boca, atlas, kickoff.AddDays(1)), CancellationToken.None);

        var table = await Handler<GetStandingsHandler>(scope).Handle(new GetStandingsQuery(1, 10, null, null), CancellationToken.None);
        Assert.True(table.IsSuccess, table.Error?.Message);
        Assert.Equal(2, table.Value.TotalRecords);
        Assert.Equal(atlas, table.Value.Data[0].TeamId);
        Assert.Equal(3, table.Value.Data[0].Points);
        Assert.Equal(2, table.Value.Data[0].GoalDifference);
        Assert.Equal(2, table.Value.Data[0].GoalsFor);
        Assert.Equal(1, table.Value.Data[0].Played);
        Assert.Equal(boca, table.Value.Data[1].TeamId);
        Assert.Equal(0, table.Value.Data[1].Points);
        Assert.Equal(-2, table.Value.Data[1].GoalDifference);

        var page = await Handler<GetStandingsHandler>(scope).Handle(new GetStandingsQuery(1, 1, null, null), CancellationToken.None);
        Assert.Single(page.Value.Data);
        Assert.Equal(2, page.Value.TotalRecords);

        var scorers = await Handler<GetScorersHandler>(scope).Handle(new GetScorersQuery(null, 1, 10, null, "desc"), CancellationToken.None);
        var row = Assert.Single(scorers.Value.Data);
        Assert.Equal(scorer, row.PlayerId);
        Assert.Equal(2, row.Goals);

        var filtered = await Handler<GetScorersHandler>(scope).Handle(new GetScorersQuery(boca, 1, 10, null, null), CancellationToken.None);
        Assert.Empty(filtered.Value.Data);

        var invalid = await Handler<GetTeamsHandler>(scope).Handle(new GetTeamsQuery(null, 1, 10, "puntos", "asc"), CancellationToken.None);
        Assert.False(invalid.IsSuccess);
    }

    private static T Handler<T>(AsyncServiceScope scope) where T : notnull => scope.ServiceProvider.GetRequiredService<T>();

    private static ServiceProvider BuildProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Torneo"] = ConnectionString
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInfrastructure(configuration);
        return services.BuildServiceProvider();
    }
}
