using Learning.Domain.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Subscriptions;

public class SubjectSubscriptionDetailEFConfig : IEntityTypeConfiguration<SubjectSubscriptionDetail>
{
    public void Configure(EntityTypeBuilder<SubjectSubscriptionDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.Subject)
            .WithOne(x => x.SubscriptionDetail)
            .HasForeignKey<SubjectSubscriptionDetail>(x => x.SubjectId);
    }
}
