using Microsoft.EntityFrameworkCore;
using Torneo.Domain.Goals;
using Torneo.Domain.Matches;
using Torneo.Domain.Players;
using Torneo.Domain.Teams;

namespace Torneo.Infrastructure.Persistence;

internal sealed class TorneoDbContext : DbContext
{
    public TorneoDbContext(DbContextOptions<TorneoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<Player> Players => Set<Player>();

    public DbSet<Match> Matches => Set<Match>();

    public DbSet<Goal> Goals => Set<Goal>();

    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Goal>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.State = EntityState.Added;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TorneoDbContext).Assembly);
    }
}
