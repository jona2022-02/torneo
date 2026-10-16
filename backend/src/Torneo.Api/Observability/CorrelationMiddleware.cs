using System.Diagnostics;
using Serilog.Context;

namespace Torneo.Api.Observability;

public sealed class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationMiddleware> _logger;

    public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Resolve(context.Request.Headers[CorrelationKeys.Header].FirstOrDefault());
        context.Items[CorrelationKeys.Item] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationKeys.Header] = correlationId;
            return Task.CompletedTask;
        });

        using (LogContext.PushProperty(CorrelationKeys.Item, correlationId))
        {
            var watch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                watch.Stop();
                _logger.LogInformation(
                    "Request completado {Method} {Path} {StatusCode} {ElapsedMs} {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path.Value,
                    context.Response.StatusCode,
                    watch.ElapsedMilliseconds,
                    correlationId);
            }
        }
    }

    private static string Resolve(string? incoming)
    {
        if (string.IsNullOrWhiteSpace(incoming))
        {
            return Guid.NewGuid().ToString("D");
        }

        var value = incoming.Trim();
        if (value.Length > 64 || value.Contains('\r') || value.Contains('\n'))
        {
            return Guid.NewGuid().ToString("D");
        }

        return value;
    }
}
