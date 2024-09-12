using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamAnswerConfigurationEFConfig : IEntityTypeConfiguration<ModelExamAnswerConfiguration>
{
    public void Configure(EntityTypeBuilder<ModelExamAnswerConfiguration> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.AnswerText).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.AnswerImageId).IsRequired(false);

        builder.HasOne(x => x.Question)
            .WithMany(x => x.ModelExamAnswers)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired();

        builder.HasOne(x => x.AnswerImage)
            .WithOne()
            .HasForeignKey<ModelExamAnswerConfiguration>(x => x.AnswerImageId);
    }
}
