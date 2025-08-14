namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.DeleteWorker;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.DeleteWorker;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class DeleteWorkerCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_WorkerDeleted()
    {
        var worker = EntitySamples.CreateWorker();

        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new DeleteWorkerCommand(worker.Id);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<DeleteWorkerCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        dbContext.Workers.Any(x => x.Id == worker.Id).Should().BeFalse();
    }
}