using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Torneo.Infrastructure.Persistence;

internal sealed class TorneoDbContextFactory : IDesignTimeDbContextFactory<TorneoDbContext>
{
    public TorneoDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<TorneoDbContext>()
            .UseSqlServer("Server=localhost,1433;Database=Torneo;User Id=sa;Password=TorneoDev_2026!;TrustServerCertificate=True;Encrypt=True")
            .Options;

        return new TorneoDbContext(options);
    }
}
