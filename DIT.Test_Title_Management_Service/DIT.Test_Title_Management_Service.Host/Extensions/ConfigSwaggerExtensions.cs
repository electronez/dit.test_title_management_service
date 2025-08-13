namespace DIT.Test_Title_Management_Service.Host.Extensions;

using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Методы расширения для регистрации Swagger
/// </summary>
public static class ConfigSwaggerExtensions
{
    /// <summary>
    /// Регистрирует сервисы Swagger
    /// </summary>
    public static IServiceCollection RegisterSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            ConfigureXmlComments(options);
            options.CustomSchemaIds(type => type.FullName);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerService(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseSwagger();
        appBuilder.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); });
        return appBuilder;
    }

    private static void ConfigureXmlComments(SwaggerGenOptions options)
    {
        var documentationFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
        if (documentationFiles.Length == 0)
        {
            return;
        }

        foreach (var xmlFilePath in documentationFiles)
        {
            if (File.Exists(xmlFilePath))
            {
                options.IncludeXmlComments(xmlFilePath);
            }
        }
    }
}