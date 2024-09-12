using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamPurchaseHistoryEFCOnfig : IEntityTypeConfiguration<ModelExamPurchaseHistory>
{
    public void Configure(EntityTypeBuilder<ModelExamPurchaseHistory> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired(true).HasMaxLength(36);

        builder.HasOne(x => x.ExamConfig)
            .WithMany()
            .HasForeignKey(x => x.ExamConfigId)
            .IsRequired(false);
    }
}
