namespace PaperDeliveryLibrary.Models;

/// <summary>
/// This class is used to store the information about the logged in user.
/// </summary>
public interface IUserModel
{
    /// <summary>
    /// This property contains the ID needed for database access.
    /// <para></para>
    /// This is the primary key in a database table.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// This property contains the user's role for the application.
    /// </summary>
    string? Role { get; set; }

    /// <summary>
    /// This property contains the user's email address.
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    /// This property contains the user's login expression.
    /// <para></para>
    /// This is an alternative key in a database table.
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    /// This property contains the user's display name.
    /// </summary>
    string DisplayName { get; set; }

    /// <summary>
    /// This property contains the user's password.
    /// </summary>
    string Password { get; set; }

    /// <summary>
    ///  This property contains the user's last login to the application.
    /// </summary>
    string LastLogin { get; set; }

    /// <summary>
    /// This property contains the user's last modification of its account.
    /// </summary>
    string LastModified { get; set; }
}