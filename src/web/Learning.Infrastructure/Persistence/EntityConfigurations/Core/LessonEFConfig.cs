using Learning.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Core;

public class LessonEFConfig : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(10);
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(50);

        builder.HasOne(x => x.Video)
            .WithOne()
            .HasForeignKey<Lesson>(x => x.VideoId)
            .HasPrincipalKey<Domain.Content.Video>(x => x.Id);

        builder.HasOne(x => x.Chapter)
            .WithMany(x=>x.Lessons)
            .HasForeignKey(x => x.ChapterId);
    }
}
