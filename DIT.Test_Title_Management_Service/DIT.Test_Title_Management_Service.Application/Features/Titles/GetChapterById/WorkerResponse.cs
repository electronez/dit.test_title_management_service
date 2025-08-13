namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Информация о работнике
/// </summary>
public record WorkerResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public required string LastName { get; init; }

    /// <summary>
    /// Роли
    /// </summary>
    public required IEnumerable<WorkerRole> Roles { get; init; }
}