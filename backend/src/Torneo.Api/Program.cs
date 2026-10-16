using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
var signingKey = builder.Configuration["Auth:SigningKey"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Auth:Issuer"],
            ValidAudience = builder.Configuration["Auth:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddTorneoSwagger();

var app = builder.Build();

app.UseTechnicalErrors();
app.UseMiddleware<CorrelationMiddleware>();
app.UseCors("frontend");
app.UseTorneoSwagger();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<IdempotencyMiddleware>();
app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();
app.Run();
