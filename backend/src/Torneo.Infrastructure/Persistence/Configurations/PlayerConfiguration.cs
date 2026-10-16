using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torneo.Domain.Players;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");
        builder.HasKey(player => player.Id);
        builder.Property(player => player.Name).HasMaxLength(Player.MaxNameLength).IsRequired();
        builder.HasIndex(player => new { player.TeamId, player.ShirtNumber }).IsUnique();
        builder.HasOne<Torneo.Domain.Teams.Team>().WithMany().HasForeignKey(player => player.TeamId).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(player => player.DomainEvents);
    }
}
