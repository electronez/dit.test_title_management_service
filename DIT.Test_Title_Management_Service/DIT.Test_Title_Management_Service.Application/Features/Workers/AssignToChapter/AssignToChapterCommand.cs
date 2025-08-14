namespace DIT.Test_Title_Management_Service.Application.Features.Workers.AssignToChapter;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на назначение работника на конкретную главу
/// </summary>
/// <param name="WorkerId">Идентификатор работника</param>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="ChapterId">Идентификатор главы</param>
/// <param name="Role">Роль</param>
public record AssignToChapterCommand(Guid WorkerId, Guid TitleId, Guid ChapterId, WorkerRole Role);