namespace DIT.Test_Title_Management_Service.Application.Features.Workers.RevokeFromTitle;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Команда на отзыв работника с конкретной роли на тайтле
/// </summary>
/// <param name="WorkerId">Идентификатор работника</param>
/// <param name="TitleId">Идентификатор тайтла</param>
/// <param name="Role">Роль</param>
public record RevokeFromTitleCommand(Guid WorkerId, Guid TitleId, WorkerRole Role);