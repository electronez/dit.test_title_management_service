namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.RevokeFromChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromChapter;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class RevokeFromChapterCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_ChapterAssignmentDeleted()
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

        var command = new RevokeFromChapterCommand(worker.Id, title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeFromChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).Single(x => x.Id == worker.Id);
        workerFromDb.Assignments.Should().HaveCount(0);
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

        var command = new RevokeFromChapterCommand(Guid.NewGuid(), title.Id, targetChapter.Id, WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeFromChapterCommandHandler>();
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

        var command = new RevokeFromChapterCommand(worker.Id, Guid.NewGuid(), Guid.NewGuid(), WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeFromChapterCommandHandler>();
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

        var command = new RevokeFromChapterCommand(worker.Id, title.Id, Guid.NewGuid(), WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeFromChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}