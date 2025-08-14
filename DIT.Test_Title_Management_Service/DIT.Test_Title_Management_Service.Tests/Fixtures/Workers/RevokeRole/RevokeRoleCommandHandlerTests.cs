namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.RevokeRole;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeRole;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class RevokeRoleCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_RoleRevoked()
    {
        var worker = EntitySamples.CreateWorker();
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new RevokeRoleCommand(worker.Id, WorkerRole.Translator);

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeRoleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Single(x => x.Id == worker.Id);
        workerFromDb.Roles.Should().NotContain(WorkerRole.Translator);
    }

    [Fact]
    public async Task Handle_WorkerNotExists_Fail()
    {
        var command = new RevokeRoleCommand(Guid.NewGuid(), WorkerRole.Translator);
        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeRoleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainItemsAssignableTo<NotFoundError>();
    }

    [Fact]
    public async Task Handle_WorkerHaveAssignments_RoleRevokedAndAssignmentsDeleted()
    {
        var title = EntitySamples.CreateTitle();
        var chapter = title.Chapters.First();
        var worker = EntitySamples.CreateWorker();
        worker.AssignRole(WorkerRole.Editor);
        worker.AssignToChapter(chapter, WorkerRole.Editor);
        worker.AssignToTitle(title, WorkerRole.Translator);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Titles.Add(title);
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new RevokeRoleCommand(worker.Id, WorkerRole.Translator);

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<RevokeRoleCommandHandler>();
        var result = await handler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();
        using var scopeForAssertion = application.Services.CreateScope();
        dbContext = scopeForAssertion.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = dbContext.Workers.Single(x => x.Id == worker.Id);
        workerFromDb.Roles.Should().NotContain(WorkerRole.Translator);
        workerFromDb.Assignments.Should().NotContain(x => x.Role == WorkerRole.Translator);
        workerFromDb.Assignments.Should().Contain(x => x.Role == WorkerRole.Editor);
    }
}