namespace PaperDeliveryLibrary.Models;

public class UserModel : ModelBase, IUserModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Email { get; set; } = null;
    public string? LastLogin { get; set; } = null;
    public string? LastModified { get; set; } = null;
}
