using System.Text;
using Torneo.Application.Abstractions;
using Torneo.Application.Idempotency;
using Torneo.Domain.Common;

namespace Torneo.Api.Idempotency;

public sealed class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(RequestDelegate next, ILogger<IdempotencyMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var cancellationToken = context.RequestAborted;
        var body = await ReadBody(context.Request, cancellationToken);
        var store = context.RequestServices.GetRequiredService<IIdempotencyStore>();
        var key = context.Request.Headers["Idempotency-Key"].FirstOrDefault();
        var stored = key is null ? null : await store.FindAsync(NormalizeLookup(key), cancellationToken);
        var decision = IdempotencyPolicy.Evaluate(key, BodyHash.Sha256(body), stored);

        if (decision.IsFailure)
        {
            await WriteFailure(context, decision);
            return;
        }

        if (decision.Value.Replay)
        {
            await WriteReplay(context, decision.Value);
            return;
        }

        await CaptureAndStore(context, store, decision.Value.Key, BodyHash.Sha256(body), cancellationToken);
    }

    private async Task CaptureAndStore(
        HttpContext context,
        IIdempotencyStore store,
        string key,
        string hash,
        CancellationToken cancellationToken)
    {
        var original = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;
        try
        {
            await _next(context);
            buffer.Position = 0;
            var responseBody = await new StreamReader(buffer).ReadToEndAsync(cancellationToken);
            if (context.Response.StatusCode < StatusCodes.Status500InternalServerError)
            {
                await store.SaveAsync(new StoredResponse(key, hash, context.Response.StatusCode, responseBody), cancellationToken);
            }

            buffer.Position = 0;
            await buffer.CopyToAsync(original, cancellationToken);
        }
        finally
        {
            context.Response.Body = original;
        }
    }

    private Task WriteFailure(HttpContext context, Result decision)
    {
        if (decision.Error!.Code == ErrorCodes.IdempotencyKeyReused)
        {
            _logger.LogWarning("Clave de idempotencia reutilizada con otro cuerpo {Code}", decision.Error.Code);
        }

        context.Response.StatusCode = decision.Error.Code == ErrorCodes.IdempotencyKeyReused
            ? StatusCodes.Status409Conflict
            : StatusCodes.Status400BadRequest;
        return context.Response.WriteAsJsonAsync(new { code = decision.Error.Code, message = decision.Error.Message });
    }

    private static Task WriteReplay(HttpContext context, IdempotencyOutcome outcome)
    {
        context.Response.StatusCode = outcome.StatusCode;
        if (outcome.StatusCode == StatusCodes.Status204NoContent || outcome.ResponseBody.Length == 0)
        {
            return Task.CompletedTask;
        }

        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(outcome.ResponseBody);
    }

    private static string NormalizeLookup(string key) =>
        Guid.TryParse(key, out var parsed) ? parsed.ToString("D") : key;

    private static async Task<string> ReadBody(HttpRequest request, CancellationToken cancellationToken)
    {
        request.EnableBuffering();
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);
        request.Body.Position = 0;
        return body;
    }
}
