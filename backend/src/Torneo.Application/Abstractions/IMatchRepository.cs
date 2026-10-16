using Torneo.Domain.Matches;

namespace Torneo.Application.Abstractions;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> TeamHasMatchesAsync(Guid teamId, CancellationToken cancellationToken);

    Task<bool> PlayerHasGoalsAsync(Guid playerId, CancellationToken cancellationToken);

    Task<bool> DuplicateExistsAsync(
        Guid homeTeamId,
        Guid awayTeamId,
        DateTimeOffset kickoff,
        Guid? excludingId,
        CancellationToken cancellationToken);

    void Add(Match match);

    void Remove(Match match);
}
