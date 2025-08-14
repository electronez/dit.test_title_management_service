namespace DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateTitle;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class UpdateTitleCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(UpdateTitleCommand command, CancellationToken ct)
    {
        var title = await context.Titles.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.Id));
        }

        var titleName = new TitleName(command.OriginalName, command.EnglishName, command.LocalizedName);
        title.Update(titleName, command.Description);
        context.Titles.Update(title);
        await context.SaveChangesAsync(ct);
        return Result.Ok();
    }
}