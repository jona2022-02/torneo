using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torneo.Domain.Teams;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(team => team.Id);
        builder.Property(team => team.Name).HasMaxLength(Team.MaxNameLength).IsRequired();
        builder.Property(team => team.CreatedAt).IsRequired();
        builder.HasIndex(team => team.Name).IsUnique();
        builder.Ignore(team => team.DomainEvents);
    }
}
