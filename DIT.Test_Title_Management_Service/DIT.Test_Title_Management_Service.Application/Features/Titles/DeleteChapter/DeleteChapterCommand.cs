namespace DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteChapter;

/// <summary>
/// Команда на удаление главы тайтла
/// </summary>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="ChapterId">Идентификатор главы</param>
public record DeleteChapterCommand(Guid TitleId, Guid ChapterId);