using PaperDeliveryLibrary.Models;
using System.Net;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public void Add(UserModel user)
    {
        throw new NotImplementedException();
    }

    public bool Authenticate(NetworkCredential networkCredential)
    {
        throw new NotImplementedException();
    }

    public void Delete(int userId)
    {
        throw new NotImplementedException();
    }

    public UserModel? GetById(int userId)
    {
        throw new NotImplementedException();
    }

    public UserModel? GetByUserName(string? userName)
    {
        throw new NotImplementedException();
    }

    public string[]? GetUserRoles(string? userRole)
    {
        throw new NotImplementedException();
    }

    public UserModel? Login(string userName, string password)
    {
        UserModel? output;

        if (userName == "karwenzman")
        {
            output = new()
            {
                Id = 1,
                UserName = userName,
                Password = password,
                Email = "karwenzman@gmx.net",
                DisplayName = "karwenzman"
            };
        }
        else if (userName == "Thorsten")
        {
            output = new()
            {
                Id = 2,
                UserName = userName,
                Password = password,
                Email = "jenning.thorsten@gmx.net",
                DisplayName = "Thorsten"
            };
        }
        else if (userName == "guest")
        {
            output = new()
            {
                Id = 3,
                UserName = userName,
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

    public void Update(UserModel user)
    {
        throw new NotImplementedException();
    }
}
