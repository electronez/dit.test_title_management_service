namespace DIT.Test_Title_Management_Service.Application.Features.Workers.DeleteWorker;

/// <summary>
/// Команда на удаление работника
/// </summary>
/// <param name="Id">Идентификатор работника</param>
public record DeleteWorkerCommand(Guid Id);