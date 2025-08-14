namespace DIT.Test_Title_Management_Service.Tests.Fixtures.Workers.CreateWorker;

using DIT.Test_Title_Management_Service.Application;
using DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;
using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Domain.Workers;
using DIT.Test_Title_Management_Service.Tests.Infrastructure;
using DIT.Test_Title_Management_Service.Tests.Samples;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

[Collection(WebApplicationCollection.CollectionName)]
public class CreateWorkerCommandHandlerTests(TestWebApplication application)
{
    [Fact]
    public async Task Handle_WorkerCreated()
    {
        var command = new CreateWorkerCommand
        {
            Username = "New username",
            FirstName = "New firstname",
            LastName = "New lastname",
            Roles = [WorkerRole.Editor],
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<CreateWorkerCommandHandler>();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();

        var applicationDbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        var workerFromDb = applicationDbContext.Workers.SingleOrDefault(x => x.Id == result.Value.Id);
        workerFromDb.Should().NotBeNull();
        workerFromDb.Username.Should().Be(command.Username);
        workerFromDb.Profile.FirstName.Should().Be(command.FirstName);
        workerFromDb.Profile.LastName.Should().Be(command.LastName);
        workerFromDb.Roles.Should().BeEquivalentTo(command.Roles);
    }

    [Fact]
    public async Task Handle_UsernameAlreadyExists_Fail()
    {
        var username = "existingUsername";
        var worker = Worker.Create(username, new Profile("firstname", "lastname"), []);
        IApplicationDbContext? dbContext;
        using (var scopeForDbContext = application.Services.CreateScope())
        {
            dbContext = scopeForDbContext.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            dbContext.Workers.Add(worker);
            await dbContext.SaveChangesAsync(default);
        }

        var command = new CreateWorkerCommand
        {
            Username = username,
            FirstName = "New firstname",
            LastName = "New lastname",
            Roles = [WorkerRole.Editor],
        };

        using var scope = application.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<CreateWorkerCommandHandler>();
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().Contain(x => x.Message == $"Уникальное имя пользователя {command.Username} уже существует");
    }
}