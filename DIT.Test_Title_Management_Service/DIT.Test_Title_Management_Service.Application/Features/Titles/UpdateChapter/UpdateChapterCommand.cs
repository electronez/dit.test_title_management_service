namespace DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateChapter;

using System.Text.Json.Serialization;

/// <summary>
/// Команда на обновление главы тайтла
/// </summary>
public record UpdateChapterCommand
{
    /// <summary>
    /// Идентификатор тайтла
    /// </summary>
    [JsonIgnore]
    public Guid TitleId { get; init; }

    /// <summary>
    /// Идентификатор главы
    /// </summary>
    [JsonIgnore]
    public Guid ChapterId { get; init; }

    /// <summary>
    /// Номер
    /// </summary>
    public int Number { get; init; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; init; }
}