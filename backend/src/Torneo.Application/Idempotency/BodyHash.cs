using System.Security.Cryptography;
using System.Text;

namespace Torneo.Application.Idempotency;

public static class BodyHash
{
    public static string Sha256(string body)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(body));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
