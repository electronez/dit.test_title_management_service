namespace DIT.Test_Title_Management_Service.Domain.Titles;

/// <summary>
/// VO Наименования тайтла
/// </summary>
public class TitleName
{
    public TitleName(string originalName, string? englishName = null, string? localizedName = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(originalName, nameof(originalName));
        this.OriginalName = originalName.Trim();
        this.EnglishName = englishName?.Trim();
        this.LocalizedName = localizedName?.Trim();
    }

    /// <summary>
    /// Оригинальное наименование
    /// </summary>
    public string OriginalName { get; init; }

    /// <summary>
    /// Наименование на английском
    /// </summary>
    public string? EnglishName { get; init; }

    /// <summary>
    /// Локализованное наименование
    /// </summary>
    public string? LocalizedName { get; init; }
}