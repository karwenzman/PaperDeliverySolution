namespace PaperDeliveryLibrary.Models;

public interface IUserModel
{
    /// <summary>
    /// This property contains the user's email address.
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// This property contains the user's login expression.
    /// </summary>
    string LoginName { get; set; }

    /// <summary>
    /// This property contains the user's display name.
    /// </summary>
    string DisplayName { get; set; }

    /// <summary>
    /// This property contains the user's password.
    /// </summary>
    string Password { get; set; }
}