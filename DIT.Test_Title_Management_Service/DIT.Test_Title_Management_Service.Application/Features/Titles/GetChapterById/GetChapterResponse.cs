namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;

/// <summary>
/// Ответ на запрос на получение главы тайтла по идентификатору
/// </summary>
public record GetChapterResponse
{
    /// <summary>
    /// Идентификатор главы
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Номер
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Работники, работающие над данной главой в тайтле
    /// </summary>
    public required IEnumerable<WorkerResponse> Workers { get; init; }
}