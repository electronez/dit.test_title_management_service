namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.CreateTitle;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.CreateTitle;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class CreateTitleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_TitleCreated()
    {
        var command = new CreateTitleCommand
        {
            OriginalName = "FakeOriginalName",
            EnglishName = "FakeEnglishName",
            LocalizedName = "FakeLocalizedName",
            Description = "FakeDescription",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<CreateTitleCommandHandler>();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var titleFromDb = applicationDbContext.Titles.SingleOrDefault(x => x.Id == result.Value.Id);
        titleFromDb.Should().NotBeNull();
        titleFromDb.Name.OriginalName.Should().Be(command.OriginalName);
        titleFromDb.Name.EnglishName.Should().Be(command.EnglishName);
        titleFromDb.Name.LocalizedName.Should().Be(command.LocalizedName);
        titleFromDb.Description.Should().Be(command.Description);
        titleFromDb.Chapters.Should().BeEmpty();
    }
}