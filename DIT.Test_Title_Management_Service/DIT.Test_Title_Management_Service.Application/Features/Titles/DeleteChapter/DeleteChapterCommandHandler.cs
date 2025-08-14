namespace DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteChapter;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class DeleteChapterCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(DeleteChapterCommand command, CancellationToken ct)
    {
        var title = await context.Titles
            .Include(x => x.Chapters)
            .SingleOrDefaultAsync(x => x.Id == command.TitleId, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(command.TitleId));
        }

        var chapter = title.Chapters.SingleOrDefault(x => x.Id == command.ChapterId);
        if (chapter is not null)
        {
            title.RemoveChapter(chapter);
            context.Titles.Update(title);
            await context.SaveChangesAsync(ct);
        }

        return Result.Ok();
    }
}