namespace PaperDeliveryLibrary.Models;

public class UserModel : ModelBase, IUserModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; } = null;
    public string Password { get; set; } = string.Empty;
    public string? Role { get; set; } = null;
    public string LastLogin { get; set; } = DateTime.Now.ToString();
    public string LastModified { get; set; } = DateTime.Now.ToString();
}
