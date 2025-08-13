namespace DIT.Test_Title_Management_Service.Host.Extensions;

using DIT.Test_Title_Management_Service.Host.Infrastructure;
using FluentResults.Extensions.AspNetCore;

public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Применяет конфигурацию библиотеки <see cref="FluentResults"/>
    /// </summary>
    public static WebApplicationBuilder ApplyFluentResultsConfiguration(this WebApplicationBuilder builder)
    {
        AspNetCoreResult.Setup(settings => settings.DefaultProfile = new DitFluentResultsEndpointProfile());
        return builder;
    }
}