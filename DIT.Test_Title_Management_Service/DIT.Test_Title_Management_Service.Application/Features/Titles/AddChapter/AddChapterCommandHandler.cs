namespace DIT.Test_Title_Management_Service.Application.Features.Titles.AddChapter;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class AddChapterCommandHandler(IApplicationDbContext context)
{
    public async Task<Result<AddChapterResponse>> Handle(AddChapterCommand command, CancellationToken ct)
    {
        var title = await context.Titles
            .SingleOrDefaultAsync(x => x.Id == command.TitleId, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.TitleId));
        }

        var chapter = title.AddChapter(command.Number, command.Name);
        context.Titles.Update(title);
        await context.SaveChangesAsync(ct);
        return Result.Ok(new AddChapterResponse(chapter.Id));
    }
}