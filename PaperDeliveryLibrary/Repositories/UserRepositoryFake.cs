using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public UserModel? Login(string loginName, string password)
    {
        UserModel? output;

        if (loginName == "karwenzman")
        {
            output = new()
            {
                Id = 1,
                Login = loginName,
                Password = password,
                Email = "karwenzman@gmx.net",
                Name = "karwenzman"
            };
        }
        else if (loginName == "Thorsten")
        {
            output = new()
            {
                Id = 2,
                Login = loginName,
                Password = password,
                Email = "jenning.thorsten@gmx.net",
                Name = "Thorsten"
            };
        }
        else if (loginName == "guest")
        {
            output = new()
            {
                Id = 3,
                Login = loginName,
                Password = password,
                Email = "guest@gmail.com",
                Name = "guest"
            };
        }
        else
        {
            output = new();
        }

        return output;
    }
}
