using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Torneo.Infrastructure.Persistence.Configurations;

internal sealed class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
    {
        builder.ToTable("IdempotencyRecords");
        builder.HasKey(record => record.Key);
        builder.Property(record => record.Key).HasMaxLength(36);
        builder.Property(record => record.RequestHash).HasMaxLength(64).IsRequired();
        builder.Property(record => record.ResponseBody).IsRequired();
        builder.Property(record => record.CreatedAt).IsRequired();
    }
}
