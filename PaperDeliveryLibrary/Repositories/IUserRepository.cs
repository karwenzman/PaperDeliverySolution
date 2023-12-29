using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using System.Collections.ObjectModel;
using System.Net;

namespace PaperDeliveryWpf.Repositories
{
    /// <summary>
    /// This interface is providing members to access a database.
    /// <para></para>
    /// Via constructor injection the needed credentials to access the database are provided.
    /// <br></br>All classes implementing this interface have to make use of <see cref="IOptions{TOptions}"/>.
    /// <br></br>The app's dependency injection system must inject the corresponding <see cref="IDatabaseOptions"/>.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// This method is validating the user's access to the application.
        /// <para></para>
        /// Two properties are checked for this validation:
        /// <br></br>-Password
        /// <br></br>-IsActive
        /// </summary>
        /// <param name="networkCredential">This credential contains the user name and the user password.</param>
        /// <returns>true, if the authentication was successful, otherwise false</returns>
        bool Authenticate(NetworkCredential networkCredential);

        /// <summary>
        /// This method is accessing the database and returning all data records.
        /// </summary>
        /// <returns>a list of data records</returns>
        ObservableCollection<UserModel> GetAllRecords();

        /// <summary>
        /// This method is accessing the database using the user's ID.
        /// </summary>
        /// <param name="userId">The user's unique ID.</param>
        /// <returns>null, if no valid user is found</returns>
        UserModel? GetById(int userId);

        /// <summary>
        /// This method is accessing the database using the user's login expression.
        /// </summary>
        /// <param name="userName">The user's unique user name.</param>
        /// <returns>null, if no valid user is found</returns>
        UserModel? GetByUserName(string? userName);

        /// <summary>
        /// This method is adding a new user account to the database.
        /// </summary>
        /// <param name="user">A new user account.</param>
        /// <returns>true, if database access was successful</returns>
        bool Add(UserModel? user);

        /// <summary>
        /// This method is updating a user account.
        /// <para></para>
        /// These properties are updated:
        /// <br></br>-DisplayName
        /// <br></br>-Role
        /// <br></br>-Email
        /// <br></br>-LastModified
        /// <br></br>-IsActive
        /// </summary>
        /// <param name="user">An updated user account.</param>
        /// <returns>true, if database access was successful</returns>
        bool UpdateAccount(UserModel? user);

        /// <summary>
        /// This method is updating a user's 'LastLogin' property, only>.
        /// <para></para>
        /// This is the only method that updates the user's 'LastLogin' property.
        /// <para></para>
        /// These properties are updated:
        /// <br></br>-LastLogin
        /// </summary>
        /// <param name="user">An updated user account.</param>
        /// <returns>true, if database access was successful</returns>
        bool UpdateLastLogin(UserModel? user);

        /// <summary>
        /// This method is updating a user's 'Password' property, only>.
        /// <para></para>
        /// This is the only method that updates the user's 'Password' property.
        /// <para></para>
        /// These properties are updated:
        /// <br></br>-Password
        /// <br></br>-LastModified
        /// </summary>
        /// <param name="user">An updated user account.</param>
        /// <returns>true, if database access was successful</returns>
        bool UpdatePassword(UserModel? user);

        /// <summary>
        /// This method is setting a user account to 'IsActive'.
        /// <para></para>
        /// This is the only method that updates the user's 'IsActive' property.
        /// <para></para>
        /// These properties are updated:
        /// <br></br>-IsActive
        /// <br></br>-LastModified
        /// </summary>
        /// <param name="user">A new user account.</param>
        /// <returns>true, if database access was successful</returns>
        bool Delete(UserModel? user);
    }
}