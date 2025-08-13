namespace DIT.Test_Title_Management_Service.Application.Features.Titles.DeleteTitle;

/// <summary>
/// Команда на удаление тайтла
/// </summary>
/// <param name="Id">Идентификатор тайтла</param>
public record DeleteTitleCommand(Guid Id);