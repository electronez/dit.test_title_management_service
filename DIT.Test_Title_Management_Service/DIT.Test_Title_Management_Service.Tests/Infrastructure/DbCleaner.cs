namespace DIT.Test_Title_Management_Service.Tests.Infrastructure;

using Npgsql;
using Respawn;

public class DbCleaner
{
    private readonly RespawnerOptions _options = new()
    {
        DbAdapter = DbAdapter.Postgres,
        TablesToIgnore = ["__EFMigrationsHistory"],
    };

    private Respawner? respawner;
    private NpgsqlConnection npgsqlConnection = null!;

    /// <summary> Схема должна существовать (т.е. нужно запускать после миграций) </summary>
    public async Task InitializeAsync(string? connectionString)
    {
        this.npgsqlConnection = new NpgsqlConnection(connectionString);
        await this.npgsqlConnection.OpenAsync();
        this.respawner = await Respawner.CreateAsync(this.npgsqlConnection, _options);
    }

    public async Task DisposeAsync()
    {
        await this.npgsqlConnection.DisposeAsync();
    }

    public async Task RespawnAsync()
    {
        if (this.respawner != null)
            await this.respawner.ResetAsync(this.npgsqlConnection);
    }
}