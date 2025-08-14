namespace DIT.Test_Title_Management_Service.Application.Features.Titles.GetTitleById;

/// <summary>
/// Запрос на получение тайтла по идентификатору
/// </summary>
/// <param name="Id">Идентификатор тайтла</param>
public record GetTitleByIdRequest(Guid Id);