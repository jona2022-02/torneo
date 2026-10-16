using Torneo.Domain.Operators;

namespace Torneo.Application.Abstractions;

public interface IOperatorRepository
{
    Task<Operator?> FindByUsernameAsync(string username, CancellationToken cancellationToken);

    void Add(Operator op);
}
