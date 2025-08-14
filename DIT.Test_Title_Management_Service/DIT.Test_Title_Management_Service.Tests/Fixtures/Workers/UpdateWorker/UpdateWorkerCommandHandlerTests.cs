namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.UpdateWorker;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.UpdateWorker;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class UpdateWorkerCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_WorkerUpdated()
    {
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new UpdateWorkerCommand
        {
            Id = worker.Id,
            FirstName = "NewFirstName",
            LastName = "NewLastName",
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateWorkerCommandHandler>();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = applicationDbContext.Workers.SingleOrDefault(x => x.Id == worker.Id);
        workerFromDb.Should().NotBeNull();
        workerFromDb.Profile.FirstName.Should().Be(command.FirstName);
        workerFromDb.Profile.LastName.Should().Be(command.LastName);
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

        var command = new UpdateWorkerCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "NewFirstName",
            LastName = "NewLastName",
        };
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<UpdateWorkerCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}