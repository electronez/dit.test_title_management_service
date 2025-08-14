namespace DIT.Test_Title_Management_Service.Application.Features.Workers.UpdateWorker;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class UpdateWorkerCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(UpdateWorkerCommand command, CancellationToken ct)
    {
        var worker = await context.Workers.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (worker is null)
        {
            return Result.Fail(new NotFoundError<Worker, Guid>(command.Id));
        }

        var profile = new Profile(command.FirstName, command.LastName);
        worker.UpdateProfile(profile);
        context.Workers.Update(worker);
        await context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}