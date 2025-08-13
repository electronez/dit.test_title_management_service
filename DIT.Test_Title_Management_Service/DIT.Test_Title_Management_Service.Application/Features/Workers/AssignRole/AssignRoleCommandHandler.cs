namespace DIT.Test_Title_Management_Service.Application.Features.Workers.AssignRole;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class AssignRoleCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(AssignRoleCommand command, CancellationToken ct)
    {
        var worker = await context.Workers.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (worker is null)
        {
            return Result.Fail(new NotFoundError<Worker, Guid>(command.Id));
        }

        worker.AssignRole(command.Role);
        context.Workers.Update(worker);
        await context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}