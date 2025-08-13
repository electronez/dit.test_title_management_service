namespace DIT.Test_Title_Management_Service.Host.Controllers;

using DIT.Test_Title_Management_Service.Application.Features.Titles.AddChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.CreateTitle;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteTitle;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetChapterById;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;
using DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitles;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateChapter;
using DIT.Test_Title_Management_Service.Application.Features.Titles.UpdateTitle;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Контроллер для работы с тайтлами
/// </summary>
[ApiController]
[Route("titles")]
public class TitleController : ControllerBase
{
    /// <summary>
    /// Получить все тайтлы
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetTitlesResponse>> GetAll(
        [FromServices] GetTitlesQueryHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new GetTitlesQuery(), ct)).ToActionResult();

    /// <summary>
    /// Получить тайтл по идентификатору
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    [HttpGet("{titleId}")]
    public async Task<ActionResult<GetTitleByIdResponse>> GetById(
        Guid titleId,
        [FromServices] GetTitleByIdRequestHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new GetTitleByIdRequest(titleId), ct)).ToActionResult();

    /// <summary>
    /// Создать тайтл
    /// </summary>
    /// <param name="command"></param>
    /// <param name="handler"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CreateTitleResponse>> Create(
        [FromBody] CreateTitleCommand command,
        [FromServices] CreateTitleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command, ct)).ToActionResult();

    /// <summary>
    /// Обновить информацию о тайтле
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    [HttpPut("{titleId}")]
    public async Task<ActionResult> Update(
        Guid titleId,
        [FromBody] UpdateTitleCommand command,
        [FromServices] UpdateTitleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command with { Id = titleId }, ct)).ToActionResult();

    /// <summary>
    /// Удалить тайтл
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    [HttpDelete("{titleId}")]
    public async Task<ActionResult> Delete(
        Guid titleId,
        [FromServices] DeleteTitleCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new DeleteTitleCommand(titleId), ct)).ToActionResult();

    /// <summary>
    /// Добавить главу в тайтл
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    [HttpPost("{titleId}/chapters")]
    public async Task<ActionResult<AddChapterResponse>> AddChapter(
        Guid titleId,
        [FromBody] AddChapterCommand command,
        [FromServices] AddChapterCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command with { TitleId = titleId }, ct)).ToActionResult();

    /// <summary>
    /// Получить информацию о главе тайтла
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="chapterId">Идентификатор главы тайтла</param>
    [HttpGet("{titleId}/chapters/{chapterId}")]
    public async Task<ActionResult<GetChapterByIdRequest>> GetChapter(
        Guid titleId,
        Guid chapterId,
        [FromServices] GetChapterByIdRequestHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new GetChapterByIdRequest(titleId, chapterId), ct)).ToActionResult();

    /// <summary>
    /// Обновляет информацию о главе
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="chapterId">Идентификатор главы</param>
    [HttpPut("{titleId}/chapters/{chapterId}")]
    public async Task<ActionResult> UpdateChapter(
        Guid titleId,
        Guid chapterId,
        [FromBody] UpdateChapterCommand command,
        [FromServices] UpdateChapterCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(command with { TitleId = titleId, ChapterId = chapterId }, ct)).ToActionResult();

    /// <summary>
    /// Удалить главу тайтла
    /// </summary>
    /// <param name="titleId">Идентификатор тайтла</param>
    /// <param name="chapterId">Идентификатор главы тайтла</param>
    [HttpDelete("{titleId}/chapters/{chapterId}")]
    public async Task<ActionResult> DeleteChapter(
        Guid titleId,
        Guid chapterId,
        [FromServices] DeleteChapterCommandHandler handler,
        CancellationToken ct)
        => (await handler.Handle(new DeleteChapterCommand(titleId, chapterId), ct)).ToActionResult();
}