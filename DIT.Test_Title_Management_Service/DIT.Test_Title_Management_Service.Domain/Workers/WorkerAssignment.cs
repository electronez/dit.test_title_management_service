namespace DIT.Test_Title_Management_Service.Domain.Workers;

using DIT.Test_Title_Management_Service.Domain.Enums;

/// <summary>
/// Назначение работника
/// </summary>
public class WorkerAssignment
{
    private WorkerAssignment()
    {
        // ORM only
    }

    internal WorkerAssignment(Guid workerId, WorkerRole role, Guid titleId, Guid? chapterId)
    {
        this.Id = Guid.CreateVersion7();
        this.WorkerId = workerId;
        this.Role = role;
        this.TitleId = titleId;
        this.ChapterId = chapterId;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Идентификатор работника
    /// </summary>
    public Guid WorkerId { get; protected set; }

    /// <summary>
    /// Роль
    /// </summary>
    public WorkerRole Role { get; protected set; }

    /// <summary>
    /// Идентификатор тайтла
    /// </summary>
    public Guid TitleId { get; protected set; }

    /// <summary>
    /// Идентификатор главы
    /// </summary>
    /// <remarks>
    /// Если значение равно null, считаем, что назначение относится к тайтлу в целом (ко всем главам)
    /// </remarks>
    public Guid? ChapterId { get; protected set; }
}