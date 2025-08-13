namespace DIT.Test_Title_Management_Service.Application.Features.Workers.UpdateWorker;

using System.Text.Json.Serialization;

/// <summary>
/// Команда на обновление работника
/// </summary>
public record UpdateWorkerCommand
{
    /// <summary>
    /// Идентификатор работника
    /// </summary>
    [JsonIgnore]
    public Guid Id { get; init; }

    /// <summary>
    /// Имя
    /// </summary>
    public required string FirstName { get; init; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public required string LastName { get; init; }
}