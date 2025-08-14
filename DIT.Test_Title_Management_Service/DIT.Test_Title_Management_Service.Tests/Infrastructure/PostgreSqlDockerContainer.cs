namespace DIT.Test_Title_Management_Service.Tests.Infrastructure;

using DotNet.Testcontainers.Builders;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

public class PostgreSqlDockerContainer
{
    private bool isDbCleanerInitialized;
    private PostgreSqlContainer? container;
    private readonly DbCleaner dbCleaner = new();

    public void Configure(IConfiguration configuration)
    {
        configuration["ConnectionStrings:PostgreSQL"] = this.container?.GetConnectionString();
    }

    public async Task StartAsync()
    {
        if (this.container is not null)
            return;
        this.container = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("public")
            .WithEnvironment("PGDATA", "/pgdata")
            .WithTmpfsMount("/pgdata")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("psql -U postgres -c \"select 1\""))
            .Build();
        await this.container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (!this.isDbCleanerInitialized)
        {
            await dbCleaner.InitializeAsync(this.container?.GetConnectionString());
            this.isDbCleanerInitialized = true;
        }
        await dbCleaner.RespawnAsync();

        await dbCleaner.DisposeAsync();
        if (this.container != null)
            await this.container.DisposeAsync();
    }
}