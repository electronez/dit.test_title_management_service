namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Titles.DeleteTitle;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteTitle;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class DeleteTitleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_TitleDeleted()
    {
        var title = EntitySamples.CreateTitle();

        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new DeleteTitleCommand(title.Id);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<DeleteTitleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        dbContext.Titles.Any(x => x.Id == title.Id).Should().BeFalse();
    }
}