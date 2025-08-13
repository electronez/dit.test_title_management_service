namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;

using DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;
using DIT.Test_Title_Management_Service.Application.Results;
using DIT.Test_Title_Management_Service.Domain.Titles;
using Microsoft.EntityFrameworkCore;

public class GetTitleByIdRequestHandler(IApplicationDbContext context)
{
    public async Task<Result<GetTitleByIdResponse>> Handle(GetTitleByIdRequest request, CancellationToken ct)
    {
        var title = await context.Titles
            .Include(x => x.Chapters)
            .SingleOrDefaultAsync(x => x.Id == request.Id, ct);
        if (title is null)
        {
            return Result.Fail(new NotFoundError<Title, Guid>(request.Id));
        }

        var workers = await this.GetWorkersAsync(title.Id, ct);
        var result = new GetTitleByIdResponse
        {
            Id = title.Id,
            OriginalName = title.Name.OriginalName,
            EnglishName = title.Name.EnglishName,
            LocalizedName = title.Name.LocalizedName,
            Description = title.Description,
            Chapters = title.Chapters.Select(chapter => new ChapterResponse(chapter.Id, chapter.Number, chapter.Name)),
            Workers = workers,
        };

        return Result.Ok(result);
    }

    /// <summary>
    /// Получает список работников, работающих непосредственно над тайтлом (без тех, кто работает индивидуально над главой)
    /// </summary>
    private Task<WorkerResponse[]> GetWorkersAsync(Guid id, CancellationToken ct)
    {
        return context.Workers
            .Where(x => x.Assignments.Any(assignment => assignment.TitleId == id && assignment.ChapterId == null))
            .Select(x => new WorkerResponse
            {
                Id = x.Id,
                FirstName = x.Profile.FirstName,
                LastName = x.Profile.LastName,
                Roles = x.Assignments
                    .Where(assignment => assignment.TitleId == id && assignment.ChapterId == null)
                    .Select(assignment => assignment.Role).Distinct(),
            }).ToArrayAsync(ct);
    }
}