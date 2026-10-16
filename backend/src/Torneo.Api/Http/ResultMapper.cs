using Microsoft.AspNetCore.Mvc;

namespace Torneo.Api.Http;

public static class ResultMapper
{
    public static IActionResult Map(ResultEnvelope envelope)
    {
        if (envelope.Result.IsFailure)
        {
            var error = envelope.Result.Error!;
            return new ObjectResult(new ApiError(error.Code, error.Message))
            {
                StatusCode = StatusCodeMap.For(error.Code)
            };
        }

        return envelope.SuccessStatus switch
        {
            StatusCodes.Status204NoContent => new NoContentResult(),
            StatusCodes.Status201Created => new CreatedResult(envelope.Location, envelope.Body),
            _ when envelope.Body is null => new OkResult(),
            _ => new OkObjectResult(envelope.Body)
        };
    }
}
