using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Torneo.Api.Observability;

public static class SerilogSetup
{
    public static void AddTorneoSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, configuration) => configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(new JsonFormatter()));
    }
}
