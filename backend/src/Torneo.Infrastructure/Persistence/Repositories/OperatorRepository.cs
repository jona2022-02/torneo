using Microsoft.EntityFrameworkCore;
using Torneo.Application.Abstractions;
using Torneo.Domain.Operators;

namespace Torneo.Infrastructure.Persistence.Repositories;

internal sealed class OperatorRepository : IOperatorRepository
{
    private readonly TorneoDbContext _context;

    public OperatorRepository(TorneoDbContext context)
    {
        _context = context;
    }

    public Task<Operator?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var normalized = username.Trim().ToLower();
        return _context.Operators.FirstOrDefaultAsync(
            op => op.Username.ToLower() == normalized,
            cancellationToken);
    }

    public void Add(Operator op) => _context.Operators.Add(op);
}
