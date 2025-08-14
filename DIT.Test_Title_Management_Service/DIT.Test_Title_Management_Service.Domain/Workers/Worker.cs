namespace DIT.Test_Title_Management_Service.Domain.Workers;

using DIT.Test_Title_Management_Service.Domain.Enums;
using DIT.Test_Title_Management_Service.Domain.Titles;

/// <summary>
/// Работник
/// </summary>
public class Worker
{
    private readonly List<WorkerAssignment> assignments = [];
    private readonly List<WorkerRole> roles = [];

    private Worker()
    {
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// Уникальное имя работника
    /// </summary>
    public string Username { get; private init; }

    /// <summary>
    /// Профиль работника
    /// </summary>
    public Profile Profile { get; private set; }

    /// <summary>
    /// Доступные роли
    /// </summary>
    public IReadOnlyCollection<WorkerRole> Roles => this.roles;

    /// <summary>
    /// Назначения
    /// </summary>
    public IReadOnlyCollection<WorkerAssignment> Assignments => this.assignments.AsReadOnly();

    /// <summary>
    /// Обновляет профиль работника
    /// </summary>
    /// <param name="profile">Профиль работника</param>
    public void UpdateProfile(Profile profile)
    {
        this.Profile = profile;
    }

    /// <summary>
    /// Назначает роль работнику
    /// </summary>
    /// <param name="role">Роль</param>
    public void AssignRole(WorkerRole role)
    {
        if (!this.roles.Contains(role))
        {
            this.roles.Add(role);
        }
    }

    /// <summary>
    /// Отзывает роль работника
    /// </summary>
    /// <param name="role">Роль</param>
    public void RevokeRole(WorkerRole role)
    {
        // Правило: при снятии роли с работника все назначения также снимаются
        if (this.roles.Remove(role))
        {
            this.assignments.RemoveAll(x => x.Role == role);
        }
    }

    /// <summary>
    /// Назначает работника на тайтл на указанную роль
    /// </summary>
    /// <param name="title">Тайтл</param>
    /// <param name="role">Роль</param>
    public Result AssignToTitle(Title title, WorkerRole role)
    {
        var validationResult = this.ValidateAssign(title.Id, null, role);
        if (validationResult is not null)
        {
            return validationResult;
        }

        // Если назначаем работника на весь тайтл, то все индивидуальные назначения отзываем
        var assignmentsOnChapters = this.assignments
            .Where(x => x.TitleId == title.Id && x.ChapterId is not null && x.Role == role)
            .ToArray();
        foreach (var assignmentsOnChapter in assignmentsOnChapters)
        {
            this.assignments.Remove(assignmentsOnChapter);
        }

        this.assignments.Add(new WorkerAssignment(this.Id, role, title.Id, null));
        return Result.Ok();
    }

    /// <summary>
    /// Назначает работника на главу тайтла на указанную роль
    /// </summary>
    /// <param name="chapter">Глава тайтла</param>
    /// <param name="role">Роль</param>
    public Result AssignToChapter(Chapter chapter, WorkerRole role)
    {
        // Правило: мы не можем назначить работника на главу в случаях, когда
        // 1) работник находится на этой роли в рамках всего тайтла
        // 2) работник уже находится в этой роли в этой главе
        var validationResult = this.ValidateAssign(chapter.TitleId, null, role)
                               ?? this.ValidateAssign(chapter.TitleId, chapter.Id, role);
        if (validationResult is not null)
        {
            return validationResult;
        }

        this.assignments.Add(new WorkerAssignment(this.Id, role, chapter.TitleId, chapter.Id));
        return Result.Ok();
    }

    /// <summary>
    /// Отзывает работника с указанной роли в тайтле
    /// </summary>
    /// <param name="title">Тайтл</param>
    /// <param name="role">Роль</param>
    public void RevokeFromTitle(Title title, WorkerRole role)
    {
        var assignment = this.assignments.SingleOrDefault(x => x.TitleId == title.Id && x.Role == role);
        if (assignment is not null)
        {
            this.assignments.Remove(assignment);
        }
    }

    /// <summary>
    /// Отзывает работника с указанной роли в главе тайтла
    /// </summary>
    /// <param name="chapter">Глава</param>
    /// <param name="role">Роль</param>
    public void RevokeFromChapter(Chapter chapter, WorkerRole role)
    {
        var assignment = this.assignments.SingleOrDefault(x => x.ChapterId == chapter.Id && x.Role == role);
        if (assignment is not null)
        {
            this.assignments.Remove(assignment);
        }
    }

    private Result? ValidateAssign(Guid titleId, Guid? chapterId, WorkerRole role)
    {
        // Правило: если у работника нет выбранной роли в списке доступных,
        // то мы не можем назначить работника на эту роль
        if (!this.roles.Contains(role))
        {
            return Result.Fail("Работник не может быть назначен на эту роль");
        }

        // Правило: если работник уже назначен объект, мы не можем назначить повторно
        if (this.assignments.Any(x => x.TitleId == titleId && x.Role == role && x.ChapterId == chapterId))
        {
            return Result.Fail("Работник уже назначен на эту роль");
        }

        return null;
    }

    /// <summary>
    /// Создаёт нового работника
    /// </summary>
    /// <param name="username">Уникальное имя работника</param>
    /// <param name="profile">Профиль работника</param>
    /// <param name="roles">Роли</param>
    /// <returns></returns>
    public static Worker Create(string username, Profile profile, IEnumerable<WorkerRole> roles)
    {
        var worker = new Worker
        {
            Id = Guid.CreateVersion7(),
            Username = username,
            Profile = profile,
        };

        foreach (var role in roles)
        {
            worker.AssignRole(role);
        }

        return worker;
    }
}