using Learning.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Core;

public class ChapterEFConfig : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);

        builder.HasOne(x => x.Subject)
            .WithMany(x => x.Chapters)
            .HasForeignKey(x => x.SubjectId);

        builder.HasMany(x => x.Lessons)
            .WithOne(x => x.Chapter)
            .HasForeignKey(x => x.ChapterId);
    }
}
