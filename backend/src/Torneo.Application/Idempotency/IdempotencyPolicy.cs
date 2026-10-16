using Torneo.Domain.Common;

namespace Torneo.Application.Idempotency;

public static class IdempotencyPolicy
{
    public static Result<IdempotencyOutcome> Evaluate(string? key, string requestHash, StoredResponse? stored)
    {
        if (!Guid.TryParse(key, out var parsed) || parsed == Guid.Empty)
        {
            return Result<IdempotencyOutcome>.Failure(
                ErrorCodes.IdempotencyMissingKey,
                "El header Idempotency-Key es obligatorio y debe ser un GUID.");
        }

        var normalized = parsed.ToString("D");
        if (stored is null)
        {
            return Result<IdempotencyOutcome>.Success(IdempotencyOutcome.Proceed(normalized));
        }

        if (!string.Equals(stored.RequestHash, requestHash, StringComparison.Ordinal))
        {
            return Result<IdempotencyOutcome>.Failure(
                ErrorCodes.IdempotencyKeyReused,
                "La clave de idempotencia ya se usó con otro cuerpo.");
        }

        return Result<IdempotencyOutcome>.Success(
            IdempotencyOutcome.Stored(normalized, stored.StatusCode, stored.ResponseBody));
    }
}
