using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using System.Net;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public void AddUser(UserModel user, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }

    public bool AuthenticateUser(NetworkCredential networkCredential, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int id, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }

    public UserModel GetUserById(int id, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }

    public UserModel GetUserByUserName(string userName, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }

    public UserModel? Login(string login, string password, IDatabaseOptions? databaseOptions)
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

    public void UpdateUser(UserModel user, IDatabaseOptions? databaseOptions)
    {
        throw new NotImplementedException();
    }
}
