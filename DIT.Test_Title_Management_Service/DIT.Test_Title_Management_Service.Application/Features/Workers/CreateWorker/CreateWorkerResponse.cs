namespace DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;

/// <summary>
/// Ответ на команду на создание работника
/// </summary>
/// <param name="Id">Идентификатор созданного работника</param>
public record CreateWorkerResponse(Guid Id);