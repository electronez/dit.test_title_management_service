namespace DIT.Test_Title_Management_Service.Domain.Workers;

/// <summary>
/// VO Профиля работника
/// </summary>
public class Profile
{
    public Profile(string firstName, string lastName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));
        this.FirstName = firstName.Trim();
        this.LastName = lastName.Trim();
    }

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; }
}