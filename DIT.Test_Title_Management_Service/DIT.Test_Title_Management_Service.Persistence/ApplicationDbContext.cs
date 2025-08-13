namespace DIT.Test_Title_Management_Service.Persistence;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using DIT.Test_Title_Management_Service.Persistence.Database.Configurations;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    /// <inheritdoc cref="IApplicationDbContext"/>
    public DbSet<Title> Titles { get; set; }

    /// <inheritdoc cref="IApplicationDbContext"/>
    public DbSet<Worker> Workers { get; set; }

    /// <inheritdoc cref="IApplicationDbContext"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorkerConfiguration).Assembly);
    }
}