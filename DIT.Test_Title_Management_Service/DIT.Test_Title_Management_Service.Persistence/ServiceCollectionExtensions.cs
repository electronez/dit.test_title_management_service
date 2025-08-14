namespace DIT.Test_Title_Management_Service.Persistence;

using DIT.Test_Title_Management_Service.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string DefaultSectionName = "PostgreSQL";

    /// <summary>
    /// Добавляет сервисы хранения данных.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DefaultSectionName);
        services.AddDbContext<ApplicationDbContext>(builder =>
        {
            builder
                .UseNpgsql(
                    connectionString,
                    options => options.MigrationsHistoryTable("__EFMigrationsHistory"));
            builder.UseSnakeCaseNamingConvention();
            // builder.EnableSensitiveDataLogging();
        });

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}