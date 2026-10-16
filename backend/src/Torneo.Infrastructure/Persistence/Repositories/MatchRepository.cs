using Microsoft.EntityFrameworkCore;
using Torneo.Application.Abstractions;
using Torneo.Domain.Matches;

namespace Torneo.Infrastructure.Persistence.Repositories;

internal sealed class MatchRepository : IMatchRepository
{
    private readonly TorneoDbContext _context;

    public MatchRepository(TorneoDbContext context)
    {
        _context = context;
    }

    public Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Matches.Include(match => match.Goals).FirstOrDefaultAsync(match => match.Id == id, cancellationToken);

    public Task<bool> TeamHasMatchesAsync(Guid teamId, CancellationToken cancellationToken) =>
        _context.Matches.AnyAsync(match => match.HomeTeamId == teamId || match.AwayTeamId == teamId, cancellationToken);

    public Task<bool> PlayerHasGoalsAsync(Guid playerId, CancellationToken cancellationToken) =>
        _context.Goals.AnyAsync(goal => goal.PlayerId == playerId, cancellationToken);

    public Task<bool> DuplicateExistsAsync(
        Guid homeTeamId,
        Guid awayTeamId,
        DateTimeOffset kickoff,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        var start = new DateTimeOffset(kickoff.Year, kickoff.Month, kickoff.Day, 0, 0, 0, kickoff.Offset);
        var end = start.AddDays(1);
        return _context.Matches.AnyAsync(
            match => match.HomeTeamId == homeTeamId
                && match.AwayTeamId == awayTeamId
                && match.Kickoff >= start
                && match.Kickoff < end
                && (!excludingId.HasValue || match.Id != excludingId.Value),
            cancellationToken);
    }

    public void Add(Match match) => _context.Matches.Add(match);

    public void Remove(Match match) => _context.Matches.Remove(match);
}
