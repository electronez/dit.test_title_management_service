namespace DIT.Test_Title_Management_Service.Persistence.Database.Configurations;

using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TitleConfiguration : IEntityTypeConfiguration<Title>
{
    public void Configure(EntityTypeBuilder<Title> builder)
    {
        builder.ToTable("titles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Description).HasColumnName("description").IsRequired(false);

        builder.OwnsOne(
                x => x.Name,
                navigationBuilder =>
                {
                    navigationBuilder.Property(x => x.OriginalName).HasColumnName("original_name").IsRequired();
                    navigationBuilder.Property(x => x.EnglishName).HasColumnName("english_name").IsRequired(false);
                    navigationBuilder.Property(x => x.LocalizedName).HasColumnName("localized_name").IsRequired(false);
                })
            .Navigation(x => x.Name).IsRequired();

        builder.HasMany(x => x.Chapters)
            .WithOne(x => x.Title)
            .HasForeignKey(x => x.TitleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Chapters).AutoInclude();
    }
}