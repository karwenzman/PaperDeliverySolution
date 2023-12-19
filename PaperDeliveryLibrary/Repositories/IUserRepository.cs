using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;

namespace PaperDeliveryWpf.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// This method is using an Access database to validate the application's access.
        /// </summary>
        /// <param name="login">The user's unique login expression.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="applicationOptions">The application options stored in configuration file.</param>
        /// <param name="databaseOptions">The database options stored in configuration file.</param>
        /// <returns>Null - if no valid user is found.</returns>
        UserModel? Login(string login, string password, IDatabaseOptions? databaseOptions = null);
    }
}