namespace DIT.Test_Title_Management_Service.Persistence.Database.Configurations;

using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.ToTable("workers");
        builder.HasKey(w => w.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever().IsRequired();
        builder.Property(x => x.Username).HasColumnName("username").IsRequired();
        builder.Property(x => x.Roles).HasColumnName("roles").IsRequired();

        builder.OwnsOne(
                x => x.Profile,
                navigationBuilder =>
                {
                    navigationBuilder.Property(x => x.FirstName).HasColumnName("first_name").IsRequired();
                    navigationBuilder.Property(x => x.LastName).HasColumnName("last_name").IsRequired();
                })
            .Navigation(x => x.Profile).IsRequired();

        builder.HasMany(x => x.Assignments)
            .WithOne()
            .HasForeignKey(x => x.WorkerId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(x => x.Assignments).AutoInclude();

        builder.HasIndex(x => x.Username).IsUnique();
    }
}