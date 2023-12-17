using PaperDeliveryLibrary.Models;

namespace PaperDeliveryWpf.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method is validating the access to the application.
        /// </summary>
        /// <param name="login">The user's unique login expression.</param>
        /// <param name="password">The user's password.</param>
        /// <returns></returns>
        UserModel? Login(string login, string password);
    }
}