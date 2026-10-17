using Microsoft.EntityFrameworkCore;
using Torneo.Application.Abstractions;
using Torneo.Domain.Players;

namespace Torneo.Infrastructure.Persistence.Repositories;

internal sealed class PlayerRepository : IPlayerRepository
{
    private readonly TorneoDbContext _context;

    public PlayerRepository(TorneoDbContext context)
    {
        _context = context;
    }

    public Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Players.FirstOrDefaultAsync(player => player.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Player>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        if (ids.Count == 0)
        {
            return [];
        }

        return await _context.Players.Where(player => ids.Contains(player.Id)).ToListAsync(cancellationToken);
    }

    public Task<bool> ShirtNumberExistsAsync(Guid teamId, int shirtNumber, Guid? excludingId, CancellationToken cancellationToken) =>
        _context.Players.AnyAsync(
            player => player.TeamId == teamId
                && player.ShirtNumber == shirtNumber
                && (!excludingId.HasValue || player.Id != excludingId.Value),
            cancellationToken);

    public Task<bool> GoalkeeperExistsAsync(Guid teamId, Guid? excludingId, CancellationToken cancellationToken) =>
        _context.Players.AnyAsync(
            player => player.TeamId == teamId
                && player.Role == LineupRole.Portero
                && (!excludingId.HasValue || player.Id != excludingId.Value),
            cancellationToken);

    public Task<bool> AnyInTeamAsync(Guid teamId, CancellationToken cancellationToken) =>
        _context.Players.AnyAsync(player => player.TeamId == teamId, cancellationToken);

    public void Add(Player player) => _context.Players.Add(player);

    public void Remove(Player player) => _context.Players.Remove(player);
}
