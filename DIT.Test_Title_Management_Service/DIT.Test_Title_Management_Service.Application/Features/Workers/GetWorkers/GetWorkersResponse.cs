namespace DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkers;

/// <summary>
/// Ответ на запрос на получение всех работников
/// </summary>
/// <param name="Items">Список работников</param>
public record GetWorkersResponse(IEnumerable<WorkerResponse> Items);