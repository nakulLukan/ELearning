using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Video;

public class QuizConfigurationEFConfig : IEntityTypeConfiguration<Learning.Domain.Quiz.QuizConfiguration>
{
    public void Configure(EntityTypeBuilder<Domain.Quiz.QuizConfiguration> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.HasMany(x => x.Questions)
            .WithOne(x => x.QuizConfiguration)
            .HasForeignKey(x => x.QuizConfigurationId);
    }
}
