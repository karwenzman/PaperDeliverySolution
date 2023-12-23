﻿using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
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
        /// For Testing ONLY.
        /// This method is using an Access database to validate the application's access.
        /// </summary>
        /// <param name="userName">The user's unique user name expression.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>Null - if no valid user is found.</returns>
        UserModel? Login(string userName, string password);

        /// <summary>
        /// This method is validating the user's access to the application.
        /// </summary>
        /// <param name="networkCredential">This credential contains the user name and the user password.</param>
        /// <returns>true, if the authentication was successful, otherwise false</returns>
        bool Authenticate(NetworkCredential networkCredential);

        /// <summary>
        /// This method is accessing the database using the user's ID.
        /// </summary>
        /// <param name="userId">The user's unique ID.</param>
        /// <returns>null, if no valid user is found</returns>
        UserModel? GetById(int userId);

        /// <summary>
        /// This method is accessing the database using the user's user name.
        /// </summary>
        /// <param name="userName">The user's unique user name.</param>
        /// <returns>null, if no valid user is found</returns>
        UserModel? GetByUserName(string userName);

        void Add(UserModel user);
        void Update(UserModel user);
        void Delete(int userId);
    }
}