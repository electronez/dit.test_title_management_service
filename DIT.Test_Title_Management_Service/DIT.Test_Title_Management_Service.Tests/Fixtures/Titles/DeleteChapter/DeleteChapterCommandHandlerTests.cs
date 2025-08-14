namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.DeleteChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteChapter;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class DeleteChapterCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_ChapterDeleted()
    {
        var title = EntitySamples.CreateTitle();
        var chapterForDelete = title.Chapters.First();

        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new DeleteChapterCommand(title.Id, chapterForDelete.Id);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<DeleteChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = dbContext.Titles.First(t => t.Id == title.Id);
        titleFromDb.Should().NotBeNull();
        titleFromDb.Chapters.Should().NotContain(x => x.Id == chapterForDelete.Id);
    }

    [Fact]
    public async Task Handle_TitleNotExists_Fail()
    {
        var command = new DeleteChapterCommand(Guid.NewGuid(), Guid.NewGuid());
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<DeleteChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_ChapterHasAssignment_ChapterAndAssignmentDeleted()
    {
        var title = EntitySamples.CreateTitle();
        var chapterForDelete = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignToChapter(chapterForDelete, WorkerRole.Translator);

        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new DeleteChapterCommand(title.Id, chapterForDelete.Id);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<DeleteChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = dbContext.Titles.First(t => t.Id == title.Id);
        titleFromDb.Should().NotBeNull();
        titleFromDb.Chapters.Should().NotContain(x => x.Id == chapterForDelete.Id);
        var workerFromDb = dbContext.Workers.Include(x => x.Assignments).First(w => w.Id == worker.Id);
        workerFromDb.Assignments.Should().NotContain(x => x.TitleId == title.Id && x.ChapterId == chapterForDelete.Id);
    }
}