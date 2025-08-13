namespace DIT.Test_Title_Management_Service.Application.Features.Workers.AssignRole;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на назначение роли работнику
/// </summary>
/// <param name="Id">Идентификатор работника</param>
/// <param name="Role">Роль</param>
public record AssignRoleCommand(Guid Id, WorkerRole Role);