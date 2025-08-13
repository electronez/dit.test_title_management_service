namespace DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateTitle;

using System.Text.Json.Serialization;

/// <summary>
/// Команда на обновление тайтла
/// </summary>
public record UpdateTitleCommand
{
    /// <summary>
    /// Идентификатор тайтла
    /// </summary>
    [JsonIgnore]
    public Guid Id { get; init; }

    /// <summary>
    /// Оригинальное название
    /// </summary>
    public required string OriginalName { get; init; }

    /// <summary>
    /// Английское название
    /// </summary>
    public required string? EnglishName { get; init; }

    /// <summary>
    /// Локализованное название
    /// </summary>
    public required string? LocalizedName { get; init; }

    /// <summary>
    /// Описание
    /// </summary>
    public required string? Description { get; init; }
}