using Learning.Domain.Master;
using Learning.Domain.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Video;

public class QuizQuestionAnswerEFConfig : IEntityTypeConfiguration<Learning.Domain.Quiz.QuizQuestionAnswer>
{
    public void Configure(EntityTypeBuilder<Domain.Quiz.QuizQuestionAnswer> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.AnswerText).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.AnswerImageId).IsRequired(false);

        builder.HasOne(x => x.AnswerImage)
            .WithOne()
            .HasForeignKey<QuizQuestionAnswer>(x => x.AnswerImageId)
            .HasPrincipalKey<Attachment>(x => x.Id);
    }
}
