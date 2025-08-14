namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.AssignToChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToChapter;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class AssignToChapterCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_Correct_WorkerAssignedToChapter()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).Single(x => x.Id == worker.Id);
        workerFromDb.Assignments.Should().HaveCount(1);
        workerFromDb.Assignments.Should().Contain(x =>
            x.TitleId == title.Id && x.ChapterId == targetChapter.Id && x.Role == WorkerRole.Translator);
    }

    [Fact]
    public async Task Handle_WorkerNotExists_Fail()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(Guid.NewGuid(), title.Id, targetChapter.Id, WorkerRole.Typer);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_TitleNotExists_Fail()
    {
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, Guid.NewGuid(), Guid.NewGuid(), WorkerRole.Typer);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_ChapterNotExists_Fail()
    {
        var title = EntitySamples.CreateTitle();
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, Guid.NewGuid(), WorkerRole.Typer);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_WorkerNotHaveRole_Fail()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Editor);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == "Работник не может быть назначен на эту роль");
    }

    [Fact]
    public async Task Handle_WorkerAlreadyAssignedToChapterOnThisRole_Fail()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignToChapter(targetChapter, WorkerRole.Translator);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == "Работник уже назначен на эту роль");
    }

    /// <summary>
    /// Работник не может быть назначен на главу на эту роль, если он назначен на эту же роль в тайтле
    /// </summary>
    [Fact]
    public async Task Handle_WorkerAlreadyAssignedToTitleOnThisRole_Fail()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignToTitle(title, WorkerRole.Translator);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == "Работник уже назначен на эту роль");
    }

    /// <summary>
    /// Если работник назначен на тайтл на одну роль, то он может быть назначен на главу на другую роль
    /// </summary>
    [Fact]
    public async Task Handle_WorkerAssignedToTitleOnOtherRole_WorkerAssignedToChapter()
    {
        var title = EntitySamples.CreateTitle();
        var targetChapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignRole(WorkerRole.Editor);
        worker.AssignToTitle(title, WorkerRole.Editor);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).Single(x => x.Id == worker.Id);
        workerFromDb.Assignments.Should().HaveCount(2);
        workerFromDb.Assignments.Should().Contain(x =>
            x.TitleId == title.Id && x.ChapterId == targetChapter.Id && x.Role == WorkerRole.Translator);
    }
}