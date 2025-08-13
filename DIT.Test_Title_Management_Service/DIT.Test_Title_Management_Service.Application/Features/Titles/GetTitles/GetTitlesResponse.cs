namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;

/// <summary>
/// Ответ на запрос на получение списка тайтлов
/// </summary>
/// <param name="Items">Список тайтлов</param>
public record GetTitlesResponse(IEnumerable<TitleResponse> Items);