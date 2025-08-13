namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;

/// <summary>
/// Информация о главе тайтла
/// </summary>
/// <param name="Id">Идентификатор главы</param>
/// <param name="Number">Номер</param>
/// <param name="Name">Наименование</param>
public record ChapterResponse(Guid Id, int Number, string? Name);