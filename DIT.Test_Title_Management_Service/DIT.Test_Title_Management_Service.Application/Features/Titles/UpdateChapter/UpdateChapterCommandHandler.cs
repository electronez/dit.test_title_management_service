namespace DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateChapter;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class UpdateChapterCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(UpdateChapterCommand command, CancellationToken ct)
    {
        var title = await context.Titles
            .Include(x => x.Chapters)
            .SingleOrDefaultAsync(x => x.Id == command.TitleId, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.TitleId));
        }

        var chapter = title.Chapters.SingleOrDefault(x => x.Id == command.ChapterId);
        if (chapter is null)
        {
            return Result.Fail(new NotFoundError<Chapter, Guid>(command.ChapterId));
        }

        var result = title.UpdateChapter(chapter, command.Number, command.Name);
        if (result.IsSuccess)
        {
            context.Titles.Update(title);
            await context.SaveChangesAsync(ct);
        }

        return result;
    }
}