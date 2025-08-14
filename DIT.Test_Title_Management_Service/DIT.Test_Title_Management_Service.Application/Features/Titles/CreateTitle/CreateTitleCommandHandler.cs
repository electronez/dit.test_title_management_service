namespace DIT.Test_Title_Management_Service.Application.Features.Titles.CreateTitle;

using DIT.Test_Title_Management_Service.Domain.Titles;

public class CreateTitleCommandHandler(IApplicationDbContext context)
{
    public async Task<Result<CreateTitleResponse>> Handle(CreateTitleCommand command, CancellationToken ct)
    {
        var titleName = new TitleName(command.OriginalName, command.EnglishName, command.LocalizedName);
        var title = Title.Create(titleName, command.Description);
        context.Titles.Add(title);
        await context.SaveChangesAsync(ct);
        return Result.Ok(new CreateTitleResponse(title.Id));
    }
}