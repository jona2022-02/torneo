using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Torneo.Application.Abstractions;
using Torneo.Domain.Operators;

namespace Torneo.Infrastructure.Persistence;

internal sealed class OperatorSeeder
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwords;
    private readonly IOperatorRepository _operators;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OperatorSeeder> _logger;

    public OperatorSeeder(
        IConfiguration configuration,
        IPasswordHasher passwords,
        IOperatorRepository operators,
        IUnitOfWork unitOfWork,
        ILogger<OperatorSeeder> logger)
    {
        _configuration = configuration;
        _passwords = passwords;
        _operators = operators;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var username = _configuration["Auth:Operator:Username"];
        var password = _configuration["Auth:Operator:Password"];
        var displayName = _configuration["Auth:Operator:DisplayName"];
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(displayName))
        {
            return;
        }

        var created = Operator.Create(username, _passwords.Hash(password), displayName);
        if (created.IsFailure)
        {
            return;
        }

        _operators.Add(created.Value);
        await _unitOfWork.CommitAsync(cancellationToken);
        _logger.LogInformation("Operador inicial listo.");
    }
}
