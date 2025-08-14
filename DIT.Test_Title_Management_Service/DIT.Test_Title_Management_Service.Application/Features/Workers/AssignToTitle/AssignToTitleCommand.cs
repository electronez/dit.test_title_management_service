namespace DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToTitle;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на назначение работника на тайтл
/// </summary>
/// <param name="WorkerId">Идентификатор работника</param>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="Role">Роль</param>
public record AssignToTitleCommand(Guid WorkerId, Guid TitleId, WorkerRole Role);