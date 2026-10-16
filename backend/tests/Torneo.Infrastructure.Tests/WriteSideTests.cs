using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Torneo.Application.Matches.CreateMatch;
using Torneo.Application.Matches.DeleteMatch;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Application.Players.CreatePlayer;
using Torneo.Application.Players.DeletePlayer;
using Torneo.Application.Teams.CreateTeam;
using Torneo.Application.Teams.DeleteTeam;
using Torneo.Domain.Matches;
using Torneo.Infrastructure;
using Torneo.Infrastructure.Persistence;

namespace Torneo.Infrastructure.Tests;

public class WriteSideTests
{
    private const string ConnectionString =
        "Server=localhost,1433;Database=Torneo;User Id=sa;Password=TorneoDev_2026!;TrustServerCertificate=True;Encrypt=True";

    [Fact]
    public async Task Equipo_jugador_y_resultado_se_guardan()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TorneoDbContext>();
        await context.Database.MigrateAsync();

        var suffix = Guid.NewGuid().ToString("N")[..8];
        var homeId = (await scope.ServiceProvider.GetRequiredService<CreateTeamHandler>()
            .Handle(new CreateTeamCommand($"Local {suffix}"), CancellationToken.None)).Value;
        var awayId = (await scope.ServiceProvider.GetRequiredService<CreateTeamHandler>()
            .Handle(new CreateTeamCommand($"Visita {suffix}"), CancellationToken.None)).Value;
        var playerId = (await scope.ServiceProvider.GetRequiredService<CreatePlayerHandler>()
            .Handle(new CreatePlayerCommand(homeId, "Juan", 9), CancellationToken.None)).Value;
        var kickoff = new DateTimeOffset(2026, 10, 19, 15, 0, 0, TimeSpan.Zero);
        var matchId = (await scope.ServiceProvider.GetRequiredService<CreateMatchHandler>()
            .Handle(new CreateMatchCommand(homeId, awayId, kickoff), CancellationToken.None)).Value;
        var registered = await scope.ServiceProvider.GetRequiredService<RegisterMatchResultHandler>()
            .Handle(new RegisterMatchResultCommand(matchId, 1, 0, [new GoalLine(playerId, 12)]), CancellationToken.None);

        Assert.True(registered.IsSuccess, registered.Error?.Message);

        await using var verify = provider.CreateAsyncScope();
        var database = verify.ServiceProvider.GetRequiredService<TorneoDbContext>();
        var match = await database.Matches.Include(item => item.Goals).SingleAsync(item => item.Id == matchId);
        Assert.Equal(MatchStatus.Finalizado, match.Status);
        Assert.Equal(1, match.HomeScore);
        Assert.Equal(playerId, Assert.Single(match.Goals).PlayerId);
        Assert.True(await database.Teams.AnyAsync(team => team.Id == homeId));
        Assert.True(await database.Players.AnyAsync(player => player.Id == playerId));

        await verify.ServiceProvider.GetRequiredService<DeleteMatchHandler>()
            .Handle(new DeleteMatchCommand(matchId), CancellationToken.None);
        await verify.ServiceProvider.GetRequiredService<DeletePlayerHandler>()
            .Handle(new DeletePlayerCommand(playerId), CancellationToken.None);
        await verify.ServiceProvider.GetRequiredService<DeleteTeamHandler>()
            .Handle(new DeleteTeamCommand(homeId), CancellationToken.None);
        await verify.ServiceProvider.GetRequiredService<DeleteTeamHandler>()
            .Handle(new DeleteTeamCommand(awayId), CancellationToken.None);
    }

    [Fact]
    public async Task Rollback_revierte_equipo_y_jugador()
    {
        await using var provider = BuildProvider();
        await using var scope = provider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TorneoDbContext>();
        await context.Database.MigrateAsync();

        await using var transaction = await context.Database.BeginTransactionAsync();
        var home = Torneo.Domain.Teams.Team.Create($"Tx {Guid.NewGuid():N}").Value;
        var away = Torneo.Domain.Teams.Team.Create($"Tx {Guid.NewGuid():N}").Value;
        context.Teams.Add(home);
        context.Teams.Add(away);
        await context.SaveChangesAsync();
        var player = Torneo.Domain.Players.Player.Create(home.Id, "Ana", 7).Value;
        context.Players.Add(player);
        await context.SaveChangesAsync();
        await transaction.RollbackAsync();

        await using var verify = provider.CreateAsyncScope();
        var database = verify.ServiceProvider.GetRequiredService<TorneoDbContext>();
        Assert.False(await database.Teams.AnyAsync(item => item.Id == home.Id));
        Assert.False(await database.Players.AnyAsync(item => item.Id == player.Id));
    }

    private static ServiceProvider BuildProvider()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Torneo"] = ConnectionString,
                ["Auth:SigningKey"] = "TorneoDev_SigningKey_2026_Mundialito"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddInfrastructure(configuration);
        return services.BuildServiceProvider();
    }
}
