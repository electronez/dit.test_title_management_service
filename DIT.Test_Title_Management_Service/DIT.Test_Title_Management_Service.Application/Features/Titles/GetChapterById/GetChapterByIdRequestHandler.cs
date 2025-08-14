namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;

using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class GetChapterByIdRequestHandler(IApplicationDbContext context)
{
    public async Task<Result<GetChapterResponse>> Handle(GetChapterByIdRequest request, CancellationToken ct)
    {
        var title = await context.Titles
            .Include(x => x.Chapters)
            .SingleOrDefaultAsync(x => x.Id == request.TitleId, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(request.TitleId));
        }

        var chapter = title.Chapters.SingleOrDefault(x => x.Id == request.ChapterId);
        if (chapter is null)
        {
            return Result.Fail(new NotFoundError<Chapter, Guid>(request.ChapterId));
        }

        var workers = await this.GetWorkersAsync(request.TitleId, request.ChapterId, ct);
        var result = new GetChapterResponse
        {
            Id = chapter.Id,
            Number = chapter.Number,
            Name = chapter.Name,
            Workers = workers,
        };

        return Result.Ok(result);
    }

    /// <summary>
    /// Получает список работников, работающих непосредственно над этой главой
    /// </summary>
    private Task<WorkerResponse[]> GetWorkersAsync(Guid titleId, Guid chapterId, CancellationToken ct)
    {
        return context.Workers
            .Where(x => x.Assignments.Any(assignment => assignment.TitleId == titleId && assignment.ChapterId == chapterId))
            .Select(x => new WorkerResponse
            {
                Id = x.Id,
                FirstName = x.Profile.FirstName,
                LastName = x.Profile.LastName,
                Roles = x.Assignments
                    .Where(assignment => assignment.TitleId == titleId && assignment.ChapterId == chapterId)
                    .Select(assignment => assignment.Role).Distinct(),
            }).ToArrayAsync(ct);
    }
}