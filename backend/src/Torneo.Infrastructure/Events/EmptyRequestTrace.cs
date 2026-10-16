using Torneo.Application.Abstractions;

namespace Torneo.Infrastructure.Events;

internal sealed class EmptyRequestTrace : IRequestTrace
{
    public string TraceId => "sin-traza";

    public string? CorrelationId => null;
}
