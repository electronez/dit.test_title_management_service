namespace DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;

using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class CreateWorkerCommandHandler(IApplicationDbContext context)
{
    public async Task<Result<CreateWorkerResponse>> Handle(CreateWorkerCommand command, CancellationToken ct)
    {
        var isUsernameExists = await context.Workers.AnyAsync(x => x.Username == command.Username, ct);
        if (isUsernameExists)
        {
            return Result.Fail<CreateWorkerResponse>($"Уникальное имя пользователя {command.Username} уже существует");
        }

        var profile = new Profile(command.FirstName, command.LastName);
        var worker = Worker.Create(command.Username, profile, command.Roles);
        context.Workers.Add(worker);
        await context.SaveChangesAsync(ct);
        return new CreateWorkerResponse(worker.Id);
    }
}