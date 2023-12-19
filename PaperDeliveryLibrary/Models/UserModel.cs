namespace PaperDeliveryLibrary.Models;

public class UserModel : ModelBase, IUserModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int AccessLevel { get; set; } = 0;
}
