using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Torneo.Domain.Common;

namespace Torneo.Api.Http;

public static class RateLimitSetup
{
    public static IServiceCollection AddTorneoRateLimit(this IServiceCollection services, IConfiguration configuration)
    {
        var loginPerMinute = configuration.GetValue("RateLimit:LoginPerMinute", 10);
        var apiPerMinute = configuration.GetValue("RateLimit:ApiPerMinute", 120);
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = WriteRejection;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                Partition(httpContext, loginPerMinute, apiPerMinute));
        });
        return services;
    }

    private static RateLimitPartition<string> Partition(HttpContext httpContext, int loginPerMinute, int apiPerMinute)
    {
        if (IsExempt(httpContext.Request.Path))
        {
            return RateLimitPartition.GetNoLimiter("exempt");
        }

        var key = ClientKey(httpContext);
        if (httpContext.Request.Path.StartsWithSegments("/api/auth/login"))
        {
            return RateLimitPartition.GetFixedWindowLimiter($"login:{key}", _ => Window(loginPerMinute));
        }

        return RateLimitPartition.GetFixedWindowLimiter($"api:{key}", _ => Window(apiPerMinute));
    }

    private static FixedWindowRateLimiterOptions Window(int permitLimit) => new()
    {
        PermitLimit = permitLimit,
        Window = TimeSpan.FromMinutes(1),
        QueueLimit = 0,
        AutoReplenishment = true
    };

    private static bool IsExempt(PathString path) =>
        path.StartsWithSegments("/health") || path.StartsWithSegments("/swagger");

    private static string ClientKey(HttpContext context)
    {
        var user = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? context.User.Identity?.Name;
        if (!string.IsNullOrWhiteSpace(user))
        {
            return user;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
    }

    private static async ValueTask WriteRejection(OnRejectedContext context, CancellationToken cancellationToken)
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            var seconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
            context.HttpContext.Response.Headers.RetryAfter = seconds.ToString();
        }

        var logger = context.HttpContext.RequestServices
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Torneo.Api.RateLimit");
        logger.LogWarning("Límite de peticiones alcanzado en {Path}", context.HttpContext.Request.Path);

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(
            new ApiError(ErrorCodes.RateLimit, "Demasiadas peticiones. Espera un momento y vuelve a intentar."),
            cancellationToken);
    }
}
