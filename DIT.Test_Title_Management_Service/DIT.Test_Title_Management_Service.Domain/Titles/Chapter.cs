namespace DIT.Test_Title_Management_Service.Domain.Titles;

/// <summary>
/// Глава
/// </summary>
public class Chapter
{
    internal Chapter()
    {
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private init; }

    /// <summary>
    /// Идентификатор тайтла
    /// </summary>
    public Guid TitleId { get; private init; }

    /// <summary>
    /// Тайтл
    /// </summary>
    public Title Title { get; private init; }

    /// <summary>
    /// Номер
    /// </summary>
    public int Number { get; private set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Обновляет информацию о главе
    /// </summary>
    /// <param name="number"></param>
    /// <param name="name"></param>
    public void Update(int number, string? name)
    {
        this.Number = number;
        this.Name = name?.Trim();
    }

    internal static Chapter Create(Title title, int number, string? name)
        => new()
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            TitleId = title.Id,
            Number = number,
            Name = name?.Trim()
        };
}