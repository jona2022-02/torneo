namespace Torneo.Api.Swagger;

public static class SwaggerSetup
{
    public static void AddTorneoSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.OperationFilter<IdempotencyHeaderFilter>());
    }

    public static void UseTorneoSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mundialito");
            options.RoutePrefix = "swagger";
        });
    }
}
