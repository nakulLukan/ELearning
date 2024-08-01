using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Video;

public class QuizQuestionEFConfig : IEntityTypeConfiguration<Learning.Domain.Quiz.QuizQuestion>
{
    public void Configure(EntityTypeBuilder<Domain.Quiz.QuizQuestion> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Question).IsRequired().HasMaxLength(255);
        builder.Property(x => x.QuestionImageRelativePath).IsRequired(false).HasMaxLength(255);

        builder.HasMany(x => x.Answers)
            .WithOne(x => x.QuizQuestion)
            .HasForeignKey(x => x.QuizQuestionId);
    }
}
