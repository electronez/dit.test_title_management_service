namespace DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на создание работника
/// </summary>
public record CreateWorkerCommand
{
    /// <summary>
    /// Уникальное имя работника
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
    public required IEnumerable<WorkerRole> Roles { get; init; } = [];
}