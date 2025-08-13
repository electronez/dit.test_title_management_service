namespace DIT.Test_Title_Management_Service.Domain.Titles;

/// <summary>
/// Тайтл
/// </summary>
public class Title
{
    private readonly List<Chapter> chapters = [];

    private Title()
    {
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public TitleName Name { get; private set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Главы
    /// </summary>
    public IReadOnlyCollection<Chapter> Chapters => this.chapters.AsReadOnly();

    /// <summary>
    /// Обновляет информацию о тайтле
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="description">Описание</param>
    public void Update(TitleName name, string? description)
    {
        this.Name = name;
        this.Description = description;
    }

    /// <summary>
    /// Добавляет главу к тайтлу
    /// </summary>
    /// <param name="number">Номер главы</param>
    /// <param name="name">Наименование главы</param>
    /// <returns>Глава тайтла</returns>
    public Chapter AddChapter(int number, string? name = null)
    {
        var chapter = Chapter.Create(this, number, name);
        this.chapters.Add(chapter);
        return chapter;
    }

    /// <summary>
    /// Обновляет информацию о главе
    /// </summary>
    /// <param name="chapter">Глава</param>
    /// <param name="number">Номер</param>
    /// <param name="name">Наименование</param>
    public Result UpdateChapter(Chapter chapter, int number, string? name = null)
    {
        var chapterInTitle = this.chapters.SingleOrDefault(x => x.Id == chapter.Id);
        if (chapterInTitle is null)
        {
            return Result.Fail("Глава не найдена");
        }

        chapterInTitle.Update(number, name);
        return Result.Ok();
    }

    /// <summary>
    /// Удаляет главу
    /// </summary>
    /// <param name="chapter">Глава</param>
    public void RemoveChapter(Chapter chapter)
    {
        this.chapters.Remove(chapter);
    }

    /// <summary>
    /// Создаёт новый тайтл
    /// </summary>
    /// <param name="name">Наименование</param>
    /// <param name="description">Описание</param>
    /// <returns>Тайтл</returns>
    public static Title Create(TitleName name, string? description = null)
        => new()
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Description = description,
        };
}