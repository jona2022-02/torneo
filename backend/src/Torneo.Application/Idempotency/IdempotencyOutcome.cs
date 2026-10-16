namespace Torneo.Application.Idempotency;

public sealed record IdempotencyOutcome(string Key, bool Replay, int StatusCode, string ResponseBody)
{
    public static IdempotencyOutcome Proceed(string key) => new(key, false, 0, "");

    public static IdempotencyOutcome Stored(string key, int statusCode, string responseBody) =>
        new(key, true, statusCode, responseBody);
}
