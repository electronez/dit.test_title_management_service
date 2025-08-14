namespace DIT.Test_Title_Management_Service.Host.Controllers;

using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignRole;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToChapter;
using DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToTitle;
using DIT.Test_Title_Management_Service.Application.Features.Workers.CreateWorker;
using DIT.Test_Title_Management_Service.Application.Features.Workers.DeleteWorker;
using DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkerById;
using DIT.Test_Title_Management_Service.Application.Features.Workers.GetWorkers;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromChapter;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromTitle;
using DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeRole;
using DIT.Test_Title_Management_Service.Application.Features.Workers.UpdateWorker;
using DIT.Test_Title_Management_Service.Domain.Enums;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер для работы с работниками
/// </summary>
[ApiController]
[Route("workers")]
public class WorkerController : ControllerBase
{
    /// <summary>
    /// Получить всех работников
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetWorkersResponse>> GetAll(
        [FromServices] GetWorkersQueryHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new GetWorkersQuery(), ct)).ToActionResult();

    /// <summary>
    /// Получить работника по идентификатору
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    [HttpGet("{workerId}")]
    public async Task<ActionResult<GetWorkerByIdResponse>> GetById(
        Guid workerId,
        [FromServices] GetWorkerByIdQueryHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new GetWorkerByIdQuery(workerId), ct)).ToActionResult();

    /// <summary>
    /// Создать работника
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromBody] CreateWorkerCommand command,
        [FromServices] CreateWorkerCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command, ct)).ToActionResult();

    /// <summary>
    /// Обновить профиль работника
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    [HttpPut("{workerId}/profile")]
    public async Task<ActionResult> UpdateProfile(
        Guid workerId,
        [FromBody] UpdateWorkerCommand command,
        [FromServices] UpdateWorkerCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command with { Id = workerId }, ct)).ToActionResult();

    /// <summary>
    /// Удалить работника
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    [HttpDelete("{workerId}")]
    public async Task<ActionResult> Delete(
        Guid workerId,
        [FromServices] DeleteWorkerCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new DeleteWorkerCommand(workerId), ct)).ToActionResult();

    /// <summary>
    /// Назначить роль на работника
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="role">Роль</param>
    [HttpPost("{workerId}/roles/{role}")]
    public async Task<ActionResult> AssignRole(
        Guid workerId,
        WorkerRole role,
        [FromServices] AssignRoleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new AssignRoleCommand(workerId, role), ct)).ToActionResult();

    /// <summary>
    /// Отозвать роль работника
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="role">Роль</param>
    [HttpDelete("{workerId}/roles/{role}")]
    public async Task<ActionResult> RevokeRole(
        Guid workerId,
        WorkerRole role,
        [FromServices] RevokeRoleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new RevokeRoleCommand(workerId, role), ct)).ToActionResult();

    /// <summary>
    /// Назначить сотрудника на тайтл на указанную роль
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="role">Роль</param>
    [HttpPost("{workerId}/titles/{titleId}/roles/{role}")]
    public async Task<ActionResult> AssignToTitle(
        Guid workerId,
        Guid titleId,
        WorkerRole role,
        [FromServices] AssignToTitleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new AssignToTitleCommand(workerId, titleId, role), ct)).ToActionResult();

    /// <summary>
    /// Отозвать сотрудника с тайтла с указанной роли
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="role">Роль</param>
    [HttpDelete("{workerId}/titles/{titleId}/roles/{role}")]
    public async Task<ActionResult> RevokeFromTitle(
        Guid workerId,
        Guid titleId,
        WorkerRole role,
        [FromServices] RevokeFromTitleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new RevokeFromTitleCommand(workerId, titleId, role), ct)).ToActionResult();

    /// <summary>
    /// Назначить сотрудника на главу тайтла на указанную роль
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="chapterId">Идентификатор главы</param>
    /// <param name="role">Роль</param>
    [HttpPost("{workerId}/titles/{titleId}/chapters/{chapterId}/roles/{role}")]
    public async Task<ActionResult> AssignToChapter(
        Guid workerId,
        Guid titleId,
        Guid chapterId,
        WorkerRole role,
        [FromServices] AssignToChapterCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new AssignToChapterCommand(workerId, titleId, chapterId, role), ct)).ToActionResult();

    /// <summary>
    /// Отзывает сотрудника с главы тайтла с указанной роли
    /// </summary>
    /// <param name="workerId">Идентификатор работника</param>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="chapterId">Идентификатор главы</param>
    /// <param name="role">Роль</param>
    [HttpDelete("{workerId}/titles/{titleId}/chapters/{chapterId}/roles/{role}")]
    public async Task<ActionResult> RevokeFromChapter(
        Guid workerId,
        Guid titleId,
        Guid chapterId,
        WorkerRole role,
        [FromServices] RevokeFromChapterCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new RevokeFromChapterCommand(workerId, titleId, chapterId, role), ct)).ToActionResult();
}