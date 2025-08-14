namespace DIT.Test_Title_Management_Service.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static IServiceProvider MigrateDatabase(this IServiceProvider services)
    {
        using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        return services;
    }
}