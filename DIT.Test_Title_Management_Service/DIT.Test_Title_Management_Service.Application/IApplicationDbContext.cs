namespace DIT.Test_Title_Management_Service.Application;

using DIT.Test_Title_Management_Service.Domain.Titles;
using DIT.Test_Title_Management_Service.Domain.Workers;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Контекст работы с БД приложения
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Тайтлы
    /// </summary>
    DbSet<Title> Titles { get; }

    /// <summary>
    /// Работники
    /// </summary>
    DbSet<Worker> Workers { get; }

    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}