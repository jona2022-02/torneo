using Microsoft.EntityFrameworkCore;
using Torneo.Application.Abstractions;
using Torneo.Domain.Teams;

namespace Torneo.Infrastructure.Persistence.Repositories;

internal sealed class TeamRepository : ITeamRepository
{
    private readonly TorneoDbContext _context;

    public TeamRepository(TorneoDbContext context)
    {
        _context = context;
    }

    public Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Teams.FirstOrDefaultAsync(team => team.Id == id, cancellationToken);

    public Task<bool> NameExistsAsync(string name, Guid? excludingId, CancellationToken cancellationToken)
    {
        var normalized = name.Trim().ToLower();
        return _context.Teams.AnyAsync(
            team => team.Name.ToLower() == normalized && (!excludingId.HasValue || team.Id != excludingId.Value),
            cancellationToken);
    }

    public void Add(Team team) => _context.Teams.Add(team);

    public void Remove(Team team) => _context.Teams.Remove(team);
}
