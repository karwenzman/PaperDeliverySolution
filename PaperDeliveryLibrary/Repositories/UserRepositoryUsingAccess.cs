using Microsoft.Extensions.Options;
using PaperDeliveryLibrary.Models;
using PaperDeliveryLibrary.ProjectOptions;
using PaperDeliveryWpf.Repositories;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Net;
using System.Runtime.Versioning;

namespace PaperDeliveryLibrary.Repositories;

/// <summary>
/// This class is providing members inherited from <see cref="IUserRepository"/>.
/// <para></para>
/// This class is explicit addressing an Access database.
/// <para></para>
/// ConnectionStrings for AcccessDb from www.connectionstrings.com
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;Persist Security Info=False;
/// <br></br>Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\myFolder\\myAccessFile.accdb;Jet OLEDB:Database Password=MyDbPassword;
/// <br></br>this is standard value: Persist Security Info=False
/// </summary>
[SupportedOSPlatform("windows")]
public class UserRepositoryUsingAccess : IUserRepository
{
    private readonly IOptions<DatabaseOptionsUsingAccess> _databaseOptions;

    public UserRepositoryUsingAccess(IOptions<DatabaseOptionsUsingAccess> databaseOptions)
    {
        _databaseOptions = databaseOptions;
    }

    public bool Authenticate(NetworkCredential networkCredential)
    {
        bool output;

        // Validate parameters.
        ArgumentNullException.ThrowIfNullOrWhiteSpace(networkCredential.UserName, nameof(networkCredential.UserName));
        ArgumentNullException.ThrowIfNullOrEmpty(networkCredential.Password, nameof(networkCredential.Password));
        ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable} WHERE UserName = '{networkCredential.UserName}'";

        // Read from database.
        UserModel userModel = new();
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();
            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Values can not be null.
                userModel.Password = (string)reader["Password"];
                userModel.IsActive = (bool)reader["IsActive"];
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        // Validate password and permission.
        if (networkCredential.Password == userModel.Password && userModel.IsActive == true)
        {
            output = true;
        }
        else
        {
            output = false;
        }

        return output;
    }

    public ObservableCollection<UserModel> GetAllRecords()
    {
        ObservableCollection<UserModel> output = [];

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable}";

        // Read from database.
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();
            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                UserModel userModel = new()
                {
                    // Values can not be null.
                    Id = (int)reader["ID"],
                    UserName = (string)reader["UserName"],
                    Password = "**********", // never publish the password; except when authenticating
                    DisplayName = (string)reader["DisplayName"],
                    Role = (string)reader["Role"],
                    IsActive = (bool)reader["IsActive"],

                    // Values can be null.
                    Email = DBNull.Value.Equals(reader["Email"]) ? null : (string)reader["Email"],
                    LastLogin = DBNull.Value.Equals(reader["LastLogin"]) ? null : (string)reader["LastLogin"],
                    LastModified = DBNull.Value.Equals(reader["LastModified"]) ? null : (string)reader["LastModified"]
                };

                output.Add(userModel);
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }


        return output;
    }

    public UserModel? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public UserModel? GetByUserName(string? userName)
    {
        UserModel? output = new();

        // Validate parameters.
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
        ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"SELECT * FROM {validatedTable} WHERE UserName = '{userName}'";

        // Read from database.
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        try
        {
            connection.Open();
            using OleDbDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Values can not be null.
                output.Id = (int)reader["ID"];
                output.UserName = (string)reader["UserName"];
                output.Password = "**********"; // never publish the password; except when authenticating
                //output.Password = (string)reader["Password"];
                output.DisplayName = (string)reader["DisplayName"];
                output.Role = (string)reader["Role"];
                output.IsActive = (bool)reader["IsActive"];

                // Values can be null.
                output.Email = DBNull.Value.Equals(reader["Email"]) ? null : (string)reader["Email"];
                output.LastLogin = DBNull.Value.Equals(reader["LastLogin"]) ? null : (string)reader["LastLogin"];
                output.LastModified = DBNull.Value.Equals(reader["LastModified"]) ? null : (string)reader["LastModified"];
            }

            reader.Close();
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        // Validate user account.
        if (!output.IsActive)
        {
            return output = null;
        }

        // Return UserModel.
        return output;
    }

    public bool Add(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool UpdateAccount(UserModel? userAccount)
    {
        bool output;

        // Validate parameters.
        ArgumentNullException.ThrowIfNull(userAccount, nameof(userAccount));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"UPDATE {validatedTable} SET " +
            $"DisplayName = @DisplayName," +
            $"Role = @Role," +
            $"Email = @Email," +
            $"LastModified = @LastModified, " +
            $"IsActive = @IsActive " +
            $"WHERE UserName = '{userAccount.UserName}'";

        //  Update database.
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        command.Parameters.AddWithValue("@DisplayName", userAccount.DisplayName);
        command.Parameters.AddWithValue("@Role", userAccount.Role);
        command.Parameters.AddWithValue("@Email", userAccount.Email);
        command.Parameters.AddWithValue("@LastModified", DateTime.Now.ToString());
        command.Parameters.AddWithValue("@IsActive", userAccount.IsActive);
        try
        {
            connection.Open();
            if (command.ExecuteNonQuery() == 0)
            {
                output = false;
            }
            else
            {
                output = true;
            }
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        return output;
    }

    public bool UpdateLastLogin(UserModel? userAccount)
    {
        bool output;

        // Validate parameters.
        ArgumentNullException.ThrowIfNull(userAccount, nameof(userAccount));

        // Setup connection strings.
        string validatedPath = ValidatePath(_databaseOptions.Value);
        string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

        string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
        string queryString = $"UPDATE {validatedTable} SET " +
            $"LastLogin = @LastLogin " +
            $"WHERE UserName = '{userAccount.UserName}'";

        //  Update database.
        using var connection = new OleDbConnection(connectionString);
        OleDbCommand command = new(queryString, connection);
        command.Parameters.AddWithValue("@LastLogin", DateTime.Now.ToString());
        try
        {
            connection.Open();
            if (command.ExecuteNonQuery() == 0)
            {
                output = false;
            }
            else
            {
                output = true;
            }
            connection.Close();
        }
        catch (OleDbException ex)
        {
            if (ex.ErrorCode == -2147217843)
            {
                throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
                    $"Error code: {ex.ErrorCode}");
            }

            throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
        }

        return output;
    }

    public bool UpdatePassword(UserModel? user)
    {
        throw new NotImplementedException();
    }

    public bool Delete(UserModel? user)
    {
        throw new NotImplementedException();
    }

    //public UserModel? Login(string userName, string password)
    //{
    //    // Validate parameters.
    //    ArgumentNullException.ThrowIfNullOrWhiteSpace(userName, nameof(userName));
    //    ArgumentNullException.ThrowIfNullOrWhiteSpace(password, nameof(password));
    //    ArgumentNullException.ThrowIfNull(_databaseOptions, nameof(DatabaseOptionsUsingAccess));

    //    // Setup connection strings.
    //    string validatedPath = ValidatePath(_databaseOptions.Value);
    //    string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={validatedPath};Jet OLEDB:Database Password={_databaseOptions.Value.DatabasePassword};";

    //    string validatedTable = ValidateTable(_databaseOptions.Value, connectionString);
    //    string queryString = $"SELECT * FROM {validatedTable} WHERE UserName = '{userName}'";

    //    // Read from database.
    //    UserModel? output = new();
    //    using var connection = new OleDbConnection(connectionString);
    //    OleDbCommand command = new(queryString, connection);
    //    try
    //    {
    //        connection.Open();

    //        using OleDbDataReader reader = command.ExecuteReader();

    //        while (reader.Read())
    //        {
    //            // Values can not be null.
    //            output.Id = (int)reader["ID"];
    //            output.UserName = (string)reader["UserName"];
    //            output.Password = (string)reader["Password"];
    //            output.DisplayName = (string)reader["DisplayName"];
    //            output.Email = (string)reader["Email"];
    //            output.IsActive = (bool)reader["IsActive"];

    //            // Values can be null.
    //            output.Role = DBNull.Value.Equals(reader["Email"]) ? null : (string)reader["Role"];
    //        }

    //        reader.Close();
    //        connection.Close();
    //    }
    //    catch (OleDbException ex)
    //    {
    //        if (ex.ErrorCode == -2147217843)
    //        {
    //            throw new UnauthorizedAccessException($"Invalid Password! No access to {validatedPath}! " +
    //                $"Error code: {ex.ErrorCode}");
    //        }

    //        throw new UnauthorizedAccessException($"No access to {validatedPath}! Error code: {ex.ErrorCode}");
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"Unexpected exception while accessing the database! Message: {ex.Message}");
    //    }

    //    // Validate password.
    //    if (password != output.Password)
    //    {
    //        output = null;
    //    }

    //    // Return UserModel.
    //    return output;
    //}

    private static string ValidateTable(DatabaseOptionsUsingAccess databaseOptions, string connectionString)
    {
        string output = databaseOptions.DatabaseTable;

        var listOfTables = CaptureDatabaseTables(connectionString);

        if (!listOfTables.Contains(output))
        {
            throw new ArgumentException("Table not found in database.", databaseOptions.DatabaseTable);
        }

        return output;
    }

    private static string ValidatePath(DatabaseOptionsUsingAccess databaseOptions)
    {
        string ouptput = databaseOptions.DatabasePath;

        if (!File.Exists(ouptput))
        {
            throw new ArgumentException("Database not found.", databaseOptions.DatabasePath);
        }

        return ouptput;
    }

    /// <summary>
    /// This method is capturing the user tables from the database provided by the configuration file.
    /// </summary>
    /// <param name="connectionString"></param>
    /// <returns>A list of user tables. The list is empty, if no user tables are found.</returns>
    /// <exception cref="Exception">If an unexpected exception is thrown.</exception>
    private static List<string> CaptureDatabaseTables(string connectionString)
    {
        List<string> output = [];

        using var connection = new OleDbConnection(connectionString);
        try
        {
            connection.Open();

            //var dbSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, []); // optional syntax
            var dbOtherSchemaTables = connection.GetSchema("Tables");

            if (dbOtherSchemaTables == null)
            {
                return output;
            }
            if (dbOtherSchemaTables.Rows.Count == 0)
            {
                return output;
            }

            for (int i = 0; i < dbOtherSchemaTables.Rows.Count; i++)
            {
                output.Add(dbOtherSchemaTables.Rows[i][2].ToString()!);
            }

            List<int> rowsToRemove = [];

            for (int i = 0; i < output.Count; i++)
            {
                if (output[i].Contains("MSys") || output[i].Contains("USys"))
                {
                    rowsToRemove.Add(i);
                }
            }

            for (int i = rowsToRemove.Count - 1; i >= 0; i--)
            {
                output.RemoveAt(rowsToRemove[i]);
            }
            connection.Close();
        }
        catch (Exception ex)
        {
            string message = $"An unexpected exception was thrown in <{nameof(CaptureDatabaseTables)}>.";
            throw new Exception(message, ex);
        }

        return output;
    }
}
