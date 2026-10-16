using Torneo.Domain.Common;

namespace Torneo.Application.Abstractions;

public interface IUnitOfWork
{
    Task<Result> CommitAsync(CancellationToken cancellationToken = default);
}
