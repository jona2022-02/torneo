using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Torneo.Api.Http;

public sealed class ResultMappingFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult { Value: ResultEnvelope envelope })
        {
            context.Result = ResultMapper.Map(envelope);
            LogConflict(context);
        }

        await next();
    }

    private static void LogConflict(ResultExecutingContext context)
    {
        if (context.Result is not ObjectResult { StatusCode: StatusCodes.Status409Conflict, Value: ApiError error })
        {
            return;
        }

        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ResultMappingFilter>>();
        logger.LogWarning("Conflicto de negocio {Code} {Message}", error.Code, error.Message);
    }
}
