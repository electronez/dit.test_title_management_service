namespace DIT.Test_Title_Management_Service.Persistence.Database.Configurations;

using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("chapters");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Name).HasColumnName("name").IsRequired(false);
        builder.Property(x => x.TitleId).HasColumnName("title_id").IsRequired();
        builder.Property(x => x.Number).HasColumnName("number").IsRequired();

        builder.HasMany<WorkerAssignment>()
            .WithOne()
            .HasForeignKey(x => x.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}