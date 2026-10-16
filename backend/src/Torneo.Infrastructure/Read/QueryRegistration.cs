using Microsoft.Extensions.DependencyInjection;
using Torneo.Application.Abstractions;
using Torneo.Application.Matches.GetMatchById;
using Torneo.Application.Matches.GetMatches;
using Torneo.Application.Players.GetPlayerById;
using Torneo.Application.Players.GetPlayers;
using Torneo.Application.Scorers.GetScorers;
using Torneo.Application.Standings.GetStandings;
using Torneo.Application.Teams.GetTeamById;
using Torneo.Application.Teams.GetTeams;
using Torneo.Infrastructure.Read.Matches;
using Torneo.Infrastructure.Read.Players;
using Torneo.Infrastructure.Read.Scorers;
using Torneo.Infrastructure.Read.Standings;
using Torneo.Infrastructure.Read.Teams;

namespace Torneo.Infrastructure.Read;

internal static class QueryRegistration
{
    public static IServiceCollection AddQueries(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new SqlConnectionFactory(connectionString));
        services.AddScoped<ITeamQueries, TeamQueries>();
        services.AddScoped<IPlayerQueries, PlayerQueries>();
        services.AddScoped<IMatchQueries, MatchQueries>();
        services.AddScoped<IStandingsQueries, StandingsQueries>();
        services.AddScoped<IScorerQueries, ScorerQueries>();
        services.AddScoped<GetTeamsHandler>();
        services.AddScoped<GetTeamByIdHandler>();
        services.AddScoped<GetPlayersHandler>();
        services.AddScoped<GetPlayerByIdHandler>();
        services.AddScoped<GetMatchesHandler>();
        services.AddScoped<GetMatchByIdHandler>();
        services.AddScoped<GetStandingsHandler>();
        services.AddScoped<GetScorersHandler>();
        return services;
    }
}
