namespace Torneo.Application.Idempotency;

public sealed record StoredResponse(string Key, string RequestHash, int StatusCode, string ResponseBody);
