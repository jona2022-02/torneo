using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public static class HttpResults
{
    public static ResultEnvelope Ok(Result result) =>
        new(result, StatusCodes.Status200OK, null, null);

    public static ResultEnvelope Ok<T>(Result<T> result) =>
        new(result, StatusCodes.Status200OK, result.IsSuccess ? result.Value : null, null);

    public static ResultEnvelope Created(Result<Guid> result, string? location) =>
        new(
            result,
            StatusCodes.Status201Created,
            result.IsSuccess ? new CreatedResource(result.Value) : null,
            location);

    public static ResultEnvelope NoContent(Result result) =>
        new(result, StatusCodes.Status204NoContent, null, null);
}
