using Microsoft.Extensions.Logging;
using Torneo.Application.Abstractions;
using Torneo.Domain.Common;
using Torneo.Domain.Matches;
using Torneo.Domain.Teams;

namespace Torneo.Infrastructure.Events;

internal sealed class DomainEventLog
{
    private readonly ILogger<DomainEventLog> _logger;
    private readonly IRequestTrace _trace;

    public DomainEventLog(ILogger<DomainEventLog> logger, IRequestTrace trace)
    {
        _logger = logger;
        _trace = trace;
    }

    public void Write(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case TeamCreated created:
                _logger.LogInformation(
                    "Equipo creado {TeamId} {TeamName} {TraceId} {CorrelationId}",
                    created.TeamId,
                    created.Name,
                    _trace.TraceId,
                    _trace.CorrelationId);
                break;
            case MatchResultRegistered registered:
                _logger.LogInformation(
                    "Resultado registrado {MatchId} {HomeScore} {AwayScore} {TraceId} {CorrelationId}",
                    registered.MatchId,
                    registered.HomeScore,
                    registered.AwayScore,
                    _trace.TraceId,
                    _trace.CorrelationId);
                break;
        }
    }
}
