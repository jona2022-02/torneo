using Torneo.Domain.Teams;

namespace Torneo.Application.Abstractions;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> NameExistsAsync(string name, Guid? excludingId, CancellationToken cancellationToken);

    void Add(Team team);

    void Remove(Team team);
}
