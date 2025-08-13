namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;

using Microsoft.EntityFrameworkCore;

public class GetTitlesQueryHandler(IApplicationDbContext context)
{
    public async Task<Result<GetTitlesResponse>> Handle(GetTitlesQuery query, CancellationToken ct)
    {
        var titles = await context.Titles.Select(x => new TitleResponse
            {
                Id = x.Id,
                OriginalName = x.Name.OriginalName,
                EnglishName = x.Name.EnglishName,
                LocalizedName = x.Name.LocalizedName,
                Description = x.Description,
                ChapterCount = x.Chapters.Count,
            })
            .ToArrayAsync(ct);

        return Result.Ok(new GetTitlesResponse(titles));
    }
}