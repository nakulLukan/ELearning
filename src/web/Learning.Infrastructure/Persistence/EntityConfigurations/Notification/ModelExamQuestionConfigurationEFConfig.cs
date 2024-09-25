using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamQuestionConfigurationEFConfig : IEntityTypeConfiguration<ModelExamQuestionConfiguration>
{
    public void Configure(EntityTypeBuilder<ModelExamQuestionConfiguration> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.QuestionText).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.QuestionImageId).IsRequired(false);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.QuestionImage)
            .WithOne()
            .HasForeignKey<ModelExamQuestionConfiguration>(x => x.QuestionImageId);

        builder.HasOne(x => x.ExamConfig)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.ExamConfigId)
            .IsRequired(false);

        builder.HasMany(x => x.ModelExamAnswers)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId);
    }
}
