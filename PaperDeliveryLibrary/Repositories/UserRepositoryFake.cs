using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public UserModel? Login(string loginName, string password)
    {
        UserModel? output = new()
        {
            LoginName = loginName,
            Password = password,
            Email = "karwenzman@gmx.net",
            DisplayName = "karwenzman"
        };

        return output;
    }
}
