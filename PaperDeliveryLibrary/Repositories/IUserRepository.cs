using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using System.Net;

namespace PaperDeliveryWpf.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method is using an Access database to validate the application's access.
        /// </summary>
        /// <param name="login">The user's unique login expression.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="databaseOptions">The database options stored in configuration file.</param>
        /// <returns>Null - if no valid user is found.</returns>
        UserModel? Login(string login, string password, IDatabaseOptions? databaseOptions = null);

        bool AuthenticateUser(NetworkCredential networkCredential, IDatabaseOptions? databaseOptions = null);
        UserModel GetUserById(int id, IDatabaseOptions? databaseOptions = null);
        UserModel GetUserByUserName(string userName, IDatabaseOptions? databaseOptions = null);
        void AddUser(UserModel user, IDatabaseOptions? databaseOptions = null);
        void UpdateUser(UserModel user, IDatabaseOptions? databaseOptions = null);
        void DeleteUser(int id, IDatabaseOptions? databaseOptions = null);
    }
}