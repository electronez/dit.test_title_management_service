namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.AssignToTitle;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToTitle;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class AssignToTitleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_Correct_WorkerAssignedToTitle()
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

        var command = new AssignToTitleCommand(worker.Id, title.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).Single(x => x.Id == worker.Id);
        workerFromDb.Assignments.Should().HaveCount(1);
        workerFromDb.Assignments.Should().Contain(x =>
            x.TitleId == title.Id && x.ChapterId == null && x.Role == WorkerRole.Translator);
    }

    [Fact]
    public async Task Handle_WorkerNotExists_Fail()
    {
        var title = EntitySamples.CreateTitle();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToTitleCommand(Guid.NewGuid(), title.Id, WorkerRole.Typer);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
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

        var command = new AssignToTitleCommand(worker.Id, Guid.NewGuid(), WorkerRole.Typer);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_WorkerNotHaveRole_Fail()
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

        var command = new AssignToTitleCommand(worker.Id, title.Id, WorkerRole.Editor);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == "Работник не может быть назначен на эту роль");
    }

    [Fact]
    public async Task Handle_WorkerAlreadyAssignedToTitleOnThisRole_Fail()
    {
        var title = EntitySamples.CreateTitle();
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

        var command = new AssignToTitleCommand(worker.Id, title.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == "Работник уже назначен на эту роль");
    }

    [Fact]
    public async Task Handle_WorkerAssignedToChapterOnThisRole_AssignmentToChapterDeletedAndTitleAssigned()
    {
        var title = EntitySamples.CreateTitle();
        var chapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignToChapter(chapter, WorkerRole.Translator);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignToTitleCommand(worker.Id, title.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignToTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).Single(x => x.Id == worker.Id);
        workerFromDb.Assignments.Should().HaveCount(1);
        workerFromDb.Assignments.Should().Contain(x =>
            x.TitleId == title.Id && x.ChapterId == null && x.Role == WorkerRole.Translator);
    }
}