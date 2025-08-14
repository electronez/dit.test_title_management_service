namespace DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteTitle;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class DeleteTitleCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(DeleteTitleCommand command, CancellationToken ct)
    {
        var title = await context.Titles.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.Id));
        }

        context.Titles.Remove(title);
        await context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}