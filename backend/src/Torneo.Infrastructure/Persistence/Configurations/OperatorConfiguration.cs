using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Torneo.Domain.Operators;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class OperatorConfiguration : IEntityTypeConfiguration<Operator>
{
    public void Configure(EntityTypeBuilder<Operator> builder)
    {
        builder.ToTable("Operators");
        builder.HasKey(op => op.Id);
        builder.Property(op => op.Username).HasMaxLength(Operator.MaxUsernameLength).IsRequired();
        builder.Property(op => op.PasswordHash).HasMaxLength(200).IsRequired();
        builder.Property(op => op.DisplayName).HasMaxLength(Operator.MaxDisplayNameLength).IsRequired();
        builder.HasIndex(op => op.Username).IsUnique();
        builder.Ignore(op => op.DomainEvents);
    }
}
