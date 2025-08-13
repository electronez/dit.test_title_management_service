namespace DIT.Test_Title_Management_Service.Application.Features.Titles.AddChapter;

using System.Text.Json.Serialization;

/// <summary>
/// Команда на добавление главы в тайтл
/// </summary>
public record AddChapterCommand
{
    /// <summary>
    /// Идентификатор тайтла
    /// </summary>
    [JsonIgnore]
    public Guid TitleId { get; init; }

    /// <summary>
    /// Номер
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Наименование
    /// </summary>
    public required string? Name { get; init; }
}