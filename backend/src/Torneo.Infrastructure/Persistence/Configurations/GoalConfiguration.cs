using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torneo.Domain.Goals;
using Torneo.Domain.Players;
using Torneo.Domain.Teams;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class GoalConfiguration : IEntityTypeConfiguration<Goal>
{
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        builder.ToTable("Goals");
        builder.HasKey(goal => goal.Id);
        builder.Property(goal => goal.Minute).IsRequired();
        builder.HasOne<Player>().WithMany().HasForeignKey(goal => goal.PlayerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Team>().WithMany().HasForeignKey(goal => goal.TeamId).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(goal => goal.DomainEvents);
    }
}
