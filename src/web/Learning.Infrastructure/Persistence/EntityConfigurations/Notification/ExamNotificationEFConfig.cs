using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ExamNotificationEFConfig : IEntityTypeConfiguration<Learning.Domain.Notification.ExamNotification>
{
    public void Configure(EntityTypeBuilder<Domain.Notification.ExamNotification> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Descrption).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.NotificationTitle).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ImageRelativePath).IsRequired();
    }
}
