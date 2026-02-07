namespace SimpleExample.Domain.Entities;

public class User : BaseEntity
{
    // Private setterit - vain entiteetti voi p‰ivitt‰‰ arvoja
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    // Paramiteriton konstruktori EF Core:a varten
    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }

    // Julkinen konstruktori uuden k‰ytt‰j‰n luomiseen
    public User(string firstName, string lastName, string email)
    {
        // K‰ytet‰‰n validoivia metodeja - ei koodin toistoa!
        UpdateBasicInfo(firstName, lastName);
        UpdateEmail(email);
    }
    /// <summary>
    /// P‰ivitt‰‰ k‰ytt‰j‰n perustiedot (etu- ja sukunimi)
    /// </summary>
    public void UpdateBasicInfo(string firstName, string lastName)
    {
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        if (firstName.Length < 3)
            throw new ArgumentException("First name must be at least 3 characters long.", nameof(firstName));

        if (lastName.Length < 3)
            throw new ArgumentException("Last name must be at least 3 characters long.", nameof(lastName));

        if (firstName.Length > 100)
            throw new ArgumentException("First name can be at most 100 characters long.", nameof(firstName));

        if (lastName.Length > 100)
            throw new ArgumentException("Last name can be at most 100 characters long.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// P‰ivitt‰‰ k‰ytt‰j‰n s‰hkˆpostiosoitteen
    /// </summary>
    public void UpdateEmail(string email)
    {
        ArgumentNullException.ThrowIfNull(email);

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email must be valid.", nameof(email));

        if (!email.Contains('@'))
            throw new ArgumentException("Email must be valid.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email can be at most 255 characters long.", nameof(email));

        Email = email;
    }
}