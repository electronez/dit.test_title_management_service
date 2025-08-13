namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;

/// <summary>
/// Информация о тайтле
/// </summary>
public record TitleResponse
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
    /// Количество глав
    /// </summary>
    public required int ChapterCount { get; init; }
}