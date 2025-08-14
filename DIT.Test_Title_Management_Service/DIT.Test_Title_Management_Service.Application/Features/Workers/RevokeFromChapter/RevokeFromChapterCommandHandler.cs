namespace DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromChapter;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

public class RevokeFromChapterCommandHandler(IApplicationDbContext context)
{
    public async Task<Result> Handle(RevokeFromChapterCommand command, CancellationToken ct)
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

        var chapter = title.Chapters.SingleOrDefault(x => x.Id == command.ChapterId);
        if (chapter is null)
        {
            return Result.Fail(new NotFoundError<Chapter, Guid>(command.ChapterId));
        }

        worker.RevokeFromChapter(chapter, command.Role);
        context.Workers.Update(worker);
        await context.SaveChangesAsync(ct);

        return Result.Ok();
    }
}