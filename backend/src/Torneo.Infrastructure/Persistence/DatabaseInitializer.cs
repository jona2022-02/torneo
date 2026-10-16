using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Torneo.Infrastructure.Persistence;

internal sealed class DatabaseInitializer : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IServiceScopeFactory scopeFactory, ILogger<DatabaseInitializer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TorneoDbContext>();

        await context.Database.MigrateAsync(cancellationToken);
        if (!await context.Teams.AnyAsync(cancellationToken))
        {
            await scope.ServiceProvider.GetRequiredService<TournamentSeeder>().SeedAsync(cancellationToken);
        }

        if (!await context.Database.CanConnectAsync(cancellationToken))
        {
            throw new InvalidOperationException("SQL Server no aceptó la conexión.");
        }

        _logger.LogInformation("SQL Server aceptó la conexión.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
