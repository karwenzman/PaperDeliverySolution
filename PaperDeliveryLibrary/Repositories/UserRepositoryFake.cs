using PaperDeliveryLibrary.Models;
using System.Net;

namespace PaperDeliveryWpf.Repositories;

public class UserRepositoryFake : IUserRepository
{
    public bool Add(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool Authenticate(NetworkCredential networkCredential)
    {
        throw new NotImplementedException();
    }

    public bool Delete(UserModel? user)
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

    public UserModel? Login(string userName, string password)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAccount(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool UpdateLastLogin(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool UpdateLastModified(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool UpdatePassword(UserModel? user)
    {
        throw new NotImplementedException();
    }
}
