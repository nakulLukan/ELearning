using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamOrderEFConfig : IEntityTypeConfiguration<ModelExamOrder>
{
    public void Configure(EntityTypeBuilder<ModelExamOrder> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired(true).HasMaxLength(36);
        builder.Property(x => x.OrderedCompletedOn).IsRequired(false);
        builder.Property(x => x.RzrpayOrderId)
            .HasMaxLength(40)
            .IsRequired(false);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired(true);

        builder.HasOne(x => x.ModelExamPackage)
            .WithMany(x => x.PurchaseOrders)
            .HasForeignKey(x => x.ModelExamPackageId)
            .IsRequired(false);
    }
}
