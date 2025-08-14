namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.UpdateChapter;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateTitle;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class UpdateTitleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_TitleExists_TitleUpdated()
    {
        var title = EntitySamples.CreateTitle();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new UpdateTitleCommand
        {
            Id = title.Id,
            OriginalName = "New OriginalName",
            EnglishName = "New EnglishName",
            LocalizedName = "New LocalizedName",
            Description = "New Description",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = dbContext.Titles.SingleOrDefault(x => x.Id == title.Id);
        titleFromDb.Should().NotBeNull();
        titleFromDb.Name.OriginalName.Should().Be(command.OriginalName);
        titleFromDb.Name.EnglishName.Should().Be(command.EnglishName);
        titleFromDb.Name.LocalizedName.Should().Be(command.LocalizedName);
        titleFromDb.Description.Should().Be(command.Description);
    }

    [Fact]
    public async Task Handle_TitleNotExists_Fail()
    {
        var command = new UpdateTitleCommand
        {
            Id = Guid.NewGuid(),
            OriginalName = "New OriginalName",
            EnglishName = "New EnglishName",
            LocalizedName = "New LocalizedName",
            Description = "New Description",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}