namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;

/// <summary>
/// Запрос на получение главы тайтла по идентификатору
/// </summary>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="ChapterId">Идентификатор главы</param>
public record GetChapterByIdRequest(Guid TitleId, Guid ChapterId);