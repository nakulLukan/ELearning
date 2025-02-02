using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamPurchaseHistoryEFCOnfig : IEntityTypeConfiguration<ModelExamPurchaseHistory>
{
    public void Configure(EntityTypeBuilder<ModelExamPurchaseHistory> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasOne(x => x.ModelExamOrder)
            .WithOne(x => x.ModelExamPurchaseHistory)
            .HasForeignKey<ModelExamPurchaseHistory>(x => x.OrderId)
            .IsRequired(true);
    }
}
