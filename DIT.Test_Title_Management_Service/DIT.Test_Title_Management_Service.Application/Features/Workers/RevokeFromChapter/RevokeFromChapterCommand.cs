namespace DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromChapter;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на отзыв работника с конкретной роли на конкретной главе тайтла
/// </summary>
/// <param name="WorkerId">Идентификатор работника</param>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="ChapterId">Идентификатор главы</param>
/// <param name="Role">Роль</param>
public record RevokeFromChapterCommand(Guid WorkerId, Guid TitleId, Guid ChapterId, WorkerRole Role);