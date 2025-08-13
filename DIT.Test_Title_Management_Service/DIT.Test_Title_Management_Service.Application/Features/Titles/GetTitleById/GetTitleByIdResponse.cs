namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;

/// <summary>
/// Ответ на запрос на получение тайтла по идентификатору
/// </summary>
public record GetTitleByIdResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Оригинальное название
    /// </summary>
    public required string OriginalName { get; init; }

    /// <summary>
    /// Английское название
    /// </summary>
    public string? EnglishName { get; init; }

    /// <summary>
    /// Локализованное название
    /// </summary>
    public string? LocalizedName { get; init; }

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Главы
    /// </summary>
    public required IEnumerable<ChapterResponse> Chapters { get; init; }

    /// <summary>
    /// Работники, работающие над данным тайтлом (без учёта тех, кто работает только над конкретными главами)
    /// </summary>
    public required IEnumerable<WorkerResponse> Workers { get; init; }
}