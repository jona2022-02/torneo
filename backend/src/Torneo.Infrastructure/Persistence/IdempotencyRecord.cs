namespace Torneo.Infrastructure.Persistence;

internal sealed class IdempotencyRecord
{
    public string Key { get; set; } = "";

    public string RequestHash { get; set; } = "";

    public int StatusCode { get; set; }

    public string ResponseBody { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }
}
