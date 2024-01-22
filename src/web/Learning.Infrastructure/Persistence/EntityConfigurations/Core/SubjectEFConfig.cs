using Learning.Domain.Core;
using Learning.Domain.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Core;

public class SubjectEFConfig : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(20);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.ThumbnailRelativePath).IsRequired(false);

        builder.HasMany(x => x.Chapters)
            .WithOne(x => x.Subject)
            .HasForeignKey(x => x.SubjectId);

        builder.HasOne(x => x.SubjectGroupLookup)
            .WithMany()
            .HasForeignKey(x => x.SubjectGroupLookupId);

        builder.HasOne(x => x.SubscriptionDetail)
            .WithOne(x => x.Subject)
            .HasForeignKey<SubjectSubscriptionDetail>(x => x.SubjectId);
    }
}
