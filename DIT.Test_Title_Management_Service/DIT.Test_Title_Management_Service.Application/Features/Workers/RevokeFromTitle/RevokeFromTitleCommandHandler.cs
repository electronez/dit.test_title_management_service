namespace DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromTitle;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class RevokeFromTitleCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(RevokeFromTitleCommand command, CancellationToken ct)
    {
        var worker = await context.Workers.SingleOrDefaultAsync(x => x.Id == command.WorkerId, ct);
        if (worker is null)
        {
            return Result.Fail(new NotFoundError<Worker, Guid>(command.WorkerId));
        }

        var title = await context.Titles.SingleOrDefaultAsync(x => x.Id == command.TitleId, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.TitleId));
        }

        worker.RevokeFromTitle(title, command.Role);
        context.Workers.Update(worker);
        await context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}