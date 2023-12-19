using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public UserModel? Login(string login, string password, IApplicationOptions? applicationOptions, IDatabaseOptions? databaseOptions)
    {
        UserModel? output;

        if (login == "karwenzman")
        {
            output = new()
            {
                Id = 1,
                Login = login,
                Password = password,
                Email = "karwenzman@gmx.net",
                DisplayName = "karwenzman"
            };
        }
        else if (login == "Thorsten")
        {
            output = new()
            {
                Id = 2,
                Login = login,
                Password = password,
                Email = "jenning.thorsten@gmx.net",
                DisplayName = "Thorsten"
            };
        }
        else if (login == "guest")
        {
            output = new()
            {
                Id = 3,
                Login = login,
                Password = password,
                Email = "guest@gmail.com",
                DisplayName = "guest"
            };
        }
        else
        {
            output = new();
        }

        return output;
    }
}
