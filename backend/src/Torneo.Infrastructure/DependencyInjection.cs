using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Torneo.Application;
using Torneo.Application.Abstractions;
using Torneo.Application.Matches.CreateMatch;
using Torneo.Application.Matches.DeleteMatch;
using Torneo.Application.Matches.RegisterMatchResult;
using Torneo.Application.Matches.UpdateMatch;
using Torneo.Application.Players.CreatePlayer;
using Torneo.Application.Players.DeletePlayer;
using Torneo.Application.Players.PatchPlayer;
using Torneo.Application.Players.UpdatePlayer;
using Torneo.Application.Teams.CreateTeam;
using Torneo.Application.Teams.DeleteTeam;
using Torneo.Application.Teams.PatchTeam;
using Torneo.Application.Teams.UpdateTeam;
using Torneo.Infrastructure.Events;
using Torneo.Infrastructure.Persistence;
using Torneo.Infrastructure.Persistence.Repositories;
using Torneo.Infrastructure.Read;

namespace Torneo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Torneo");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Falta la cadena de conexión Torneo.");
        }

        services.AddDbContext<TorneoDbContext>(options => options.UseSqlServer(connectionString));
        services.TryAddScoped<IRequestTrace, EmptyRequestTrace>();
        services.AddScoped<DomainEventLog>();
        services.AddScoped<DomainEventDispatcher>();
        services.AddScoped<IIdempotencyStore, IdempotencyStore>();
        services.AddScoped<IApplicationTransaction, EfApplicationTransaction>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<CreateTeamHandler>();
        services.AddScoped<UpdateTeamHandler>();
        services.AddScoped<PatchTeamHandler>();
        services.AddScoped<DeleteTeamHandler>();
        services.AddScoped<CreatePlayerHandler>();
        services.AddScoped<UpdatePlayerHandler>();
        services.AddScoped<PatchPlayerHandler>();
        services.AddScoped<DeletePlayerHandler>();
        services.AddScoped<CreateMatchHandler>();
        services.AddScoped<UpdateMatchHandler>();
        services.AddScoped<DeleteMatchHandler>();
        services.AddScoped<RegisterMatchResultHandler>();
        services.AddScoped<TournamentSeeder>();
        services.AddQueries(connectionString);
        services.AddHostedService<DatabaseInitializer>();
        services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("sqlserver");

        return services;
    }
}
