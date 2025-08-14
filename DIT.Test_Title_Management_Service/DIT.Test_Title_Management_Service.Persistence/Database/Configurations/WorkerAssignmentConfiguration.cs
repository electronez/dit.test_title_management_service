namespace DIT.Test_Title_Management_Service.Persistence.Database.Configurations;

using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WorkerAssignmentConfiguration : IEntityTypeConfiguration<WorkerAssignment>
{
    public void Configure(EntityTypeBuilder<WorkerAssignment> builder)
    {
        builder.ToTable("worker_assignments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Role).HasColumnName("role").IsRequired();

        builder.Property(x => x.WorkerId).HasColumnName("worker_id").IsRequired();
        builder.HasOne<Worker>()
            .WithMany(x => x.Assignments)
            .HasForeignKey(x => x.WorkerId);

        builder.Property(x => x.TitleId).HasColumnName("title_id").IsRequired();
        builder.HasOne<Title>()
            .WithMany()
            .HasForeignKey(x => x.TitleId);

        builder.Property(x => x.ChapterId).HasColumnName("chapter_id").IsRequired(false);
        builder.HasOne<Chapter>()
            .WithMany()
            .HasForeignKey(x => x.ChapterId);
    }
}