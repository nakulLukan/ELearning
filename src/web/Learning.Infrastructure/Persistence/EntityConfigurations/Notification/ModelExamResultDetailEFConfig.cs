using Learning.Domain.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Learning.Infrastructure.Persistence.EntityConfigurations.Notification;

public class ModelExamResultDetailEFConfig : IEntityTypeConfiguration<ModelExamResultDetail>
{
    public void Configure(EntityTypeBuilder<ModelExamResultDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.SelectedAnswerId).IsRequired(false);

        builder.HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey(x => x.QuestionId);

        builder.HasOne(x => x.SelectedAnswer)
            .WithMany()
            .HasForeignKey(x => x.SelectedAnswerId);

    }
}
