namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.AssignRole;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignRole;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class AssignRoleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_WorkerExists_RoleAssigned()
    {
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new AssignRoleCommand(worker.Id, WorkerRole.Typer);

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignRoleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Single(x => x.Id == worker.Id);
        workerFromDb.Roles.Should().Contain(WorkerRole.Typer);
    }

    [Fact]
    public async Task Handle_WorkerNotExists_Fail()
    {
        var command = new AssignRoleCommand(Guid.NewGuid(), WorkerRole.Typer);

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<AssignRoleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }
}