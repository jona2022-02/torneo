using Torneo.Api.Http;
using Torneo.Api.Idempotency;
using Torneo.Api.Observability;
using Torneo.Api.Swagger;
using Torneo.Application.Abstractions;
using Torneo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddTorneoSerilog();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRequestTrace, HttpRequestTrace>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ResultMappingFilter>();
    options.Conventions.Add(new ErrorResponsesConvention());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy => policy
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders(CorrelationKeys.Header));
});
builder.Services.AddTorneoSwagger();

var app = builder.Build();

app.UseTechnicalErrors();
app.UseMiddleware<CorrelationMiddleware>();
app.UseCors("frontend");
app.UseMiddleware<IdempotencyMiddleware>();
app.UseTorneoSwagger();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
