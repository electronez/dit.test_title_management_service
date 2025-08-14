namespace DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkerById;

/// <summary>
/// Запрос на получение работника по идентификатору
/// </summary>
/// <param name="Id">Идентификатор работника</param>
public record GetWorkerByIdQuery(Guid Id);