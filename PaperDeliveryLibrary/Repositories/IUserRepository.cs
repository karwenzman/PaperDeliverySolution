using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.Repositories
{
    public interface IUserRepository
    {
        UserModel? Login(string login, string password);
    }
}