namespace DIT.Test_Title_Management_Service.Application.Features;

using DIT.Test_Title_Management_Service.Application.Features.Titles.AddChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.CreateTitle;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteTitle;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateTitle;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignRole;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToChapter;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToTitle;
using DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;
using DIT.Test_Title_Management_Service.Application.Features.Workers.DeleteWorker;
using DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkerById;
using DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkers;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromChapter;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromTitle;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeRole;
using DIT.Test_Title_Management_Service.Application.Features.Workers.UpdateWorker;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Методы расширения для регистрации сервисов приложения
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет сервисы приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов.</returns>
    public static IServiceCollection AddApplicationFeatures(this IServiceCollection services)
        => services
            .AddScoped<CreateWorkerCommandHandler>()
            .AddScoped<DeleteWorkerCommandHandler>()
            .AddScoped<UpdateWorkerCommandHandler>()
            .AddScoped<GetWorkersQueryHandler>()
            .AddScoped<GetWorkerByIdQueryHandler>()
            .AddScoped<AssignRoleCommandHandler>()
            .AddScoped<RevokeRoleCommandHandler>()
            .AddScoped<AssignToTitleCommandHandler>()
            .AddScoped<AssignToChapterCommandHandler>()
            .AddScoped<RevokeFromTitleCommandHandler>()
            .AddScoped<RevokeFromChapterCommandHandler>()
            .AddScoped<CreateTitleCommandHandler>()
            .AddScoped<DeleteTitleCommandHandler>()
            .AddScoped<UpdateTitleCommandHandler>()
            .AddScoped<GetTitlesQueryHandler>()
            .AddScoped<GetTitleByIdRequestHandler>()
            .AddScoped<AddChapterCommandHandler>()
            .AddScoped<DeleteChapterCommandHandler>()
            .AddScoped<GetChapterByIdRequestHandler>()
            .AddScoped<UpdateChapterCommandHandler>();
}