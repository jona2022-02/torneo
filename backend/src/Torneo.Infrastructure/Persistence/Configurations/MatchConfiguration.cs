using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torneo.Domain.Goals;
using Torneo.Domain.Matches;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");
        builder.HasKey(match => match.Id);
        builder.Property(match => match.Status).HasConversion<int>();
        builder.HasMany(match => match.Goals)
            .WithOne()
            .HasForeignKey(goal => goal.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(match => match.Goals)
            .HasField("_goals")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.HasOne<Torneo.Domain.Teams.Team>().WithMany().HasForeignKey(match => match.HomeTeamId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Torneo.Domain.Teams.Team>().WithMany().HasForeignKey(match => match.AwayTeamId).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(match => match.DomainEvents);
    }
}
