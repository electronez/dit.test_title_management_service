namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.AddChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.AddChapter;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class AddChapterCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_ChapterAdded()
    {
        var title = EntitySamples.CreateTitle();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AddChapterCommand
        {
            TitleId = title.Id,
            Number = 10,
            Name = $"Name{Guid.NewGuid()}",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AddChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = dbContext.Titles.Include(x => x.Chapters).SingleOrDefault(x => x.Id == title.Id);
        titleFromDb.Should().NotBeNull();
        titleFromDb.Chapters.Should().Contain(x => x.Number == command.Number && x.Name == command.Name);
    }

    [Fact]
    public async Task Handle_TitleNotExists_Fail()
    {
        using var scope = application.Services.CreateScope();

        var command = new AddChapterCommand
        {
            TitleId = Guid.NewGuid(),
            Number = 10,
            Name = $"Name{Guid.NewGuid()}",
        };

        var handler = scope.ServiceProvider.GetRequiredService<AddChapterCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}