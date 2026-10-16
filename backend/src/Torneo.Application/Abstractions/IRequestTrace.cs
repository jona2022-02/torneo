namespace Torneo.Application.Abstractions;

public interface IRequestTrace
{
    string TraceId { get; }

    string? CorrelationId { get; }
}
