using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public UserModel? Login(string loginName, string password)
    {
        UserModel? output = new()
        {
            Login = loginName,
            Password = password,
            Email = "karwenzman@gmx.net",
            Name = "karwenzman"
        };

        return output;
    }
}
