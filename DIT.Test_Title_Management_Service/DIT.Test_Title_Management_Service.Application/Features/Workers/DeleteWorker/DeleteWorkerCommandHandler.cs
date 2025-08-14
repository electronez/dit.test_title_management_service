namespace DIT.Test_Title_Management_Service.Application.Features.Workers.DeleteWorker;

using Microsoft.EntityFrameworkCore;

public class DeleteWorkerCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(DeleteWorkerCommand command, CancellationToken ct)
    {
        var worker = await context.Workers.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (worker is not null)
        {
            context.Workers.Remove(worker);
            await context.SaveChangesAsync(ct);
        }

        return Result.Ok();
    }
}