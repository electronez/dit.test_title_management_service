namespace DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkerById;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Ответ на запрос на получение работника по идентификатору
/// </summary>
public record GetWorkerByIdResponse
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// Уникальное имя
    /// </summary>
    public required string Username { get; init; }

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