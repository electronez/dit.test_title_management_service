namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.UpdateChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateChapter;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class UpdateChapterCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_ChapterExists_ChapterUpdated()
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

        var command = new UpdateChapterCommand
        {
            TitleId = title.Id,
            ChapterId = targetChapter.Id,
            Number = 25,
            Name = "New Name",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = dbContext.Titles.Include(x => x.Chapters).SingleOrDefault(x => x.Id == title.Id);
        titleFromDb.Should().NotBeNull();
        var chapter = titleFromDb.Chapters.SingleOrDefault(x => x.Id == targetChapter.Id);
        chapter.Should().NotBeNull();
        chapter.Number.Should().Be(command.Number);
        chapter.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_TitleNotExists_Fail()
    {
        using var scope = application.Services.CreateScope();

        var command = new UpdateChapterCommand
        {
            TitleId = Guid.NewGuid(),
            ChapterId = Guid.NewGuid(),
            Number = 25,
            Name = "New Name",
        };

        var handler = scope.ServiceProvider.GetRequiredService<UpdateChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_ChapterNotExists_Fail()
    {
        var title = EntitySamples.CreateTitle();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }


        var command = new UpdateChapterCommand
        {
            TitleId = title.Id,
            ChapterId = Guid.NewGuid(),
            Number = 25,
            Name = "New Name",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}