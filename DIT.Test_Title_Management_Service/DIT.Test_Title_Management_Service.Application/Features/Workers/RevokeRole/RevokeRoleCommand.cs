namespace DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeRole;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на отзыв роли работника
/// </summary>
/// <param name="Id">Идентификатор работника</param>
/// <param name="Role">Роль</param>
public record RevokeRoleCommand(Guid Id, WorkerRole Role);