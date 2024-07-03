using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ExamNotificationEFConfig : IEntityTypeConfiguration<Learning.Domain.Notification.ExamNotification>
{
    public void Configure(EntityTypeBuilder<Domain.Notification.ExamNotification> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.NotificationTitle).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ImageRelativePath).IsRequired();
        builder.Property(x => x.GovtLink).IsRequired();
        builder.Property(x => x.ImportantPoints).IsRequired(false).HasMaxLength(1000);

        builder.HasOne(x => x.Video)
            .WithOne()
            .HasForeignKey<ExamNotification>(x => x.VideoId);

        builder.HasOne(x => x.PdfFile)
            .WithOne()
            .HasForeignKey<ExamNotification>(x => x.PdfFileId);
    }
}
