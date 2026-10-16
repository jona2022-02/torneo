using Microsoft.AspNetCore.Diagnostics;

namespace Torneo.Api.Observability;

public static class TechnicalErrorHandler
{
    public static void UseTechnicalErrors(this WebApplication app)
    {
        app.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Torneo.Api");
                logger.LogError(
                    exception,
                    "Error no controlado {TraceId} {CorrelationId}",
                    context.TraceIdentifier,
                    context.Items[CorrelationKeys.Item]);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    code = "error",
                    message = "Ocurrió un error interno.",
                    traceId = context.TraceIdentifier
                });
            });
        });
    }
}
