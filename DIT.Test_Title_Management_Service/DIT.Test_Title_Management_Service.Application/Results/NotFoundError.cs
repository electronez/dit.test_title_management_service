namespace DIT.Test_Title_Management_Service.Application.Results;

public class NotFoundError(string message) : Error(message);

public class NotFoundError<TType, TId>(TId id)
    : NotFoundError($"Сущность {typeof(TType).Name} с идентификатором {id} не найдена");