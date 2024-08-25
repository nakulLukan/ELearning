using Learning.Domain.Subscription.Offer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Subscriptions;

public class CouponCodeEFConfig : IEntityTypeConfiguration<CouponCode>
{
    public void Configure(EntityTypeBuilder<CouponCode> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(20);
        builder.Property(x => x.ExpiresOn).IsRequired(false);

        builder.HasIndex(x => x.Code).IsUnique();
    }
}
