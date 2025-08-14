namespace DIT.Test_Title_Management_Service.Application.Features.Titles.CreateTitle;

/// <summary>
/// Команда на создание нового тайтла
/// </summary>
public record CreateTitleCommand
{
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
}