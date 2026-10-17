using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public static class StatusCodeMap
{
    public static int For(string code) => code switch
    {
        ErrorCodes.AuthInvalid => StatusCodes.Status401Unauthorized,
        ErrorCodes.RateLimit => StatusCodes.Status429TooManyRequests,
        ErrorCodes.NotFound => StatusCodes.Status404NotFound,
        ErrorCodes.Conflict
            or ErrorCodes.IdempotencyKeyReused
            or ErrorCodes.TeamNameDuplicate
            or ErrorCodes.TeamHasMatches
            or ErrorCodes.PlayerDuplicateNumber
            or ErrorCodes.PlayerGoalkeeperTaken
            or ErrorCodes.PlayerHasGoals => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status400BadRequest
    };
}
