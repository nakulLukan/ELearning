using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamResultEFConfig : IEntityTypeConfiguration<ModelExamResult>
{
    public void Configure(EntityTypeBuilder<ModelExamResult> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired(true).HasMaxLength(36);

        builder.HasOne(x => x.ExamConfig)
            .WithMany()
            .HasForeignKey(x => x.ExamConfigId)
            .IsRequired(false);

        builder.HasMany(x => x.ModelExamResultDetails)
            .WithOne()
            .HasForeignKey(x => x.ModelExamResultId);
    }
}
