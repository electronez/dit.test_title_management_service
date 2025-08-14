namespace DIT.Test_Title_Management_Service.Tests.Infrastructure;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

public class TestWebApplication : IAsyncLifetime
{
    private readonly PostgreSqlDockerContainer postgreSqlDockerContainer = new();
    private WebApplicationFactory<Program> factory = default!;

    public IServiceProvider Services => this.factory.Services;

    public async Task InitializeAsync()
    {
        await this.postgreSqlDockerContainer.StartAsync();

        this.factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            var configurationProvider = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            this.postgreSqlDockerContainer.Configure(configurationProvider);
            builder.UseConfiguration(configurationProvider);
        });
    }

    public Task DisposeAsync()
        => this.postgreSqlDockerContainer.DisposeAsync();
}