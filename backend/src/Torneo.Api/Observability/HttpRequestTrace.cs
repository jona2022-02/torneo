using Torneo.Application.Abstractions;

namespace Torneo.Api.Observability;

public sealed class HttpRequestTrace : IRequestTrace
{
    private readonly IHttpContextAccessor _http;

    public HttpRequestTrace(IHttpContextAccessor http)
    {
        _http = http;
    }

    public string TraceId => _http.HttpContext?.TraceIdentifier ?? "sin-traza";

    public string? CorrelationId => _http.HttpContext?.Items[CorrelationKeys.Item] as string;
}
