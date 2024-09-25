using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamPackageEFConfig : IEntityTypeConfiguration<ModelExamPackage>
{
    public void Configure(EntityTypeBuilder<ModelExamPackage> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasMany(x => x.ModelExamConfigs)
            .WithOne(x => x.ModelExamPackage)
            .HasForeignKey(x => x.ModelExamPackageId);

        builder.HasOne(x => x.ExamNotification)
            .WithOne()
            .HasForeignKey<ModelExamPackage>(x => x.ExamNotificationId)
            .IsRequired(false);
    }
}
